using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.ViewModels.Categories;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;

namespace AspNetCoreMvcPractice.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductViewModel, Product>().ReverseMap();
            CreateMap<EditProductViewModel, Product>().ReverseMap();

            CreateMap<EditImageViewModel, Category>().ReverseMap();
        }
    }
}
