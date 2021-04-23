using EStore.API.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace EStore.API.Data
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public SubCategory SubCategory { get; set; }
        
    }
}