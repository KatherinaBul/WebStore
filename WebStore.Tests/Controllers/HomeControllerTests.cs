using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Controllers;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }
        
        [TestMethod]
        public void Blogs_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Blogs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.BlogSingle();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Error404_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Error404();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void Throw_thrown_ApplicationException()
        {
            var controller = new HomeController();

            const string id = "Test value";

            _ = controller.Throw(id);
        }

        [TestMethod]
        public void Throw_thrown_ApplicationException_2()
        {
            var controller = new HomeController();

            const string id = "Test value";
            var expectedMessage = $"Исключение: {id}";

            var exception = Assert.Throws<ApplicationException>(() => controller.Throw(id));

            var actualMessage = exception.Message;
            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}