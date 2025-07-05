using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ShipmentSolution.Tests.Services
{
    public class CustomerServiceTests
    {
        private ApplicationDbContext _context;
        private CustomerService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"CustomerServiceDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _service = new CustomerService(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedCustomers()
        {
            // Arrange
            var testEmail = "alice@example.com";
            _context.Customers.Add(new Customer
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = testEmail,
                PhoneNumber = "123-456-7890",
                City = "City A",
                State = "State A",
                ZipCode = "10001",
                PreferredShippingMethod = "Air",
                ShippingCostThreshold = 50,
                IsDeleted = false
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllAsync();
            var alice = result.FirstOrDefault(c => c.Email == testEmail);

            // Assert
            Assert.That(alice, Is.Not.Null);
            Assert.That(alice!.FirstName, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task CreateAsync_AddsCustomerToDatabase()
        {
            // Arrange
            var model = new CustomerCreateViewModel
            {
                FirstName = "Charlie",
                LastName = "Day",
                Email = "charlie@example.com",
                PhoneNumber = "111-222-3333",
                City = "Philly",
                State = "PA",
                ZipCode = "19104",
                PreferredShippingMethod = "Air",
                ShippingCostThreshold = 100
            };

            // Act
            await _service.CreateAsync(model);

            // Assert
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == "charlie@example.com");
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer!.FirstName, Is.EqualTo("Charlie"));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsCorrectCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Bob",
                LastName = "Marley",
                Email = "bob@example.com",
                PhoneNumber = "444-555-6666",
                City = "Kingston",
                State = "JA",
                ZipCode = "00000",
                PreferredShippingMethod = "Boat",
                ShippingCostThreshold = 200,
                IsDeleted = false
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetForEditAsync(customer.CustomerId);

            // Assert
            Assert.That(result.FirstName, Is.EqualTo("Bob"));
        }

        [Test]
        public void GetForEditAsync_Throws_WhenCustomerNotFound()
        {
            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetForEditAsync(999));
        }

        [Test]
        public async Task EditAsync_UpdatesCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Carl",
                LastName = "Old",
                Email = "carl@example.com",
                PhoneNumber = "000-000-0000",
                City = "Old City",
                State = "CA",
                ZipCode = "90000",
                PreferredShippingMethod = "Ground",
                ShippingCostThreshold = 50,
                IsDeleted = false
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var model = new CustomerEditViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = "Carlos",
                LastName = "New",
                Email = "carl@example.com",
                PhoneNumber = "111-111-1111",
                City = "New City",
                State = "NY",
                ZipCode = "10001",
                PreferredShippingMethod = "Air",
                ShippingCostThreshold = 75
            };

            // Act
            await _service.EditAsync(model);

            // Assert
            var updated = await _context.Customers.FindAsync(customer.CustomerId);
            Assert.That(updated!.FirstName, Is.EqualTo("Carlos"));
            Assert.That(updated.LastName, Is.EqualTo("New"));
            Assert.That(updated.PhoneNumber, Is.EqualTo("111-111-1111"));
            Assert.That(updated.City, Is.EqualTo("New City"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
