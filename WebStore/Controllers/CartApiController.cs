using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartApiController(ICartService cartService) => _cartService = cartService;

        [HttpGet("view")]
        public IActionResult CartView() => new ViewComponentResult {ViewComponentName = "Cart"};

        [HttpGet("add/{id}")]
        [HttpGet("increment/{id}")]
        public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return new JsonResult(new {id, message = $"Товар с id:{id} был добавлен в корзину"});
        }

        [HttpGet("decrement/{id}")]
        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return Ok();
        }

        [HttpGet("remove/{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return Ok();
        }


        [HttpGet("clear")]
        public IActionResult Clear()
        {
            _cartService.Clear();
            return Ok();
        }
    }
}