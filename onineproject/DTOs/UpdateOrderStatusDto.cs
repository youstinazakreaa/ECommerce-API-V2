using onine.core.Enums;
using System.ComponentModel.DataAnnotations;

namespace onineproject.DTOs
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus? newstatus { get; set; }
    }
}
