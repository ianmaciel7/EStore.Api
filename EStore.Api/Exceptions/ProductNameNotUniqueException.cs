using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class ProductNameNotUniqueException : Exception
    {
        public ProductNameNotUniqueException(string productName) 
            : base($"There is already a product with name '{productName}'")
        {
        }
    }
}
