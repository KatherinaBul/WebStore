using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.Dto.Order
{
    public class CreateOrderModel
    {
        public OrderViewModel Order { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; }
    }
}