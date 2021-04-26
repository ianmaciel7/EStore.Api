using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> AllAsync();
        Task<Product> GetByNameAsync(string name);
        void Add(Product product);
        void Delete(Product product);
        Task<bool> SaveChangesAsync();

    }
}
