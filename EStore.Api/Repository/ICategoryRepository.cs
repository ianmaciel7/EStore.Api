using EStore.API.Data.Entities;
using EStore.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> AllCategories(bool includeSubCategories);
        Task<Category> GetCategoryByName(string name);
        Task<SubCategory> GetSubCategoryByNameCategoryAndIdSubCategory(string name, int id);        
        void AddCategory(Category category);
        void AddSubCategory(SubCategory subCategory);
        Task<bool> SaveChangesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        void DeleteSubCategory(SubCategory subCategory);
        void DeleteCategory(Category category);
        Task<SubCategory> GetSubCategoryByNameSubCategory(string name);
        Task<SubCategory> GetSubCategoryByIdSubCategory(int id);
        Task<SubCategory> GetSubCategoryByNameCategoryAndNameSubCategory(string nameCategory, string nameSub);
    }
}