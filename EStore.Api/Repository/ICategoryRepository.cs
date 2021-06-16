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
        Task<Category> GetCategoryAsync(string categoryName, bool includeSubCategories = false, bool includeProducts = false);
        Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName, bool includeProducts = true);
        Task<IEnumerable<Product>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity);
    }
}
