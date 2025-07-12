
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    [TestFixture]
    public class DeliveryControllerTests
    {
        private Mock<IDeliveryService> _mockService;
        private DeliveryController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IDeliveryService>();
            _controller = new DeliveryController(_mockService.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithModel()
        {
            _mockService.Setup(s => s.GetPaginatedAsync(1, 5, null, null))
                        .ReturnsAsync(new PaginatedList<DeliveryViewModel> { Items = new List<DeliveryViewModel>() });
            _mockService.Setup(s => s.GetCarrierNamesAsync())
                        .ReturnsAsync(new List<string>());

            var result = await _controller.Index(null, null, 1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.TypeOf<PaginatedList<DeliveryViewModel>>());
        }

        [Test]
        public async Task Create_Get_ReturnsViewWithModel()
        {
            _mockService.Setup(s => s.GetCreateModelAsync()).ReturnsAsync(new DeliveryCreateViewModel());

            var result = await _controller.Create() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.TypeOf<DeliveryCreateViewModel>());
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var model = new DeliveryCreateViewModel();
            var result = await _controller.Create(model) as RedirectToActionResult;

            _mockService.Verify(s => s.CreateAsync(model), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            var model = new DeliveryCreateViewModel();
            _controller.ModelState.AddModelError("Error", "Invalid");

            var result = await _controller.Create(model) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(model));
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var viewModel = new DeliveryEditViewModel();
            _mockService.Setup(s => s.GetForEditAsync(1)).ReturnsAsync(viewModel);

            var result = await _controller.Edit(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var model = new DeliveryEditViewModel();
            var result = await _controller.Edit(model) as RedirectToActionResult;

            _mockService.Verify(s => s.EditAsync(model), Times.Once);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Delete_Get_ReturnsViewWithModel()
        {
            var viewModel = new DeliveryDeleteViewModel();
            _mockService.Setup(s => s.GetDeleteViewModelAsync(1)).ReturnsAsync(viewModel);

            var result = await _controller.Delete(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(viewModel));
        }

        [Test]
        public async Task DeleteConfirmed_DeletesAndRedirects()
        {
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            _mockService.Verify(s => s.SoftDeleteAsync(1), Times.Once);
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
