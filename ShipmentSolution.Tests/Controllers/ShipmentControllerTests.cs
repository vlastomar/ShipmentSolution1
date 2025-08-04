
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class ShipmentControllerTests
    {
        private Mock<IShipmentService> _shipmentServiceMock;
        private Mock<ILogger<ShipmentController>> _loggerMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;
#pragma warning disable NUnit1032
        private ShipmentController _controller;
        [SetUp]
        public void SetUp()
        {
            _shipmentServiceMock = new Mock<IShipmentService>();
            _loggerMock = new Mock<ILogger<ShipmentController>>();

            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var userLogger = new Mock<ILogger<UserManager<IdentityUser>>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errorDescriber.Object,
                serviceProvider.Object,
                userLogger.Object
            );

           
            _userManagerMock
                .Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("user-123");

            _controller = new ShipmentController(
                _shipmentServiceMock.Object,
                _loggerMock.Object,
                _userManagerMock.Object);
        }



        [Test]
        public async Task Index_ReturnsViewWithModel()
        {
            // Arrange
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "user-123")
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-123");

            _shipmentServiceMock.Setup(s => s.GetPaginatedAsync(
                1,
                5,
                null,
                null,
                "user-123",
                false // not admin
            )).ReturnsAsync(new PaginatedList<ShipmentViewModel>());

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }



        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            // Arrange
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "user-123")
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            var model = new ShipmentEditViewModel();

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns("user-123");

            _shipmentServiceMock
                .Setup(s => s.GetForEditAsync(1, "user-123", It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(model);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(model));
        }


        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "user-123")
    };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            var model = new ShipmentEditViewModel();

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-123");

            _shipmentServiceMock
                .Setup(s => s.EditAsync(model, "user-123", It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(model);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }



        [Test]
        public async Task Delete_Get_ReturnsViewWithModel()
        {
            var model = new ShipmentDeleteViewModel();

            _shipmentServiceMock.Setup(s => s.GetForDeleteAsync(1)).ReturnsAsync(model);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(model));
        }

        [Test]
        public async Task DeleteConfirmed_DeletesAndRedirects()
        {
            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-123");

            _shipmentServiceMock
                .Setup(s => s.DeleteAsync(1, "user-123", It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(true);

            var result = await _controller.DeleteConfirmed(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }
    }
}
