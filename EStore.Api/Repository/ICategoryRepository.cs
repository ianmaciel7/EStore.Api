using EStore.Api.InputModel;
using EStore.API.Data;
using EStore.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Repository
{
    public interface ICategoryRepository : IDisposable
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity);
        Task<Category> GetCategoryAsync(string categoryName);
        Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName);
        Task<Product> GetProductAsync(string productName);
        Task<Product> GetProductAsync(string categoryName, string subCategoryName, int productId);
        Task<Product> AddProductAsync(string subCategoryName, Product product);
        
    }
}
