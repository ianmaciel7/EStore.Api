using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class SubCategoriesContainsProductsException : Exception
    {
        public SubCategoriesContainsProductsException(int subCategoryId) 
            : base($"Não é possivel remover a subcategoria com id '{subCategoryId}' porque há uma referência de um ou mais produtos")
        {
        }
    }
}
