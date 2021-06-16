using AutoMapper;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using EStore.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Controllers
{
    [ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public CategoriesController(ICategoryService categoryService, LinkGenerator linkGenerator)
        {
            _categoryService = categoryService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(bool includeSubCategories = false)
        {
            try
            {
                var result = await _categoryService.AllCategories(includeSubCategories);
                return Ok(result);            
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{nameCat}")]
        public async Task<ActionResult> Get(string nameCat)
        {
            try
            {
                var result = await _categoryService.GetCategoryByName(nameCat);
                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost()]
        public async Task<ActionResult<CategoryModel>> Post(CategoryModel model)
        {
            try
            {

                if (await _categoryService.IsThereThisCategory(model.Name))
                    return BadRequest($"There is already a category with this name '{model.Name}'");

                foreach (var s in model.SubCategories)
                {
                    if (await _categoryService.IsThereThisSubCategory(s.Name))
                        return BadRequest($"There is already a subcategory with this name '{s.Name}'");
                }
               
                var url = _linkGenerator.GetPathByAction(
                        HttpContext,
                        "Get",
                        values: new { name = model.Name });

                if (string.IsNullOrWhiteSpace(url))
                    return BadRequest("Could not use curret category");

                var category = await _categoryService.AddCategory(model);

                if (category != null)
                    return Created($"/api/categories/{category.Name}", model);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut("{nameCat}")]
        public async Task<ActionResult<CategoryModel>> Put(string nameCat, CategoryModel model)
        {
            try
            {
                if (await _categoryService.IsThereThisCategory(model.Name))
                    return BadRequest($"There is already a category with this name '{model.Name}'");

                foreach (var s in model.SubCategories ?? Array.Empty<SubCategoryModel>())
                {
                    if (await _categoryService.IsThereThisSubCategory(s.Name))
                        return BadRequest($"There is already a subcategory with this name '{s.Name}'");
                }

                if (!await _categoryService.IsThereThisCategory(nameCat))
                    return NotFound($"Could not find category with this name '{nameCat}'");

                return await _categoryService.UpdateCategory(nameCat, model);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{nameCat}")]
        public async Task<IActionResult> Delete(string nameCat)
        {
            try
            {
                var oldCategory = await _categoryService.GetCategoryEntityByName(nameCat);

                if (oldCategory == null)
                    return NotFound($"Could not find category with this name '{nameCat}'");

                if (await _categoryService.DeleteCategory(oldCategory))
                    return Ok();
                else
                    return BadRequest("Failed to delete category");

            }
            catch (DbUpdateException)
            {
                return BadRequest("You cannot remove the category because there is a reference in one or more sub categories");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
