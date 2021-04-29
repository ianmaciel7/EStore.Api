using AutoMapper;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Controllers
{
    [Route("api/categories/{name}/subCategories")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public SubCategoriesController(
            ICategoryRepository categoryRepository,
            IMapper mapper,LinkGenerator linkGenerator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<SubCategoryModel[]>> Get(string name)
        {
            try
            {
                var sub = await _categoryRepository.GetSubCategoriesByNameCategory(name);
                return _mapper.Map<SubCategoryModel[]>(sub);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubCategoryModel>> Get(string name,int id)
        {
            try
            {
                var sub = await _categoryRepository.GetSubCategoryByIdSubCategory(name,id);
                return _mapper.Map<SubCategoryModel>(sub);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubCategoryModel>> Post(string name, SubCategoryModel subCategoryModel)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByNameAsync(name);
                if (category == null) BadRequest("Category does not exist");

                var subcategory = _mapper.Map<SubCategory>(subCategoryModel);
                subcategory.Category = category;
                _categoryRepository.AddSubCategory(subcategory);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(
                        HttpContext,
                        "Get",
                        values: new { name, id = subcategory.SubCategoryId });

                    return Created(url, _mapper.Map<SubCategoryModel>(subcategory));
                }
                else
                {
                    return BadRequest("Failed to save new Sub Category");
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubCategoryModel>> Put(string name, int id,SubCategoryModel subCategoryModel)
        {
            try
            {
                var sub = await _categoryRepository.GetSubCategoryByIdSubCategory(name, id);
                if (sub == null) return NotFound("Couldn't find the subcategory");

                if (sub.Category != null)
                {
                    var category = await _categoryRepository.GetCategoryByIdAsync(sub.Category.CategoryId);
                    if (category == null) return NotFound("Couldn't find the category");
                    sub.Category = category;
                }

                _mapper.Map(subCategoryModel, sub);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    return _mapper.Map<SubCategoryModel>(sub);
                }
                else
                {
                    return BadRequest("Failed to update database");
                }
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<SubCategoryModel>> Delete(string name, int id)
        {
            try
            {
                var sub = await _categoryRepository.GetSubCategoryByIdSubCategory(name, id);
                if (sub == null) return NotFound("Couldn't find the subcategory");
                _categoryRepository.DeleteSubCategory(sub);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to delete talk");
                }

            }
            catch (DbUpdateException)
            {
                return BadRequest("You cannot remove the subcategory because there is a reference in one or more products");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
