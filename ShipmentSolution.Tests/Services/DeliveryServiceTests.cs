using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Services
{
    public class DeliveryServiceTests
    {
        private ApplicationDbContext _context;
        private DeliveryService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"DeliveryTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _service = new DeliveryService(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedDeliveries()
        {
            // Arrange
            var shipment = new ShipmentEntity
            {
                ShippingMethod = "Express",
                Dimensions = "20x10x5",
                Weight = 10.0f,
                IsDeleted = false
            };

            var carrier = new MailCarrier
            {
                FirstName = "Mike",
                LastName = "Brown",
                Email = "mike.brown@example.com",
                PhoneNumber = "555-555-5555",
                Status = "Available",
                CurrentLocation = "Depot A",
                IsDeleted = false
            };

            var route = new Route
            {
                StartLocation = "A",
                EndLocation = "B",
                IsDeleted = false
            };

            var delivery1 = new Delivery
            {
                Shipment = shipment,
                MailCarrier = carrier,
                Route = route,
                DateDelivered = new DateTime(2025, 7, 1),
                IsDeleted = false
            };

            var delivery2 = new Delivery
            {
                Shipment = shipment,
                MailCarrier = carrier,
                Route = route,
                DateDelivered = new DateTime(2025, 7, 1),
                IsDeleted = true
            };

            _context.AddRange(shipment, carrier, route, delivery1, delivery2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();
            var matching = result.Where(d => d.MailCarrierName == "Mike Brown" && d.Route == "A → B").ToList();

            // Assert
            Assert.That(matching.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAsync_AddsDelivery()
        {
            // Arrange
            var testUserId = "test-user-id";

            var shipment = new ShipmentEntity
            {
                ShippingMethod = "Standard",
                Dimensions = "10x20x30",
                Weight = 5.5f,
                CreatedByUserId = testUserId,
                IsDeleted = false
            };

            var carrier = new MailCarrier
            {
                FirstName = "Anna",
                LastName = "Smith",
                Email = "anna@example.com",
                PhoneNumber = "123-456-7890",
                Status = "Available",
                CurrentLocation = "Warehouse A",
                IsDeleted = false
            };

            var route = new Route
            {
                StartLocation = "X",
                EndLocation = "Y",
                IsDeleted = false
            };

            _context.AddRange(shipment, carrier, route);
            await _context.SaveChangesAsync();

            var model = new DeliveryCreateViewModel
            {
                ShipmentId = shipment.ShipmentId,
                MailCarrierId = carrier.MailCarrierId,
                RouteId = route.RouteId,
                DateDelivered = new DateTime(2024, 1, 1)
            };

            // Act
            await _service.CreateAsync(model, testUserId);

            // Assert
            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(d => d.DateDelivered == model.DateDelivered);

            Assert.That(delivery, Is.Not.Null);
            Assert.That(delivery!.DateDelivered, Is.EqualTo(model.DateDelivered));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsCorrectViewModel()
        {
            // Arrange
            var testUserId = "test-user-id";

            var shipment = new ShipmentEntity
            {
                ShippingMethod = "Air",
                Dimensions = "50x50x20",
                Weight = 15.0f,
                CreatedByUserId = testUserId,
                IsDeleted = false
            };

            var carrier = new MailCarrier
            {
                FirstName = "Tom",
                LastName = "Taylor",
                Email = "tom.taylor@example.com",
                PhoneNumber = "777-888-9999",
                Status = "Available",
                CurrentLocation = "Hub B",
                IsDeleted = false
            };

            var route = new Route
            {
                StartLocation = "C",
                EndLocation = "D",
                IsDeleted = false
            };

            var delivery = new Delivery
            {
                Shipment = shipment,
                MailCarrier = carrier,
                Route = route,
                DateDelivered = DateTime.Today
            };

            _context.AddRange(shipment, carrier, route, delivery);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, testUserId),
        new Claim(ClaimTypes.Role, "RegisteredUser")
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            // Act
            var result = await _service.GetForEditAsync(delivery.DeliveryId, testUserId, userPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.DateDelivered, Is.EqualTo(delivery.DateDelivered));
            Assert.That(result.Shipments.Any(), Is.True);
            Assert.That(result.MailCarriers.Any(), Is.True);
            Assert.That(result.Routes.Any(), Is.True);
        }


        [Test]
        public async Task GetForEditAsync_Throws_WhenDeliveryNotFound()
        {
            // Arrange
            var testUserId = "test-user-id";
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, testUserId),
                    new Claim(ClaimTypes.Role, "RegisteredUser")
                };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() =>
             _service.GetForEditAsync(999, testUserId, userPrincipal));

            Assert.That(ex!.Message, Is.EqualTo("Delivery not found."));
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
