using EStore.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EStore.API.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddSubCategory(SubCategory subCategory)
        {
            _appDbContext.SubCategories.AddAsync(subCategory);
        }

        public async Task<IEnumerable<Category>> AllCategoriesAsync(bool includeSubCategories = false)
        {
            if (includeSubCategories == true)
            {
                var query = _appDbContext.Categories
               .Include(c => c.SubCategories);
                return await query.ToListAsync();
            }
            else
            {
                var query = _appDbContext.Categories;
                return await query.ToListAsync();
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string name, bool includeSubCategories = false)
        {
            IQueryable<Category> query;

            if (includeSubCategories)
            {
                query = _appDbContext.Categories
               .Include(c => c.SubCategories);
            }
            else
            {
                query = _appDbContext.Categories;
            }

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByNameCategory(string name)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Where(s => s.Category.Name == name);
            return await query.ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdSubCategory(string name, int id)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories.Include(s => s.Category);
            query = query.Where(s => s.Category.Name == name);
            query = query.Where(s => s.SubCategoryId == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryByNameSubCategory(string name, string nameSubCategory)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Where(s => s.Category.Name == name);
            query = query.Where(s => s.Name == name);
            return await query.FirstOrDefaultAsync();
        }



        public async Task<bool> SaveChangesAsync()
        {
            return await (_appDbContext.SaveChangesAsync()) > 0;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            IQueryable<Category> query = _appDbContext.Categories.Include(c => c.SubCategories);
            query = query.Where(c => c.CategoryId == categoryId);

            return await query.FirstOrDefaultAsync();
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            _appDbContext.SubCategories.Remove(subCategory);
        }

        public void DeleteCategory(Category category)
        {
            _appDbContext.Categories.Remove(category);
        }

        public void AddCategory(Category category)
        {
            _appDbContext.Categories.AddAsync(category);
        }
    }
}