using EStore.API.Data;
using EStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Services
{
    public interface IProductService
    {
        Task<Product> GetProductEntityByName(string name);
        Task<ProductModel> GetProductByName(string name);
        Task<ProductModel> AddProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(string name, ProductModel model);
        Task<bool> DeleteProduct(Product product);
        Task<bool> IsThereThisProduct(string name);
        Task<ProductModel[]> GetProductByNameCategoryAndNameSubCategory(string nameCat, string nameSub);
        Task<ProductModel> GetProductByNameCategoryAndNameSubCategoryAndNameProduct(string nameCat, string nameSub, string nameProd);
        Task<ProductModel> GetProductByNameCategoryAndNameSubCategoryAndIdProduct(string nameCat, string nameSub, int id);
        Task<bool> IsThereThisProduct(int idProd);
        Task<ProductModel> UpdateProduct(int idProd, ProductModel model);
        Task<Product> GetProductEntityById(int idProd);
        Task<ProductModel> GetProductById(int idProd);
    }
}
