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
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    [TestFixture]
    public class DeliveryControllerTests
    {
        private Mock<IDeliveryService> _mockService = null!;
        private DeliveryController _controller = null!;
        private Mock<UserManager<IdentityUser>> _mockUserManager = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IDeliveryService>();

            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new Mock<IdentityErrorDescriber>();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<IdentityUser>>>();

            _mockUserManager = new Mock<UserManager<IdentityUser>>(
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

            _controller = new DeliveryController(_mockService.Object, _mockUserManager.Object);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
        new Claim(ClaimTypes.Role, "Administrator")
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };
        }


        [Test]
        public async Task Index_ReturnsViewWithModel()
        {
            string testUserId = "test-user-id";
            bool isAdmin = false;
            bool isLoggedIn = true;

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns(testUserId);

            _mockService.Setup(s => s.GetPaginatedAsync(1, 5, null, null, testUserId, isAdmin, isLoggedIn))
                        .ReturnsAsync(new PaginatedList<DeliveryViewModel> { Items = new List<DeliveryViewModel>() });

            _mockService.Setup(s => s.GetCarrierNamesAsync())
                        .ReturnsAsync(new List<string>());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "RegisteredUser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            var result = await _controller.Index(null, null, 1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.TypeOf<PaginatedList<DeliveryViewModel>>());
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel()
        {
            _mockService.Setup(s => s.GetCreateModelAsync(It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(new DeliveryCreateViewModel());

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns("test-user-id"); // 🔧 Добавено

            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "test-user-id")
    }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            var result = await _controller.Create() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.TypeOf<DeliveryCreateViewModel>());
        }


        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var testUserId = "test-user-id";
            var model = new DeliveryCreateViewModel();

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns(testUserId);

            _mockService.Setup(s => s.CreateAsync(model, testUserId))
                        .Returns(Task.CompletedTask);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "RegisteredUser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            var result = await _controller.Create(model) as RedirectToActionResult;

            _mockService.Verify(s => s.CreateAsync(model, testUserId), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

       

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var testUserId = "test-user-id";
            var viewModel = new DeliveryEditViewModel();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "RegisteredUser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            _mockUserManager.Setup(um => um.GetUserId(userPrincipal)).Returns(testUserId);

            _mockService.Setup(s => s.GetForEditAsync(1, testUserId, userPrincipal))
                        .ReturnsAsync(viewModel);

            var result = await _controller.Edit(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var testUserId = "test-user-id";
            var model = new DeliveryEditViewModel();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "RegisteredUser")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            _mockUserManager.Setup(um => um.GetUserId(userPrincipal)).Returns(testUserId);

            _mockService.Setup(s => s.EditAsync(model, testUserId, userPrincipal))
                        .ReturnsAsync(true);

            var result = await _controller.Edit(model) as RedirectToActionResult;

            _mockService.Verify(s => s.EditAsync(model, testUserId, userPrincipal), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithModel()
        {
            var testUserId = "test-user-id";
            var viewModel = new DeliveryDeleteViewModel();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            _mockUserManager.Setup(um => um.GetUserId(userPrincipal)).Returns(testUserId);

            _mockService.Setup(s => s.GetDeleteViewModelAsync(1, testUserId, userPrincipal))
                        .ReturnsAsync(viewModel);

            var result = await _controller.Delete(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public async Task DeleteConfirmed_DeletesAndRedirects()
        {
            var testUserId = "test-user-id";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, testUserId),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            _mockUserManager.Setup(um => um.GetUserId(userPrincipal)).Returns(testUserId);

            _mockService.Setup(s => s.SoftDeleteAsync(1, testUserId, userPrincipal))
                        .ReturnsAsync(true);

            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            _mockService.Verify(s => s.SoftDeleteAsync(1, testUserId, userPrincipal), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            if (_controller is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
