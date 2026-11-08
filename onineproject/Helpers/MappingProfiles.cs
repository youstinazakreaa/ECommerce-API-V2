using AutoMapper;
using onine.core.DTOs;
using onine.core.Entites;
using onine.core.Entites.Identity;
using onineproject.DTOs;

namespace onineproject.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() { 
            CreateMap<Product,ProductToReturnDTOs>()
                .ForMember(d=> d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d=> d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductpictureurlResolver>());
            
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>().ForMember
                (d => d.Id, o => o.Ignore());
            CreateMap<AddressDto, Address>().ReverseMap();
            




        }

        
    }
}
