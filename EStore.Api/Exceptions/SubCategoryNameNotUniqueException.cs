using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class SubCategoryNameNotUniqueException : Exception
    {
        public SubCategoryNameNotUniqueException(string subCategoryName)
            : base($"There is already a subcategory with name '{subCategoryName}'")
        {
        }
    }
}
