using EStore.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        } 

        public async Task<IEnumerable<Category>> AllAsync()
        {
           
            var query = appDbContext.Categories
                .Include(c => c.SubCategories);
           
            return await query.ToListAsync();
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            IQueryable<Category> query = appDbContext.Categories
           .Include(c => c.SubCategories);

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }
    }
}
