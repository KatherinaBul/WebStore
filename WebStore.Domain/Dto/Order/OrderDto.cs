using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Dto.Order
{
    public class OrderDto : INamedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<OrderItemDto> Items { get; set; }
    }
}