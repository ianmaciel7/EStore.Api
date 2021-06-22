using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class CategoryContainsSubCategoriesException : Exception
    {
        public CategoryContainsSubCategoriesException(int categoryId) 
            : base($"Não é possivel remover a categoria com id '{categoryId}' porque há uma referência em uma ou mais de uma subcategorias")
        {
        }
    }
}
