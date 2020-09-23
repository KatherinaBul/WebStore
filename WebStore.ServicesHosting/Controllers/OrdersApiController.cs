using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Dto.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersApiController : Controller, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService) => _orderService = orderService;

        [HttpGet("user/{username}")]
        public Task<IEnumerable<OrderDto>> GetUserOrders(string userName) => _orderService.GetUserOrders(userName);

        [HttpGet("{id}")]
        public Task<OrderDto> GetOrderById(int id) => _orderService.GetOrderById(id);

        [HttpPost("{username}")]
        public Task<OrderDto> CreateOrder(string userName, [FromBody] CreateOrderModel orderModel) =>
            _orderService.CreateOrder(userName, orderModel);
    }
}