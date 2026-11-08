using onine.core.Enums;
using System;

namespace onineproject.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public decimal Deliverycost { get; set; }
    }
}
