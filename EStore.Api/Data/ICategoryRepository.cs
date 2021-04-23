using EStore.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EStore.API.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> AllAsync();
        Task<Category> GetByNameAsync(string name);
    }
}