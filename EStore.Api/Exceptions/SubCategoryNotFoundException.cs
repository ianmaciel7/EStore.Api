using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Exceptions
{
    public class SubCategoryNotFoundException : Exception
    {       
        public SubCategoryNotFoundException(string categoryName,int subCategoryId)
            : base($"Não foi possivel encontrar subcategoria com id '{subCategoryId}' na categoria '{categoryName}'")
        {          
        }

        public SubCategoryNotFoundException(string categoryName,string subcategoryName)
            : base($"Não foi possivel encontrar subcategoria com nome '{subcategoryName}' na categoria '{categoryName}'")
        {
        }
    }
}
