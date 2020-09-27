using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Services.Mapping
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order) => order is null ? null : new OrderDto
        {
            Id = order.Id,
            Phone = order.Phone,
            Address = order.Address,
            Date = order.Date,
            Items = order.Items.Select(ToDto)
        };

        public static IEnumerable<OrderDto> ToDto(this IEnumerable<Order> orders) => orders.Select(ToDto);
        
        public static OrderItemDto ToDto(this OrderItem item) => item is null ? null : new OrderItemDto
        {
            Id = item.Id,
            Price = item.Price,
            Quantity = item.Quantity
        };
        
        public static IEnumerable<OrderItemDto> ToDto(this IEnumerable<OrderItem> items) => items.Select(ToDto);

        public static Order FromDto(this OrderDto order) => order is null ? null : new Order
        {
            Id = order.Id,
            Phone = order.Phone,
            Address = order.Address,
            Date = order.Date,
            Items = order.Items.Select(FromDto).ToArray()
        };
        
        public static IEnumerable<Order> FromDto(this IEnumerable<OrderDto> orders) => orders.Select(FromDto);

        public static OrderItem FromDto(this OrderItemDto item) => item is null ? null : new OrderItem
        {
            Id = item.Id,
            Price = item.Price,
            Quantity = item.Quantity
        };
        
        public static IEnumerable<OrderItem> FromDto(this IEnumerable<OrderItemDto> items) => items.Select(FromDto);
    }
}