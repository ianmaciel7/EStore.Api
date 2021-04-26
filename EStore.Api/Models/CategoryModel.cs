﻿using EStore.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Models
{
    public class CategoryModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<SubCategoryModel> SubCategories { get; set; }
    }
}
