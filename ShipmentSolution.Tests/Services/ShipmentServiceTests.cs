using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Services
{
    public class ShipmentServiceTests
    {
        private ApplicationDbContext _context;
        private ShipmentService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Always unique DB name
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new ShipmentService(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedShipments()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Alice",
                LastName = "Walker",
                City = "New York",
                Email = "alice@example.com",
                PhoneNumber = "123-456-7890",
                PreferredShippingMethod = "Air",
                State = "NY",
                ZipCode = "10001",
                IsDeleted = false
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            var shipment1 = new ShipmentEntity
            {
                CustomerId = customer.CustomerId,
                ShippingMethod = "Air",
                ShippingCost = 25.0f,
                DeliveryDate = DateTime.Today,
                Dimensions = "10x10x10",
                IsDeleted = false
            };

            var shipment2 = new ShipmentEntity
            {
                CustomerId = customer.CustomerId,
                ShippingMethod = "Sea",
                ShippingCost = 50.0f,
                DeliveryDate = DateTime.Today,
                Dimensions = "20x20x20",
                IsDeleted = true
            };

            await _context.Shipments.AddRangeAsync(shipment1, shipment2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CustomerName, Is.EqualTo("Alice Walker"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Optional, but cleans up memory
            _context.Dispose();
        }
    }
}
