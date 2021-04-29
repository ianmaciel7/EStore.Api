using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public void AddProducts(Product product)
        {
            _appDbContext.Products.Add(product);
        }

        public async Task<IEnumerable<Product>> AllProductsAsync()
        {
            IQueryable<Product> query = _appDbContext.Products
           .Include(c => c.SubCategory);


            query = query.Include(c => c.SubCategory)
                  .ThenInclude(t => t.Category);

            return await query.ToListAsync();
        }

        public void DeleteProduct(Product product)
        {
           _appDbContext.Products.Remove(product);
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            IQueryable<Product> query = _appDbContext.Products
           .Include(c => c.SubCategory).ThenInclude(t => t.Category);

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await (_appDbContext.SaveChangesAsync()) > 0;
        }
    }
}
