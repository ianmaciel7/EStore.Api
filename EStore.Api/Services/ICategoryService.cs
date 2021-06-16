using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Services
{
    public interface ICategoryService
    {
        Task<CategoryModel[]> AllCategories(bool includeSubCategories);             
        Task<CategoryModel> GetCategoryByName(string name);
        Task<Category> GetCategoryEntityByName(string name);   
        Task<SubCategoryModel> GetSubCategoryByNameCategoryAndIdSubCategory(string name, int id);
        Task<SubCategory> GetSubCategoryEntityByIdSubCategoryAndNameCategory(string name, int id);
        Task<SubCategoryModel> GetSubCategoryByNameCategoryAndNameSubCategory(string nameCat, string nameSub);
        Task<CategoryModel> AddCategory(CategoryModel model);
        Task<SubCategoryModel> AddSubCategory(SubCategoryModel subcategory);    
        Task<bool> DeleteSubCategory(SubCategory sub);
        Task<bool> DeleteCategory(Category category);
        Task<CategoryModel> UpdateCategory(string name, CategoryModel model);
        Task<SubCategoryModel> UpdateSubCategory(string nameSubCategory, SubCategoryModel model);
        Task<SubCategoryModel> UpdateSubCategory(int id, SubCategoryModel model);
        Task<bool> IsThereThisCategory(string name);
        Task<bool> IsThereThisCategory(int id);
        Task<bool> IsThereThisSubCategory(string name);
        Task<bool> IsThereThisSubCategory(int id);
        Task<bool> IsThereThisSubCategory(string nameCategory, int idSub);
        Task<bool> IsThereThisSubCategory(string nameCategory, string nameSub);
        
    }
}
