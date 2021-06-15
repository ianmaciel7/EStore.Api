using AutoMapper;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using EStore.API.Services;
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
    [ApiVersion("1.1")]
    [Route("api/Categories/{name}/SubCategories")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public SubCategoriesController(
            ICategoryService categoryService,
            LinkGenerator linkGenerator)
        {
            _categoryService = categoryService;            
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubCategoryModel>> Get(string name,int id)
        {
            try
            {
                if (!await _categoryService.IsThereThisCategory(name))
                    return BadRequest($"Could not find category with this name '{name}'");
                
                if (!await _categoryService.IsThereThisSubCategory(name,id))
                    return BadRequest($"Could not find subcategory with this id '{id}' in category '{name}'");

                return await _categoryService.GetSubCategoryByNameCategoryAndIdSubCategory(name, id);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubCategoryModel>> Post(string name, SubCategoryModel model)
        {
            try
            {

                if (!await _categoryService.IsThereThisCategory(name))
                    return BadRequest($"Could not find category with this name '{name}'");

                if (await _categoryService.IsThereThisSubCategory(model.Name))
                    return BadRequest($"There is already a subcategory with this name '{model.Name}'");
       
                var sub = await _categoryService.AddSubCategory(model);

                var url = _linkGenerator.GetPathByAction(
                        HttpContext,
                        "Get",
                        values: new { name, id = sub.SubCategoryId });

                if (string.IsNullOrWhiteSpace(url))
                    return BadRequest("Could not use curret category");

                return Created(url, sub);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubCategoryModel>> Put(int id,SubCategoryModel model)
        {
            try
            {

                if (!await _categoryService.IsThereThisSubCategory(model.Name))
                    return BadRequest("There is already a subcategory with this name");

                if (!await _categoryService.IsThereThisSubCategory(id))
                    return NotFound($"Couldn't find the subcategory with this id '{id}'");

                if (!await _categoryService.IsThereThisSubCategory(model.Name, id))
                    return BadRequest($"Could not find subcategory with this id '{id}' in category '{model.Name}'");

                return await _categoryService.UpdateSubCategory(id,model);
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
               
                if (!await _categoryService.IsThereThisCategory(name))               
                    return BadRequest($"Could not find category with this name '{name}'");

                var sub = await _categoryService.GetSubCategoryEntityByIdSubCategoryAndNameCategory(name, id);

                if (sub == null)
                    return NotFound($"Could not find subcategory with this id '{id}' in category '{name}'");
                             
                if (await _categoryService.DeleteSubCategory(sub))
                    return Ok();
                else
                    return BadRequest("Failed to delete category");
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
