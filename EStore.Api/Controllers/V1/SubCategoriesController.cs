using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Services;
using EStore.API.Data.Entities;
using EStore.API.InputModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Controllers.V1
{
    [Route("api/v1/Categories/{categoryName}/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public SubCategoriesController(ICategoryService categoryService, LinkGenerator linkGenerator)
        {
            this._categoryService = categoryService;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromRoute] string categoryName)
        {

            try
            {
                var result = await _categoryService.GetAllSubCategoriesAsync(categoryName);

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }           
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{subCategoryId:int}")]
        public async Task<ActionResult> Get(
                        [FromRoute] string categoryName,
                        [FromRoute] int subCategoryId
                        )
        {

            try
            {
                var result = await _categoryService.GetSubCategoryAsync(categoryName,subCategoryId);
                return Ok(result);
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubCategory>> Post([FromRoute] string categoryName,
                                                          [FromBody] CategoryInputModel model)
        {
            try
            {
                var result = await _categoryService.AddSubCategoryAsync(categoryName, model);
                var uri = _linkGenerator.GetPathByAction("Get",
                    "SubCategories",
                    new { categoryName = categoryName, subCategoryId = result.SubCategoryId }
                    );
                return Created(uri, result);
            }
            catch (CategoryNameNotUniqueException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{subCategoryId:int}")]
        public async Task<ActionResult> Put([FromRoute] string categoryName,
                                            [FromRoute] int subCategoryId, 
                                            [FromBody] SubCategoryInputModel model)
        {
            try
            {
                var result = await _categoryService.UpdateSubCategoryAsync(categoryName,subCategoryId, model);
                return Ok(result);
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoryNameNotUniqueException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{subCategoryId:int}")]
        public async Task<ActionResult> Delete([FromRoute] string categoryName,
                                               [FromRoute] int subCategoryId)
        {
            try
            {
                await _categoryService.DeleteSubCategoryAsync(categoryName, subCategoryId);
                return Ok();
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoriesContainsProductsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
