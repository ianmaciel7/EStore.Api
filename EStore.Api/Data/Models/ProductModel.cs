using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public double Price { get; set; }        
        public string CategoryName { get; set; } //auto map
        public string SubCategoryName { get; set; }
    }
}
