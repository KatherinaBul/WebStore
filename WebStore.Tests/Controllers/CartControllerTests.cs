using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public async Task CheckOut_ModelState_Invalid_Returns_ViewModel()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartServiceMock.Object);
            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedModelName = "Test order";

            var orderViewModel = new OrderViewModel { Name = expectedModelName };
            var result = await controller.CheckOut(orderViewModel, orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(expectedModelName, model.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_Calls_Service_and_Return_Redirect()
        {
            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
               .Setup(c => c.TransformFromCart())
               .Returns(() => new CartViewModel
                {
                    Items = new[] { (new ProductViewModel { Name = "Product" }, 1) }
                });

            const int expectedOrderId = 1;
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
               .Setup(c => c.CreateOrder(It.IsAny<string>(), It.IsAny<CreateOrderModel>()))
               .ReturnsAsync(new OrderDto
                {
                    Id = expectedOrderId
                });

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User =  new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "TestUser") }))
                    }
                }
            };

            var orderModel = new OrderViewModel
            {
                Name = "Test order",
                Address = "Test address",
                Phone = "+1(234)567-89-00"
            };

            var result = await controller.CheckOut(orderModel, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);

            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}