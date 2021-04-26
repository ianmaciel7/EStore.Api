using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Models
{
    public class ProductModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string CategoryName { get; set; } //auto map
        [Required]
        public string SubCategoryName { get; set; }
    }
}
