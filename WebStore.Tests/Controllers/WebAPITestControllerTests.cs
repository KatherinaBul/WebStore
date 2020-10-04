using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebAPITestControllerTests
    {
        [TestMethod]
        public void Index_Returns_View_with_Values()
        {
            var expectedValues = new[] { "1", "2", "3" };

            var valueServiceMock = new Mock<IValueService>();

            valueServiceMock
                .Setup(service => service.Get())
                .Returns(expectedValues);

            var controller = new WebApiTestController(valueServiceMock.Object);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);

            Assert.Equal(expectedValues.Length, model.Count());

            // Если объект просто притворяется интерфейсом, то это "Стаб" (Stab)

            valueServiceMock.Verify(service => service.Get());
            valueServiceMock.VerifyNoOtherCalls();

            // Если выполняется последующая проверка состояния, то это "Мок" (Moq)
        }
    }
}