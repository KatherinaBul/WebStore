using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Dto.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route(WebApi.Orders)]
    [ApiController]
    public class OrdersApiController : Controller, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersApiController(IOrderService orderService) => _orderService = orderService;

        /// <summary>
        /// Получить список заказов пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список заказов</returns>
        [HttpGet("user/{username}")]
        public Task<IEnumerable<OrderDto>> GetUserOrders(string userName) => _orderService.GetUserOrders(userName);

        /// <summary>
        /// Получить информацию о заказе по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заказа</param>
        /// <returns>Информация о заказе</returns>
        [HttpGet("{id}")]
        public Task<OrderDto> GetOrderById(int id) => _orderService.GetOrderById(id);

        /// <summary>
        /// Создать новый заказ
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="orderModel">Информация о новом заказе</param>
        /// <returns>Информация о созданном заказе</returns>
        [HttpPost("{username}")]
        public Task<OrderDto> CreateOrder(string userName, [FromBody] CreateOrderModel orderModel) =>
            _orderService.CreateOrder(userName, orderModel);
    }
}