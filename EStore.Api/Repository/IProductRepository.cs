using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> AllProductsAsync();
        Task<Product> GetProductByNameAsync(string name);
        void AddProducts(Product product);
        void DeleteProduct(Product product);
        Task<bool> SaveChangesAsync();

    }
}
