using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data.Entities
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }     
        public Category Category { get; set; }
    }
}
