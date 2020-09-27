using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetUserOrders(string userName);
        Task<OrderDto> GetOrderById(int id);
        Task<OrderDto> CreateOrder(string userName, CreateOrderModel orderModel);
    }
}