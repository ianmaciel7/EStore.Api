using EStore.Api.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Services
{
    public interface ICategoryService : IDisposable
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductAsync(string categoryName, string subCategoryName, int page,int quantity);
        
    }
}
