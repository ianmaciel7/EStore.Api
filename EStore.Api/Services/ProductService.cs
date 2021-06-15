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

        public async Task<ProductModel[]> AllProductsAsync()
        {
            var results = await _productRepository.AllProductsAsync();
            return _mapper.Map<ProductModel[]>(results);           
        }

        public async Task<Product> GetProductEntityByNameAsync(string name)
        {
            var results = await _productRepository.GetProductByNameAsync(name);
            return results;
        }

        public async Task<ProductModel> GetProductByNameAsync(string name)
        {
            var results = await _productRepository.GetProductByNameAsync(name);
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
            var oldProduct = await _productRepository.GetProductByNameAsync(name);
            
            _mapper.Map(model, oldProduct);

            await _productRepository.SaveChangesAsync();
            return model;         
        }

        public async Task<bool> IsThereThisProduct(string name)
        {
            var existing = await _productRepository.GetProductByNameAsync(name);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            _productRepository.DeleteProduct(product);
            return await _productRepository.SaveChangesAsync();
        }
    }
}
