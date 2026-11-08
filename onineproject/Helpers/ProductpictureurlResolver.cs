
using AutoMapper;
using onine.core.Entites;
using onineproject.DTOs;

namespace onineproject.Helpers
{
    public class ProductpictureurlResolver
        : IValueResolver<Product, ProductToReturnDTOs, string?>
    {
        private readonly IConfiguration _configuration;

        public ProductpictureurlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(
            Product source,
            ProductToReturnDTOs destination,
            string? destMember,
            ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(source.PictureUrl))
            {
                return string.Empty;
            }

            var baseUrl = _configuration["ApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException(
                    "ApiBaseUrl is missing from configuration.");
            }

            return $"{baseUrl.TrimEnd('/')}/{source.PictureUrl.TrimStart('/')}";
        }
    }
}
