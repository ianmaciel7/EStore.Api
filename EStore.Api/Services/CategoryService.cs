using AutoMapper;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        public async Task<CategoryModel> AddCategory(CategoryModel model)
        {
            var category = _mapper.Map<Category>(model);
            _categoryRepository.AddCategory(category);

            await _categoryRepository.SaveChangesAsync();
            return model;
        }

        public async Task<SubCategoryModel> AddSubCategory(SubCategoryModel model)
        {
            var sub = _mapper.Map<SubCategory>(model);
            _categoryRepository.AddSubCategory(sub);

            await _categoryRepository.SaveChangesAsync();
            return model;
        }

        public async Task<CategoryModel[]> AllCategories(bool includeSubCategories)
        {
            var results = await _categoryRepository.AllCategories(includeSubCategories);
            return _mapper.Map<CategoryModel[]>(results);
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _categoryRepository.DeleteCategory(category);
            return await _categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSubCategory(SubCategory sub)
        {          
            _categoryRepository.DeleteSubCategory(sub);
            return await _categoryRepository.SaveChangesAsync();
        }

        public async Task<CategoryModel> GetCategoryByName(string name)
        {
            var result = await _categoryRepository.GetCategoryByName(name);
            var model = _mapper.Map<CategoryModel>(result);
            return model;
        }

        public async Task<Category> GetCategoryEntityByName(string name)
        {
            var result = await _categoryRepository.GetCategoryByName(name);
            return result;
        }

        public async Task<SubCategoryModel> GetSubCategoryByNameCategoryAndIdSubCategory(string name, int id)
        {
            var result = await _categoryRepository.GetSubCategoryByNameCategoryAndIdSubCategory(name,id);
            var model = _mapper.Map<SubCategoryModel>(result);
            return model;
        }

        public async Task<SubCategoryModel> GetSubCategoryByNameCategoryAndNameSubCategory(string nameCat, string nameSub)
        {
            var result = await _categoryRepository.GetSubCategoryByNameCategoryAndNameSubCategory(nameCat, nameSub);
            var model = _mapper.Map<SubCategoryModel>(result);
            return model;
        }

        public async Task<SubCategory> GetSubCategoryEntityByIdSubCategoryAndNameCategory(string name, int id)
        {
            var result = await _categoryRepository.GetSubCategoryByNameCategoryAndIdSubCategory(name, id);          
            return result;
        }

        public async Task<bool> IsThereThisCategory(string name)
        {
            var existing = await _categoryRepository.GetCategoryByName(name);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> IsThereThisCategory(int id)
        {
            var existing = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> IsThereThisSubCategory(string name)
        {
            var existing = await _categoryRepository.GetSubCategoryByNameSubCategory(name);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> IsThereThisSubCategory(int id)
        {
            var existing = await _categoryRepository.GetSubCategoryByIdSubCategory(id);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> IsThereThisSubCategory(string nameCategory, int idSub)
        {
            var existing = await _categoryRepository.GetSubCategoryByNameCategoryAndIdSubCategory(nameCategory, idSub);
            if (existing != null) return true;
            return false;
        }

        public async Task<bool> IsThereThisSubCategory(string nameCategory, string nameSub)
        {
            var existing = await _categoryRepository.GetSubCategoryByNameCategoryAndNameSubCategory(nameCategory, nameSub);
            if (existing != null) return true;
            return false;
        }

        public async Task<CategoryModel> UpdateCategory(string name, CategoryModel model)
        {
            var oldCategory = await _categoryRepository.GetCategoryByName(name);

            _mapper.Map(model, oldCategory);

            await _categoryRepository.SaveChangesAsync();
            return model;
        }

        public async Task<SubCategoryModel> UpdateSubCategory(string nameSubCategory, SubCategoryModel model)
        {
            var oldSub = await _categoryRepository.GetSubCategoryByNameSubCategory(nameSubCategory);
            _mapper.Map(model, oldSub);
            await _categoryRepository.SaveChangesAsync();
            return model;
        }

        public async Task<SubCategoryModel> UpdateSubCategory(int id, SubCategoryModel model)
        {
            var oldSub = await _categoryRepository.GetSubCategoryByIdSubCategory(id);
            _mapper.Map(model, oldSub);
            await _categoryRepository.SaveChangesAsync();
            return model;
        }
    }
}
