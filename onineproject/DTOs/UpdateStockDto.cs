using System.ComponentModel.DataAnnotations;

namespace onineproject.DTOs
{
    public class UpdateStockDto
    {
        [Required(ErrorMessage ="Stock Quantity is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity must be a non-negative integer.")]
        public int? StockQuantity { get; set; }
    }
}
