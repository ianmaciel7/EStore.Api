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
    [Route("api/Categories/{nameCat}/[controller]")]
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

        [HttpGet("{nameSub}")]
        public async Task<ActionResult<SubCategoryModel>> Get(string nameCat,string nameSub)
        {
            try
            {
                if (!await _categoryService.IsThereThisCategory(nameCat))
                    return BadRequest($"Could not find category with this name '{nameCat}'");
                
                if (!await _categoryService.IsThereThisSubCategory(nameCat, nameSub))
                    return BadRequest($"Could not find subcategory with this name '{nameSub}' in category '{nameCat}'");

                return await _categoryService.GetSubCategoryByNameCategoryAndNameSubCategory(nameCat, nameSub);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubCategoryModel>> Post(string nameCat, SubCategoryModel model)
        {
            try
            {

                if (!await _categoryService.IsThereThisCategory(nameCat))
                    return BadRequest($"Could not find category with this name '{nameCat}'");

                if (await _categoryService.IsThereThisSubCategory(model.Name))
                    return BadRequest($"There is already a subcategory with this name '{model.Name}'");
       
                var sub = await _categoryService.AddSubCategory(model);

                var url = _linkGenerator.GetPathByAction(
                        HttpContext,
                        "Get",
                        values: new { nameCat, nameSub = sub.Name });

                if (string.IsNullOrWhiteSpace(url))
                    return BadRequest("Could not use curret category");

                return Created(url, sub);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{idSub:int}")]
        public async Task<ActionResult<SubCategoryModel>> Put(int idSub, SubCategoryModel model)
        {
            try
            {

                if (!await _categoryService.IsThereThisSubCategory(model.Name))
                    return BadRequest("There is already a subcategory with this name");

                if (!await _categoryService.IsThereThisSubCategory(idSub))
                    return NotFound($"Couldn't find the subcategory with this id '{idSub}'");

                if (!await _categoryService.IsThereThisSubCategory(model.Name, idSub))
                    return BadRequest($"Could not find subcategory with this id '{idSub}' in category '{model.Name}'");

                return await _categoryService.UpdateSubCategory(idSub, model);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpDelete("{idSub:int}")]
        public async Task<ActionResult<SubCategoryModel>> Delete(string nameCat, int idSub)
        {
            try
            {                
               
                if (!await _categoryService.IsThereThisCategory(nameCat))               
                    return BadRequest($"Could not find category with this name '{nameCat}'");

                var sub = await _categoryService.GetSubCategoryEntityByIdSubCategoryAndNameCategory(nameCat, idSub);

                if (sub == null)
                    return NotFound($"Could not find subcategory with this id '{idSub}' in category '{nameCat}'");
                             
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
