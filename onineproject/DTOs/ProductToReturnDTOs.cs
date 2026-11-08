
namespace onineproject.DTOs
{
    public class ProductToReturnDTOs
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? PictureUrl { get; set; }

        public decimal Price { get; set; }

        public int ProductTypeId { get; set; }

        public string ProductType { get; set; } = string.Empty;

        public int ProductBrandId { get; set; }

        public string ProductBrand { get; set; } = string.Empty;
    }
}
