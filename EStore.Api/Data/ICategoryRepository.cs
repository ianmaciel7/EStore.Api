using EStore.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> AllAsync(bool includeSubCategories = false);
        Task<Category> GetByNameAsync(string name, bool includeSubCategories = false);
    }
}