using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System;
using System.Collections.Generic;
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
        public async Task CreateAsync_AddsDelivery()
        {
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

            await _service.CreateAsync(model, testUserId);

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(d => d.DateDelivered == model.DateDelivered);

            Assert.That(delivery, Is.Not.Null);
            Assert.That(delivery!.DateDelivered, Is.EqualTo(model.DateDelivered));
        }

        
        [Test]
        public Task GetForEditAsync_Throws_WhenDeliveryNotFound()
        {
            var testUserId = "test-user-id";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "RegisteredUser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            var ex = Assert.ThrowsAsync<Exception>(() =>
                _service.GetForEditAsync(999, testUserId, userPrincipal));

            Assert.That(ex!.Message, Is.EqualTo("Delivery not found."));
            return Task.CompletedTask;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
