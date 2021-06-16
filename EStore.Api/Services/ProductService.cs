using AutoMapper;
using EStore.API.Data;
using EStore.API.Models;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IMapper mapper)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;

        }
        
        public async Task<Product> GetProductEntityByName(string name)
        {
            var results = await _productRepository.GetProductByName(name);
            return results;
        }

        public async Task<ProductModel> GetProductByName(string name)
        {
            var results = await _productRepository.GetProductByName(name);
            return _mapper.Map<ProductModel>(results);
        }

        public async Task<ProductModel> AddProduct(ProductModel model)
        {        
            var product = _mapper.Map<Product>(model);
            _productRepository.AddProducts(product);

            await _productRepository.SaveChangesAsync();
            return model;         
        }

        public async Task<ProductModel> UpdateProduct(string name, ProductModel model)
        {          
            var oldProduct = await _productRepository.GetProductByName(name);
            
            _mapper.Map(model, oldProduct);

            await _productRepository.SaveChangesAsync();
            return model;         
        }

        public async Task<bool> IsThereThisProduct(string name)
        {
            var existing = await _productRepository.GetProductByName(name);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _productRepository.DeleteProduct(product);
            return await _productRepository.SaveChangesAsync();
        }

        public async Task<ProductModel[]> GetProductByNameCategoryAndNameSubCategory(string nameCat, string nameSub)
        {
            var result = await _productRepository.GetProductByNameCategoryAndNameSubCategory(nameCat, nameSub);
            var model = _mapper.Map<ProductModel[]>(result);
            return model;
        }

        public async Task<ProductModel> GetProductByNameCategoryAndNameSubCategoryAndNameProduct(string nameCat, string nameSub, string nameProd)
        {
            var result = await _productRepository.GetProductByNameCategoryAndNameSubCategoryAndNameProduct(nameCat, nameSub,nameProd);
            var model = _mapper.Map<ProductModel>(result);
            return model;
        }

        public async Task<ProductModel> GetProductByNameCategoryAndNameSubCategoryAndIdProduct(string nameCat, string nameSub,int id)
        {
            var result = await _productRepository.GetProductByNameCategoryAndNameSubCategoryAndIdProduct(nameCat, nameSub, id);
            var model = _mapper.Map<ProductModel>(result);
            return model;
        }

        public async Task<bool> IsThereThisProduct(int idProd)
        {
            var existing = await _productRepository.GetProductByIdProduct(idProd);
            if (existing != null) return true;
            return false;
        }

        public async Task<ProductModel> UpdateProduct(int idProd, ProductModel model)
        {
            var oldProduct = await _productRepository.GetProductByIdProduct(idProd);

            _mapper.Map(model, oldProduct);

            await _productRepository.SaveChangesAsync();
            return model;
        }

        public async Task<Product> GetProductEntityById(int idProd)
        {
            var results = await _productRepository.GetProductByIdProduct(idProd);
            return results;
        }

        public async Task<ProductModel> GetProductById(int idProd)
        {
            var results = await _productRepository.GetProductByIdProduct(idProd);
            return _mapper.Map<ProductModel>(results);
        }
    }
}
