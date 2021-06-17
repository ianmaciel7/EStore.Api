using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string subcategoryName, int productId) 
            : base($"Não foi possivel encontrar produto com id '{productId}' na subcategoria '{subcategoryName}'")
        {
        }
    }
}
