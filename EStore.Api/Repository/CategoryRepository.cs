using EStore.Api.InputModel;
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

        public Task<Product> GetProductAsync(string categoryName, string subCategoryName, int productId)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            query.Include(c => c.SubCategories).ThenInclude(s => s.Products);

            var cats = query.Where(c => c.Name == categoryName);
            var subs = cats.SelectMany(c => c.SubCategories).Where(s => s.Name == subCategoryName);
            var prod = subs.SelectMany(s => s.Products).Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return prod;
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            IQueryable<Category> query = _appDbContext.Categories;    
            query.Include(c => c.SubCategories).ThenInclude(s => s.Products);                                                                    
            query = query.Where(c => c.Name == categoryName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query.Include(s => s.Products).Include(s => s.Category);

            query = query.Where(s => s.Category.Name == categoryName);
            query = query.Where(s => s.Name == subCategoryName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductAsync(string productName)
        {
            IQueryable<Product> query = _appDbContext.Products;

            query = query.Where(p => p.Name == productName);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> AddProductAsync(string subCategoryName, Product product)
        {
            var entityEntry = await _appDbContext.Products.AddAsync(product);
            var subCategory = _appDbContext.SubCategories.FirstOrDefaultAsync(s => s.Name == subCategoryName).Result;
            subCategory.Products.Add(product);           
            await _appDbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
