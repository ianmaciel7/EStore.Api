using EStore.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> AllCategoriesAsync(bool includeSubCategories = false);
        Task<Category> GetCategoryByNameAsync(string name, bool includeSubCategories = false);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByNameCategory(string name);
        Task<SubCategory> GetSubCategoryByIdSubCategory(string name, int id);
        Task<SubCategory> GetSubCategoryByNameSubCategory(string name,string nameSubCategory);
        void AddCategory(Category category);
        void AddSubCategory(SubCategory subCategory);
        Task<bool> SaveChangesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        void DeleteSubCategory(SubCategory subCategory);
        void DeleteCategory(Category category);
    }
}