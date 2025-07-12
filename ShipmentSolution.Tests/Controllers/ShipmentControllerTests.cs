
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class ShipmentControllerTests
    {
        private Mock<IShipmentService> _shipmentServiceMock;
        private Mock<ILogger<ShipmentController>> _loggerMock;
#pragma warning disable NUnit1032
        private ShipmentController _controller;

        [SetUp]
        public void SetUp()
        {
            _shipmentServiceMock = new Mock<IShipmentService>();
            _loggerMock = new Mock<ILogger<ShipmentController>>();
            _controller = new ShipmentController(_shipmentServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithModel()
        {
            _shipmentServiceMock.Setup(s => s.GetPaginatedAsync(1, 5, null, null))
                .ReturnsAsync(new Web.ViewModels.Common.PaginatedList<ShipmentViewModel>());

            var result = await _controller.Index(null, null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel()
        {
            _shipmentServiceMock.Setup(s => s.PrepareCreateViewModelAsync())
                .ReturnsAsync(new ShipmentCreateViewModel());

            var result = await _controller.Create();

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("Test", "Invalid");

            var model = new ShipmentCreateViewModel();

            _shipmentServiceMock.Setup(s => s.PrepareCreateViewModelAsync())
                .ReturnsAsync(model);

            var result = await _controller.Create(model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var model = new ShipmentCreateViewModel();

            _shipmentServiceMock.Setup(s => s.CreateAsync(model)).Returns(Task.CompletedTask);

            var result = await _controller.Create(model);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var model = new ShipmentEditViewModel();

            _shipmentServiceMock.Setup(s => s.GetForEditAsync(1)).ReturnsAsync(model);

            var result = await _controller.Edit(1);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("Test", "Invalid");

            var model = new ShipmentEditViewModel();

            _shipmentServiceMock.Setup(s => s.GetCustomerListAsync()).ReturnsAsync(new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>());
            model.Customers = await _shipmentServiceMock.Object.GetCustomerListAsync();

            var result = await _controller.Edit(model);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var model = new ShipmentEditViewModel();

            _shipmentServiceMock.Setup(s => s.EditAsync(model)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(model);

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
            _shipmentServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        }
    }
}
