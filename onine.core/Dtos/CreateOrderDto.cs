using onine.core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace onine.core.DTOs
{

    public class CreateOrderDto
    {
        [Required]
        public string BasketId { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int DeliveryMethodId { get; set; }

        [Required]
        public AddressDto Address { get; set; } = new();
    }
}