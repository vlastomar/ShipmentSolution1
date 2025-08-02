using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.RouteViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class RouteControllerTests
    {
        private Mock<IRouteService> _routeServiceMock;
        private Mock<ILogger<RouteController>> _loggerMock;
#pragma warning disable NUnit1032
        private RouteController _controller;
        private Mock<UserManager<IdentityUser>> _userManagerMock = null!;

        [SetUp]
        public void SetUp()
        {
            _routeServiceMock = new Mock<IRouteService>();
            _loggerMock = new Mock<ILogger<RouteController>>();

            // ✅ Fully mocked dependencies for UserManager to avoid CS8625
            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<IdentityUser>>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errorDescriber.Object,
                serviceProvider.Object,
                logger.Object
            );

            _controller = new RouteController(
                _routeServiceMock.Object,
                _loggerMock.Object,
                _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithModel()
        {
            var pagedRoutes = new PaginatedList<RouteViewModel>
            {
                Items = new List<RouteViewModel>
                {
                    new RouteViewModel { RouteId = 1, StartLocation = "A", EndLocation = "B", Priority = 3 }
                },
                PageIndex = 1,
                TotalPages = 1
            };

            _routeServiceMock.Setup(s =>
                s.GetPaginatedAsync(1, 5, null, null, null, false)) // Replace nulls with actual values if needed
                .ReturnsAsync(pagedRoutes);

            var result = await _controller.Index(null, null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.Model, Is.InstanceOf<PaginatedList<RouteViewModel>>());
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithModel()
        {
            var deleteModel = new RouteDeleteViewModel { RouteId = 1, StartLocation = "A", EndLocation = "B" };
            _routeServiceMock.Setup(s => s.GetDeleteViewModelAsync(1)).ReturnsAsync(deleteModel);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(deleteModel));
        }

        [Test]
        public async Task DeleteConfirmed_DeletesAndRedirects()
        {
            _routeServiceMock.Setup(s => s.SoftDeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            _routeServiceMock = null!;
            _loggerMock = null!;
            _controller = null!;
        }
    }
}
