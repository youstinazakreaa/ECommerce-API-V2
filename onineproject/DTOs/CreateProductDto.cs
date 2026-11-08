using System.ComponentModel.DataAnnotations;

namespace onineproject.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Range(0.01 ,double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue)]
        public int ProductTypeId { get; set; }
        [Range(1, int.MaxValue)]
        public int ProductBrandId { get; set; }
        [Required]
        public string PictureUrl { get; set; }
    }
}
