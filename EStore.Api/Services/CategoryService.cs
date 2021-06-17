using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Repository;
using EStore.Api.ViewModel;
using EStore.API.Data;
using System;
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
        
        public async Task<IEnumerable<ProductViewModel>> GetAllProductAsync(string categoryName, string subCategoryName, int page, int quantity)
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

        public async Task<ProductViewModel> GetProduct(string categoryName, string subCategoryName, int productId)
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

            return new ProductViewModel
            {
                ProductId = addedProduct.ProductId,
                Price = addedProduct.Price,
                Name = addedProduct.Name
            };

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

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }        
    }
}
