using EStore.Api.ViewModel;
using EStore.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.ViewModel
{
    public class SubCategoryViewModel
    {
        [Required]
        public int SubCategoryId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
