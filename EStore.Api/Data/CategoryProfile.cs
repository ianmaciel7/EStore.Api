using AutoMapper;
using EStore.API.Data.Entities;
using EStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            this.CreateMap<Category, CategoryModel>()
                .ReverseMap();

            this.CreateMap<SubCategory, SubCategoryModel>()
                .ReverseMap()
                .ForMember(s => s.Category, opt => opt.Ignore());

        }
    }
}
