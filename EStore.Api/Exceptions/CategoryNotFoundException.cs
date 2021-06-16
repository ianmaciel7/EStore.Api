using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class CategoryNotFoundException : Exception
    {        
        public CategoryNotFoundException(string categoryName) 
            : base($"Could not find category with this name '{categoryName}'")
        {
        }
    }
}
