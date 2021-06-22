using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Repository;
using EStore.Api.ViewModel;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.InputModel;
using EStore.API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<CategoryViewModel> GetCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            if (!IsThereThisCategory(category))
                throw new CategoryNotFoundException(categoryId);

            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                SubCategories = category.SubCategories                
            };
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryAsync(categoryId);

            if (!IsThereThisCategory(category))
                throw new CategoryNotFoundException(categoryId);

            if (!category.SubCategories.Any())
                throw new CategoryContainsSubcategoriesException(categoryId);

            _categoryRepository.DeleteCategory(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<CategoryViewModel> AddCategoryAsync(CategoryInputModel model)
        {
            
            var category = new Category()
            {               
                Name = model.Name,               
            };
                      
            if (await IsThereThisProductAsync(model.Name))
                throw new CategoryNameNotUniqueException(model.Name);
         
            var addedCategory = _categoryRepository.AddCategoryAsync(category).Result;
            await _categoryRepository.SaveChangesAsync();
            return new CategoryViewModel
            {
                CategoryId = addedCategory.CategoryId,
                Name = addedCategory.Name,
                SubCategories = addedCategory.SubCategories               
            };

        }

        public async Task<CategoryViewModel> UpdateCategoryAsync(int categoryId, CategoryInputModel model)
        {
            var categoryOld = await _categoryRepository.GetCategoryAsync(categoryId);
            
            if (!IsThereThisCategory(categoryOld))
                throw new CategoryNotFoundException(categoryId);

            if (await IsThereThisCategoryAsync(model.Name))
                throw new CategoryNameNotUniqueException(model.Name);

            var category = new Category()
            {
                CategoryId = categoryOld.CategoryId,
                SubCategories = categoryOld.SubCategories,
                Name = model.Name
            };

            _categoryRepository.UpdateCategory(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,                
                Name = category.Name,
                SubCategories = category.SubCategories
            };
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryViewModel
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                SubCategories = c.SubCategories
            });
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetAllSubCategoriesAsync(string categoryName)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);
           
            var subCategories = await _categoryRepository.GetAllSubCategoriesAsync(categoryName);
            return subCategories.Select(s => new SubCategoryViewModel
            {
                SubCategoryId = s.SubCategoryId,
                Name = s.Name,
                Products = s.Products
            });
        }

        public async Task<SubCategoryViewModel> GetSubCategoryAsync(string categoryName, int subCategoryId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            var subCategory = await _categoryRepository.GetSubCategoryAsync(categoryName, subCategoryId);

            if (!IsThereThisSubCategory(subCategory))
                throw new SubCategoryNotFoundException(categoryName,subCategoryId);

            return new SubCategoryViewModel
            {
                SubCategoryId = subCategory.SubCategoryId,
                Name = subCategory.Name,
                Products = subCategory.Products
            };
        }

        public async Task<SubCategoryViewModel> AddSubCategoryAsync(string categoryName, CategoryInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (await IsThereThisProductAsync(model.Name))
                throw new SubCategoryNameNotUniqueException(model.Name);

            var subCategory = new SubCategory()
            {               
                Name = model.Name
            };

            var addedSubCategory = await _categoryRepository.AddSubCategoryAsync(categoryName,subCategory);
            await _categoryRepository.SaveChangesAsync();

            return new SubCategoryViewModel
            {
                SubCategoryId = addedSubCategory.SubCategoryId,
                Name = addedSubCategory.Name,
                Products = addedSubCategory.Products
            };
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync(string categoryName, string subCategoryName, int page, int quantity)
        {

            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var products = await _categoryRepository.GetAllProductsAsync(categoryName, subCategoryName, page, quantity);

            return products.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Price = p.Price,
                Name = p.Name              
            });
        }

        public async Task<ProductViewModel> GetProductAsync(string categoryName, string subCategoryName, int productId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))
                throw new ProductNotFoundException(categoryName,productId);

            return new ProductViewModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                Name = product.Name
            };
        }

        public async Task<ProductViewModel> AddProductAsync(string categoryName, string subCategoryName, ProductInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            if (await IsThereThisProductAsync(model.Name))
                throw new ProductNameNotUniqueException(model.Name);

            var product = new Product()
            {
                Price = model.Price,
                Name = model.Name,  
            };

            var addedProduct = await _categoryRepository.AddProductAsync(subCategoryName,product);
            await _categoryRepository.SaveChangesAsync();
            return new ProductViewModel
            {
                ProductId = addedProduct.ProductId,
                Price = addedProduct.Price,
                Name = addedProduct.Name
            };

        }

        public async Task<ProductViewModel> UpdateProductAsync(string categoryName, string subCategoryName, int productId, ProductInputModel model)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))            
                throw new ProductNotFoundException(subCategoryName, productId);            
                
            if (await IsThereThisProductAsync(model.Name))
                throw new ProductNameNotUniqueException(model.Name);

            var newProduct = new Product()
            {
                ProductId = productId,
                Price = model.Price,
                Name = model.Name,
            };

            _categoryRepository.UpdateProduct(newProduct);
            await _categoryRepository.SaveChangesAsync();

            return new ProductViewModel
            {
                ProductId = newProduct.ProductId,
                Price = newProduct.Price,
                Name = newProduct.Name
            };

        }

        public async Task DeleteProductAsync(string categoryName, string subCategoryName, int productId)
        {
            if (!await IsThereThisCategoryAsync(categoryName))
                throw new CategoryNotFoundException(categoryName);

            if (!await IsThereThisSubCategoryAsync(categoryName, subCategoryName))
                throw new SubCategoryNotFoundException(categoryName, subCategoryName);

            var product = await _categoryRepository.GetProductAsync(categoryName, subCategoryName, productId);

            if (!IsThereThisProduct(product))
                throw new ProductNotFoundException(subCategoryName, productId);

            _categoryRepository.DeleteProduct(product);
            await _categoryRepository.SaveChangesAsync();
        }

        private async Task<bool> IsThereThisCategoryAsync(string categoryName)
        {
            var existing = await _categoryRepository.GetCategoryAsync(categoryName);
            if (existing != null) return true;
            return false;
        }

        private async Task<bool> IsThereThisSubCategoryAsync(string categoryName, string subCategoryName)
        {
            var existing = await _categoryRepository.GetSubCategoryAsync(categoryName, subCategoryName);
            if (existing != null) return true;
            return false;
        }

        private async Task<bool> IsThereThisProductAsync(string productName)
        {
            var existing = await _categoryRepository.GetProductAsync(productName);
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisProduct(Product product)
        {
            var existing = product;
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisSubCategory(SubCategory subCategory)
        {
            var existing = subCategory;
            if (existing != null) return true;
            return false;
        }

        private bool IsThereThisCategory(Category category)
        {
            var existing = category;
            if (existing != null) return true;
            return false;
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }        
    }
}
