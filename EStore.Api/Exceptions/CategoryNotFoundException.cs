using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class CategoryNotFoundException : Exception
    {        
        public CategoryNotFoundException(string categoryName) 
            : base($"Não foi possivel encontrar categoria com nome '{categoryName}'")
        {
        }
    }
}
