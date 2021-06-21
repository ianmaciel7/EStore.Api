using EStore.API.Data.Entities;
using EStore.API.InputModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.ViewModel
{
    public class CategoryViewModel
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
