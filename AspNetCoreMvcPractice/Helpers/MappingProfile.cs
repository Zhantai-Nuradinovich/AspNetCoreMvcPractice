using AspNetCoreMvcPractice.Data.Models;
using AspNetCoreMvcPractice.ViewModels.Products;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductViewModel, Product>().ReverseMap();
            CreateMap<EditProductViewModel, Product>().ReverseMap();
        }
    }
}
