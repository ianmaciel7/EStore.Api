using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> AllProducts();
        Task<Product> GetProductByName(string name);
        void AddProducts(Product product);
        void DeleteProduct(Product product);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Product>> GetProductByNameCategoryAndNameSubCategory(string nameCat, string nameSub);
        Task<Product> GetProductByNameCategoryAndNameSubCategoryAndNameProduct(string nameCat, string nameSub, string nameProd);
        Task<Product> GetProductByNameCategoryAndNameSubCategoryAndIdProduct(string nameCat, string nameSub, int id);
        Task<Product> GetProductByIdProduct(int idProd);
    }
}
