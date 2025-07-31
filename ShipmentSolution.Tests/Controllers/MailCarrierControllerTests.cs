using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class MailCarrierControllerTests
    {
        private Mock<IMailCarrierService> _mailCarrierServiceMock;
#pragma warning disable NUnit1032
        private Mock<ILogger<MailCarrierController>> _loggerMock;
//#pragma warning restore NUnit1032
        private MailCarrierController _controller;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        [SetUp]
        public void SetUp()
        {
            _mailCarrierServiceMock = new Mock<IMailCarrierService>();
            _loggerMock = new Mock<ILogger<MailCarrierController>>();

            _controller = new MailCarrierController(_mailCarrierServiceMock.Object, _loggerMock.Object);

            // Simulate logged-in RegisteredUser
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "user-123"),
        new Claim(ClaimTypes.Role, "RegisteredUser")
    }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }


        [Test]
        public async Task Index_ReturnsViewResult_WithMailCarriers()
        {
            // Arrange
            var mailCarriers = new List<MailCarrierViewModel>
            {
                new MailCarrierViewModel { MailCarrierId = 1, FullName = "John", Status = "Active" },
                new MailCarrierViewModel { MailCarrierId = 2, FullName = "Jane", Status = "Inactive" }
            };

            var paginatedResult = new PaginatedList<MailCarrierViewModel>
            {
                Items = mailCarriers,
                PageIndex = 1,
                TotalPages = 1
            };

            string userId = "user-123";
            bool isAdmin = false;

            _mailCarrierServiceMock.Setup(s =>
                s.GetPaginatedAsync(1, 5, null, null, userId, isAdmin))
                .ReturnsAsync(paginatedResult);

            _mailCarrierServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(mailCarriers);

            // Mock User Identity (if controller depends on UserManager)
            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _userManagerMock.Setup(um => um.IsInRoleAsync(It.IsAny<IdentityUser>(), "Administrator")).ReturnsAsync(isAdmin);

            // Act
            var result = await _controller.Index(null, null);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.Model, Is.InstanceOf<PaginatedList<MailCarrierViewModel>>());
        }


        [Test]
        public void Create_Get_ReturnsView()
        {
            var result = _controller.Create();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("FullName", "Required");

            var result = await _controller.Create(new MailCarrierCreateViewModel());

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var model = new MailCarrierCreateViewModel { FullName = "John Doe" };
            var testUserId = "fake-user-id";

            // Update mock to expect two parameters
            _mailCarrierServiceMock
                .Setup(s => s.CreateAsync(model, testUserId))
                .Returns(Task.CompletedTask);

            // Simulate the logged-in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, testUserId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await _controller.Create(model);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }


        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var model = new MailCarrierEditViewModel { MailCarrierId = 1, FullName = "Test" };
            _mailCarrierServiceMock.Setup(s => s.GetForEditAsync(1)).ReturnsAsync(model);

            var result = await _controller.Edit(1);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("FullName", "Required");

            var result = await _controller.Edit(new MailCarrierEditViewModel());

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var model = new MailCarrierEditViewModel { MailCarrierId = 1, FullName = "Updated" };

            _mailCarrierServiceMock.Setup(s => s.EditAsync(model)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(model);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }

        

        [Test]
        public async Task DeleteConfirmed_DeletesAndRedirects()
        {
            _mailCarrierServiceMock.Setup(s => s.SoftDeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }

        [TearDown]
        public void TearDown()
        {
            _mailCarrierServiceMock = null!;
            _loggerMock = null!;
            _controller = null!;
        }
    }
}
