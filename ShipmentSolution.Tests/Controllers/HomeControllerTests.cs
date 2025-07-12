using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Web.Controllers;
using ShipmentSolution.Web.ViewModels;

namespace ShipmentSolution.Tests.Controllers
{
    public class HomeControllerTests
    {
        #pragma warning disable NUnit1032
        private Mock<ILogger<HomeController>> _loggerMock;
        //#pragma warning restore NUnit1032

        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_loggerMock.Object);
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            var result = _controller.Index();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            var result = _controller.Privacy();
            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public void Error_ReturnsViewResultWithModel()
        {
            var result = _controller.Error() as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Model, Is.InstanceOf<ErrorViewModel>());
        }

        [Test]
        public void Error404_ReturnsViewResult()
        {
            var result = _controller.Error404();
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).ViewName, Is.EqualTo("Error404"));
        }

        [Test]
        public void Error500_ReturnsViewResult()
        {
            var result = _controller.Error500();
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(((ViewResult)result).ViewName, Is.EqualTo("Error500"));
        }
    }
}
