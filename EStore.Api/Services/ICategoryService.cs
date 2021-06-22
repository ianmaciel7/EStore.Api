using EStore.Api.InputModel;
using EStore.Api.ViewModel;
using EStore.API.InputModel;
using EStore.API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Services
{
    public interface ICategoryService : IDisposable
    {
        Task<CategoryViewModel> UpdateCategoryAsync(int categoryId, CategoryInputModel model);
        Task<CategoryViewModel> GetCategoryAsync(int CategoryId);
        Task<CategoryViewModel> AddCategoryAsync(CategoryInputModel model);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<IEnumerable<SubCategoryViewModel>> GetAllSubCategoriesAsync(string categoryName);       
        Task<SubCategoryViewModel> GetSubCategoryAsync(string categoryName, int subCategoryId);
        Task<SubCategoryViewModel> AddSubCategoryAsync(string categoryName, CategoryInputModel model);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync(string categoryName, string subCategoryName, int page,int quantity);
        Task<ProductViewModel> AddProductAsync(string categoryName, string subCategoryName, ProductInputModel model);
        Task<ProductViewModel> GetProductAsync(string categoryName, string subCategoryName, int productId);       
        Task<ProductViewModel> UpdateProductAsync(string categoryName, string subCategoryName, int productId, ProductInputModel model);
        Task DeleteProductAsync(string categoryName, string subCategoryName, int productId);
        Task DeleteCategoryAsync(int categoryId);
    }
}
