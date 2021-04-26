using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void Add(Product product)
        {
            appDbContext.Products.Add(product);
        }

        public async Task<IEnumerable<Product>> AllAsync()
        {
            IQueryable<Product> query = appDbContext.Products
           .Include(c => c.SubCategory);


            query = query.Include(c => c.SubCategory)
                  .ThenInclude(t => t.Category);
          
            return await query.ToListAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            IQueryable<Product> query = appDbContext.Products
           .Include(c => c.SubCategory).ThenInclude(t => t.Category); 

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await (appDbContext.SaveChangesAsync()) > 0;
        }
    }
}
