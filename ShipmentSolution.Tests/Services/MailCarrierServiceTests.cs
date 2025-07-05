using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Services
{
    public class MailCarrierServiceTests
    {
        private ApplicationDbContext _context;
        private MailCarrierService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"MailCarrierDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _service = new MailCarrierService(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedMailCarriers()
        {
            var uniqueEmail = "unique_test@example.com";

            _context.MailCarriers.AddRange(
                new MailCarrier
                {
                    FirstName = "A",
                    LastName = "One",
                    Email = uniqueEmail,
                    PhoneNumber = "000",
                    Status = "Active",
                    CurrentLocation = "X",
                    IsDeleted = false
                },
                new MailCarrier
                {
                    FirstName = "B",
                    LastName = "Two",
                    Email = "deleted@example.com",
                    PhoneNumber = "111",
                    Status = "Inactive",
                    CurrentLocation = "Y",
                    IsDeleted = true
                }
            );

            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();
            var filtered = result.Where(m => m.Email == uniqueEmail).ToList();

            Assert.That(filtered.Count, Is.EqualTo(1));
            Assert.That(filtered.First().FullName, Is.EqualTo("A One"));
        }


        [Test]
        public async Task GetForEditAsync_ReturnsCorrectModel()
        {
            var mc = new MailCarrier
            {
                FirstName = "Tom",
                LastName = "Smith",
                Email = "tom@example.com",
                PhoneNumber = "123",
                Status = "Active",
                CurrentLocation = "Depot",
                IsDeleted = false
            };
            _context.MailCarriers.Add(mc);
            await _context.SaveChangesAsync();

            var result = await _service.GetForEditAsync(mc.MailCarrierId);

            Assert.That(result.FullName, Is.EqualTo("Tom Smith"));
            Assert.That(result.Email, Is.EqualTo("tom@example.com"));
        }

        [Test]
        public void GetForEditAsync_ThrowsWhenNotFound()
        {
            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetForEditAsync(999));
            Assert.That(ex!.Message, Is.EqualTo("Mail carrier not found."));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectModel()
        {
            var mc = new MailCarrier
            {
                FirstName = "Sara",
                LastName = "Jones",
                Email = "sara@example.com",
                PhoneNumber = "999",
                Status = "Available",
                CurrentLocation = "Station",
                IsDeleted = false
            };
            _context.MailCarriers.Add(mc);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(mc.MailCarrierId);

            Assert.That(result.FullName, Is.EqualTo("Sara Jones"));
            Assert.That(result.PhoneNumber, Is.EqualTo("999"));
        }

        [Test]
        public void GetByIdAsync_ThrowsWhenNotFound()
        {
            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(999));
            Assert.That(ex!.Message, Is.EqualTo("Mail carrier not found."));
        }

        [Test]
        public async Task CreateAsync_AddsMailCarrier()
        {
            var model = new MailCarrierCreateViewModel
            {
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "555-1234",
                Status = "Available",
                CurrentLocation = "Warehouse"
            };

            await _service.CreateAsync(model);

            var carrier = await _context.MailCarriers
                .FirstOrDefaultAsync(c => c.Email == "john@example.com");
            Assert.That(carrier, Is.Not.Null);
            Assert.That(carrier!.FirstName, Is.EqualTo("John"));
            Assert.That(carrier.LastName, Is.EqualTo("Doe"));
            Assert.That(carrier.Email, Is.EqualTo("john@example.com"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
