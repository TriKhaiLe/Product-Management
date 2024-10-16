using WebApplication1.DataConnector;
using WebApplication1.DTOs;
using AutoMapper;

namespace WebApplication1.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Nếu cần map ngược lại, có thể sử dụng ReverseMap()
            CreateMap<Product, CreateNewProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<Catalog, CreateCatalogDto>().ReverseMap();
            CreateMap<Catalog,  UpdateCatalogDto>().ReverseMap();
        }

    }
}
