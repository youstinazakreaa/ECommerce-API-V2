

using onine.core.Enums;

namespace onine.core.Entites
{
    public class Order : BaseEntity
    {
        public string BuyerEmail { get; set; } = string.Empty;

        public DateTimeOffset OrderDate { get; set; }
            = DateTimeOffset.UtcNow;

        public OrderStatus orderStatus { get; set; }
            = OrderStatus.Pending;

        public decimal Subtotal { get; set; }

        public ICollection<OrderItem> orderItems { get; set; }
            = new List<OrderItem>();

        
        public OrderAddress ShipToAddress { get; set; } = new();

        public int DeliveryMethodId { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } = null!;

        public decimal DeliveryCost { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryCost;
        }
    }
}
