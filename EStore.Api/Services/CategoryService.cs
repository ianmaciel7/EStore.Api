using EStore.Api.Exceptions;
using EStore.Api.Repository;
using EStore.Api.ViewModel;
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

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
    }
}
