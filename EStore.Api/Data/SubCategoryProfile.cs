using AutoMapper;
using EStore.API.Data.Entities;
using EStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public class SubCategoryProfile : Profile
    {
        public SubCategoryProfile()
        {
            this.CreateMap<SubCategory, SubCategoryModel>();               
        }
    }
}
