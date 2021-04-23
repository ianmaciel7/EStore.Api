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

        public async Task<IEnumerable<Category>> AllAsync(bool includeSubCategories = false)
        {
            if (includeSubCategories == true)
            {
                var query = appDbContext.Categories
               .Include(c => c.SubCategories);
                return await query.ToListAsync();
            }
            else
            {
                var query = appDbContext.Categories;
                return await query.ToListAsync();
            }
        }

        public async Task<Category> GetByNameAsync(string name, bool includeSubCategories = false)
        {
            IQueryable<Category> query;

            if (includeSubCategories)
            {
                query = appDbContext.Categories
               .Include(c => c.SubCategories);
            }
            else
            {
                query = appDbContext.Categories;
            }

            query = query.Where(c => c.Name == name);

            return await query.FirstOrDefaultAsync();
        }
    }
}