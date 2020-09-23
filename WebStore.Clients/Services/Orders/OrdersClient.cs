using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Dto.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Services.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration configuration) : base(configuration, "api/orders")
        {
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrders(string userName) =>
            await GetAsync<IEnumerable<OrderDto>>($"{ServiceAddress}/user/{userName}");

        public async Task<OrderDto> GetOrderById(int id) => await GetAsync<OrderDto>($"{ServiceAddress}/{id}");

        public async Task<OrderDto> CreateOrder(string userName, CreateOrderModel orderModel)
        {
            var response = await PostAsync($"{ServiceAddress}/{userName}", orderModel);
            return await response.Content.ReadAsAsync<OrderDto>();
        }
    }
}