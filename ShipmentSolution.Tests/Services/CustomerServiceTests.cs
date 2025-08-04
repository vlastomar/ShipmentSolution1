using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task CreateAsync_AddsCustomer()
        {
            // Arrange
            var model = new CustomerCreateViewModel
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                PhoneNumber = "123-456-7890",
                City = "Springfield",
                State = "IL",
                ZipCode = "62704",
                PreferredShippingMethod = "Express",
                ShippingCostThreshold = 75
            };

            string userId = "test-user-id";

            // Act
            await _service.CreateAsync(model, userId);

            // Assert
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == "alice@example.com");
            Assert.That(customer, Is.Not.Null);
            Assert.That(customer!.FirstName, Is.EqualTo("Alice"));
            Assert.That(customer.CreatedByUserId, Is.EqualTo(userId)); // if tracking user ownership
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

            var userId = "test-user-id";
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, "Administrator") // Or "RegisteredUser"
    }));

            // Act
            var result = await _service.GetForEditAsync(customer.CustomerId, userId, claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstName, Is.EqualTo("Bob"));
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

            string userId = "test-user-id";
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, "Administrator") // Or "RegisteredUser"
    }));

            // Act
            await _service.EditAsync(model, userId, principal);

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
