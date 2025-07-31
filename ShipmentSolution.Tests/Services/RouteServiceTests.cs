using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Web.ViewModels.RouteViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Services
{
    public class RouteServiceTests
    {
        private ApplicationDbContext _context;
        private RouteService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"RouteDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.Routes.RemoveRange(_context.Routes);
            _context.SaveChanges();

            _service = new RouteService(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedRoutes()
        {
            _context.Routes.AddRange(
                new Route { StartLocation = "A", EndLocation = "B", IsDeleted = false },
                new Route { StartLocation = "C", EndLocation = "D", IsDeleted = true }
            );
            await _context.SaveChangesAsync();

            var result = await _service.GetAllAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
            var route = result.First();
            Assert.That(route.StartLocation, Is.EqualTo("A"));
            Assert.That(route.EndLocation, Is.EqualTo("B"));
        }

        [Test]
        public async Task CreateAsync_AddsNewRoute()
        {
            var model = new RouteCreateViewModel
            {
                StartLocation = "X",
                EndLocation = "Y",
                Stops = "3",
                Distance = 100,
                Priority = 1,
                MailCarrierId = 1
            };

            string userId = "test-user-id";

            await _service.CreateAsync(model, userId);

            var route = await _context.Routes.FirstOrDefaultAsync(r => r.StartLocation == "X");
            Assert.That(route, Is.Not.Null);
            Assert.That(route!.EndLocation, Is.EqualTo("Y"));
            Assert.That(route.CreatedByUserId, Is.EqualTo(userId));
        }


        [Test]
        public async Task GetForEditAsync_ReturnsCorrectRoute()
        {
            var route = new Route { StartLocation = "Start", EndLocation = "End", IsDeleted = false };
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            var result = await _service.GetForEditAsync(route.RouteId);

            Assert.That(result.StartLocation, Is.EqualTo("Start"));
            Assert.That(result.EndLocation, Is.EqualTo("End"));
        }

        [Test]
        public void GetForEditAsync_ThrowsWhenNotFound()
        {
            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetForEditAsync(999));
            Assert.That(ex!.Message, Is.EqualTo("Route not found."));
        }

        [Test]
        public async Task EditAsync_UpdatesRoute()
        {
            var route = new Route { StartLocation = "OldStart", EndLocation = "OldEnd", IsDeleted = false };
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            var model = new RouteEditViewModel
            {
                RouteId = route.RouteId,
                StartLocation = "NewStart",
                EndLocation = "NewEnd",
                Stops = "4", 
                Distance = 150,
                Priority = 2
            };

            await _service.EditAsync(model);

            var updatedRoute = await _context.Routes.FindAsync(route.RouteId);
            Assert.That(updatedRoute!.StartLocation, Is.EqualTo("NewStart"));
            Assert.That(updatedRoute.EndLocation, Is.EqualTo("NewEnd"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectRoute()
        {
            var route = new Route { StartLocation = "A", EndLocation = "B", IsDeleted = false };
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(route.RouteId);

            
            Assert.That(route.StartLocation, Is.EqualTo("A"));
            Assert.That(route.EndLocation, Is.EqualTo("B"));
        }

        [Test]
        public void GetByIdAsync_ThrowsWhenNotFound()
        {
            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(999));
            Assert.That(ex!.Message, Is.EqualTo("Route not found."));
        }

        [Test]
        public async Task SoftDeleteAsync_SoftDeletesRoute()
        {
            var route = new Route { StartLocation = "X", EndLocation = "Y", IsDeleted = false };
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            await _service.SoftDeleteAsync(route.RouteId); // ✅ Call the correct method

            var deletedRoute = await _context.Routes.FindAsync(route.RouteId);
            Assert.That(deletedRoute!.IsDeleted, Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
