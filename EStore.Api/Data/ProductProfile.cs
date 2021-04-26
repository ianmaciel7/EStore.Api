using AutoMapper;
using EStore.API.Models;

namespace EStore.API.Data
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.CategoryName, source => source.MapFrom(source => source.SubCategory.Category.Name))
                .ReverseMap();
        }
    }
}
