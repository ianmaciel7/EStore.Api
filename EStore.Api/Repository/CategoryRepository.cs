using EStore.API.Data;
using EStore.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            query.Include(c => c.SubCategories).ThenInclude(s => s.Products);

            var cats = query.Where(c => c.Name == categoryName);
            var subs = cats.SelectMany(c => c.SubCategories).Where(s => s.Name == subCategoryName);
            var products = subs.SelectMany(s => s.Products).ToDictionary(p => p.ProductId);
 
            return await Task.FromResult(products.Values.Skip((page - 1) * quantity).Take(quantity).ToArray());

        }

        public async Task<Category> GetCategoryAsync(string categoryName,bool includeSubCategories = false, bool includeProducts = false)
        {
            IQueryable<Category> query = _appDbContext.Categories;

            if (includeSubCategories)
            {
                if (includeProducts)
                    query.Include(c => c.SubCategories).ThenInclude(s => s.Products);
                else
                    query.Include(c => c.SubCategories);                                  
            }
            
            query = query.Where(c => c.Name == categoryName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName, bool includeProducts = true)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;

            if (includeProducts)
                query.Include(s => s.Products);
           
            query = query.Where(s => s.Category.Name == categoryName)
                         .Where(s => s.Name == subCategoryName);

            return await query.FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
