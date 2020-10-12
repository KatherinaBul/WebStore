using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService) => _cartService = cartService;

        public IActionResult Details() => View(new CartOrderViewModel { Cart = _cartService.TransformFromCart() });

        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel orderViewModel, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    Cart = _cartService.TransformFromCart(),
                    Order = orderViewModel
                });

            var orderModel = new CreateOrderModel
            {
                Order = orderViewModel,
                Items = _cartService.TransformFromCart().Items
                    .Select(item => new OrderItemDto
                    {
                        Id = item.Product.Id,
                        Price = item.Product.Price,
                        Quantity = item.Quantity
                    })
            };

            var order = await orderService.CreateOrder(User.Identity.Name, orderModel);

            _cartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
        
        #region API

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddToCartApi(int id)
        {
            _cartService.AddToCart(id);
            return Json(new { id, message = $"Товар с id:{id} был добавлен в корзину" });
        }

        public IActionResult DecrementFromCartApi(int id)
        {
            _cartService.DecrementFromCart(id);
            return Ok();
        }

        public IActionResult RemoveFromCartApi(int id)
        {
            _cartService.RemoveFromCart(id);
            return Ok();
        }


        public IActionResult ClearApi()
        {
            _cartService.Clear();
            return Ok();
        }

        #endregion
    }
}
