using EStore.API.Data;
using EStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Services
{
    public interface IProductService
    {
        Task<ProductModel[]> AllProductsAsync();
        Task<Product> GetProductEntityByNameAsync(string name);
        Task<ProductModel> GetProductByNameAsync(string name);
        Task<ProductModel> AddProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(string name, ProductModel model);
        Task<bool> DeleteProduct(Product product);
        Task<bool> IsThereThisProduct(string name);
    }
}
