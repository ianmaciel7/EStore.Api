﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Models
{
    public class SubCategoryModel
    {
        public int SubCategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
