using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _mockService = null!;
        private CustomerController _controller = null!;
        private Mock<UserManager<IdentityUser>> _userManagerMock = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICustomerService>();

            var store = new Mock<IUserStore<IdentityUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();
            var userValidators = new List<IUserValidator<IdentityUser>>();
            var passwordValidators = new List<IPasswordValidator<IdentityUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<IdentityUser>>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors.Object,
                services.Object,
                logger.Object
            );

            _controller = new CustomerController(_mockService.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithModel()
        {
            var mockResult = new PaginatedList<CustomerViewModel>
            {
                Items = new List<CustomerViewModel> { new CustomerViewModel { FirstName = "John", LastName = "Doe" } },
                PageIndex = 1,
                TotalPages = 1
            };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-123");

            _mockService.Setup(s => s.GetPaginatedAsync(1, 5, null, "user-123", false, true))
                        .ReturnsAsync(mockResult);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user-123"),
                        new Claim(ClaimTypes.Role, "RegisteredUser")
                    }, "mock"))
                }
            };

            var result = await _controller.Index(null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(mockResult));
        }

        [Test]
        public async Task Index_ReturnsViewWithErrorOnException()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "user-123"),
                        new Claim(ClaimTypes.Role, "RegisteredUser")
                    }, "mock"))
                }
            };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns("user-123");

            _mockService.Setup(s => s.GetPaginatedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Test Exception"));

            var result = await _controller.Index(null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.False(result!.ViewData.ModelState.IsValid);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            var result = _controller.Create() as ViewResult;
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var model = new CustomerCreateViewModel();
            var result = await _controller.Create(model) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var model = new CustomerCreateViewModel
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john@example.com",
                City = "New York",
                PhoneNumber = "123-456-7890",
                PreferredShippingMethod = "Air",
                State = "NY",
                ZipCode = "10001",
                ShippingCostThreshold = 50
            };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns("user-123");

            _mockService.Setup(s => s.CreateAsync(model, "user-123"))
                        .Returns(Task.CompletedTask);

            var result = await _controller.Create(model) as RedirectToActionResult;

            _mockService.Verify(s => s.CreateAsync(model, "user-123"), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var mockModel = new CustomerEditViewModel { CustomerId = 1, FirstName = "Test" };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user-123");

            _mockService.Setup(s => s.GetForEditAsync(1, "user-123", It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(mockModel);

            var result = await _controller.Edit(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(mockModel));
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var model = new CustomerEditViewModel { CustomerId = 1, FirstName = "Edit" };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                            .Returns("user-123");

            _mockService.Setup(s => s.EditAsync(model, "user-123", It.IsAny<ClaimsPrincipal>()))
                        .ReturnsAsync(true);

            var result = await _controller.Edit(model) as RedirectToActionResult;

            _mockService.Verify(s => s.EditAsync(model, "user-123", It.IsAny<ClaimsPrincipal>()), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithModel()
        {
            var customer = new CustomerViewModel
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com"
            };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(customer);

            var result = await _controller.Delete(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.TypeOf<CustomerDeleteViewModel>());
        }

        [Test]
        public async Task DeleteConfirmed_Post_DeletesCustomerAndRedirects()
        {
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            _mockService.Verify(s => s.SoftDeleteAsync(1), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }
    }
}
