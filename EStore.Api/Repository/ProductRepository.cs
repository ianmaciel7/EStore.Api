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

        public async Task<IEnumerable<Product>> AllProducts()
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

        public async Task<Product> GetProductByIdProduct(int idProd)
        {
            IQueryable<Product> query = _appDbContext.Products
           .Include(c => c.SubCategory).ThenInclude(t => t.Category);

            query = query.Where(c => c.ProductId == idProd);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            IQueryable<Product> query = _appDbContext.Products
           .Include(c => c.SubCategory).ThenInclude(t => t.Category);

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameCategoryAndNameSubCategory(string nameCat, string nameSub)
        {
            IQueryable<Product> query = _appDbContext.Products;

            query = query.Include(c => c.SubCategory)
                  .ThenInclude(t => t.Category);

            query = query.Where(p => p.SubCategory.Name == nameSub);
            query = query.Where(p => p.SubCategory.Category.Name == nameCat);

            return await query.ToListAsync();
        }

        public async Task<Product> GetProductByNameCategoryAndNameSubCategoryAndIdProduct(string nameCat, string nameSub, int id)
        {
            IQueryable<Product> query = _appDbContext.Products;

            query = query.Include(c => c.SubCategory)
                  .ThenInclude(t => t.Category);

            query = query.Where(p => p.SubCategory.Name == nameSub);
            query = query.Where(p => p.SubCategory.Category.Name == nameCat);
            query = query.Where(p => p.ProductId == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByNameCategoryAndNameSubCategoryAndNameProduct(string nameCat, string nameSub, string nameProd)
        {
            IQueryable<Product> query = _appDbContext.Products;

            query = query.Include(c => c.SubCategory)
                  .ThenInclude(t => t.Category);
            
            query = query.Where(p => p.SubCategory.Name == nameSub);
            query = query.Where(p => p.SubCategory.Category.Name == nameCat);
            query = query.Where(p => p.Name == nameProd);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await (_appDbContext.SaveChangesAsync()) > 0;
        }
    }
}
