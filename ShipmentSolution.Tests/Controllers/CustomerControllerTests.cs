using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using ShipmentSolution.Web.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipmentSolution.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _mockService = null!;
        private CustomerController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockService.Object);
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

            _mockService.Setup(s => s.GetPaginatedAsync(1, 5, null)).ReturnsAsync(mockResult);

            var result = await _controller.Index(null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(mockResult));
        }

        [Test]
        public async Task Index_ReturnsViewWithErrorOnException()
        {
            _mockService.Setup(s => s.GetPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
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

            var result = await _controller.Create(model) as RedirectToActionResult;

            _mockService.Verify(s => s.CreateAsync(model), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Edit_Get_ReturnsViewWithModel()
        {
            var mockModel = new CustomerEditViewModel { CustomerId = 1, FirstName = "Test" };
            _mockService.Setup(s => s.GetForEditAsync(1)).ReturnsAsync(mockModel);

            var result = await _controller.Edit(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.EqualTo(mockModel));
        }

        [Test]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var model = new CustomerEditViewModel { CustomerId = 1, FirstName = "Edit" };
            var result = await _controller.Edit(model) as RedirectToActionResult;

            _mockService.Verify(s => s.EditAsync(model), Times.Once);
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
