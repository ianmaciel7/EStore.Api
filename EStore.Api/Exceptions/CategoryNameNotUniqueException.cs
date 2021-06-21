using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class CategoryNameNotUniqueException : Exception
    {
        public CategoryNameNotUniqueException(string categoryName)
            : base($"There is already a category with name '{categoryName}'")
        {
        }
    }
}
