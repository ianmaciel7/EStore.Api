using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class SubCategoryNotFoundException : Exception
    {
        public SubCategoryNotFoundException(string categoryName,string subcategoryName)
            : base($"Could not find subcategory with this name '{categoryName}' in category '{subcategoryName}'")
        {
        }
    }
}
