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

        public void DeleteCategory(Category category)
        {
            _appDbContext.Categories.Remove(category);
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var entityEntry = await _appDbContext.Categories.AddAsync(category);
            return entityEntry.Entity;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IQueryable<Category> query = _appDbContext.Categories;
            //query = query.Include(c => c.SubCategories);
            return await Task.FromResult(query.ToArray());
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync(string categoryName)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Include(c => c.Category).Include(c => c.Products);
            query = query.Where(c => c.Category.Name == categoryName);
            
            return await Task.FromResult(query.ToArray());
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            query = query.Include(c => c.SubCategories).ThenInclude(s => s.Products);

            var cats = query.Where(c => c.Name == categoryName);
            var subs = cats.SelectMany(c => c.SubCategories).Where(s => s.Name == subCategoryName);
            var products = subs.SelectMany(s => s.Products).ToDictionary(p => p.ProductId);
 
            return await Task.FromResult(products.Values.Skip((page - 1) * quantity).Take(quantity).ToArray());

        }

        public Task<Product> GetProductAsync(string categoryName, string subCategoryName, int productId)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            query = query.Include(c => c.SubCategories).ThenInclude(s => s.Products);

            var cats = query.Where(c => c.Name == categoryName);
            var subs = cats.SelectMany(c => c.SubCategories).Where(s => s.Name == subCategoryName);
            var prod = subs.SelectMany(s => s.Products).Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return prod;
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            IQueryable<Category> query = _appDbContext.Categories;    
            query = query.Include(c => c.SubCategories).ThenInclude(s => s.Products);                                                                    
            query = query.Where(c => c.Name == categoryName);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryAsync(string categoryName, string subCategoryName)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Include(s => s.Products).Include(s => s.Category);

            query = query.Where(s => s.Category.Name == categoryName);
            query = query.Where(s => s.Name == subCategoryName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryAsync(string categoryName, int subCategoryId)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Include(s => s.Products).Include(s => s.Category);

            query = query.Where(s => s.Category.Name == categoryName);
            query = query.Where(s => s.SubCategoryId == subCategoryId);

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
            return entityEntry.Entity;
        }

        public void UpdateProduct(Product newProduct)
        {
            var products = _appDbContext.Products;
            var oldProduct = products.Where(p => p.ProductId == newProduct.ProductId).FirstOrDefault();
            oldProduct.Name = newProduct.Name;
            oldProduct.Price = newProduct.Price;
        }
        public void DeleteProduct(Product product)
        {
            _appDbContext.Remove(product);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        public async Task<SubCategory> AddSubCategoryAsync(string categoryName,SubCategory subCategory)
        {
            var entityEntry = await _appDbContext.SubCategories.AddAsync(subCategory);
            var category = _appDbContext.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            category.Result.SubCategories.Add(entityEntry.Entity);
            return entityEntry.Entity;
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            IQueryable<Category> query = _appDbContext.Categories;
            query = query.Include(c => c.SubCategories).ThenInclude(s => s.Products);
            query = query.Where(c => c.CategoryId == categoryId);
            return await query.FirstOrDefaultAsync();
        }

        public void UpdateCategory(Category newCategory)
        {
            var categories = _appDbContext.Categories;
            var oldCategory = categories.Where(c => c.CategoryId == newCategory.CategoryId).FirstOrDefault();
            oldCategory.Name = newCategory.Name;
            oldCategory.SubCategories = newCategory.SubCategories;
        }

        public async Task<SubCategory> GetSubCategoryAsync(string subCategoryName)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Include(s => s.Category).Include(s => s.Products);
            query = query.Where(s => s.Name == subCategoryName);
            return await query.FirstOrDefaultAsync();
        }

        public void UpdateSubCategory(SubCategory newSubCategory)
        {
            IQueryable<SubCategory> query = _appDbContext.SubCategories;
            query = query.Include(s => s.Category).Include(s => s.Category);
            var oldSubCategory = query.Where(c => c.SubCategoryId == newSubCategory.SubCategoryId).FirstOrDefault();
            oldSubCategory.Name = newSubCategory.Name;
            oldSubCategory.Category = newSubCategory.Category;
            oldSubCategory.Products = newSubCategory.Products;            
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            _appDbContext.SubCategories.Remove(subCategory);
        }
    }
}
