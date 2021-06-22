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
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync(string categoryName);
        Task<Category> AddCategoryAsync(Category category);
        Task<SubCategory> GetSubCategoryAsync(string categoryName, int subCategoryId);
        Task<SubCategory> AddSubCategoryAsync(string categoryName,SubCategory subCategory);
        Task<IEnumerable<Product>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity);
        Task<Category> GetCategoryAsync(string categoryName);
        Task<Category> GetCategoryAsync(int categoryId);
        Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName);        
        Task<Product> GetProductAsync(string productName);
        Task<Product> GetProductAsync(string categoryName, string subCategoryName, int productId);
        Task<Product> AddProductAsync(string subCategoryName, Product product);      
        void DeleteProduct(Product product);
        void UpdateProduct(Product newProduct);
        Task SaveChangesAsync();
        void UpdateCategory(Category newCategory);
        void DeleteCategory(Category category);
        Task<SubCategory> GetSubCategoryAsync(string subCategoryName);
        void UpdateSubCategory(SubCategory newSubCategory);
        void DeleteSubCategory(SubCategory subCategory);
    }
}
