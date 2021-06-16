using EStore.Api.ViewModel;
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
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<ProductViewModel> Products { get; set; }
    }
}
