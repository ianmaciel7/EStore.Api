using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Services;
using EStore.API.InputModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public CategoriesController(ICategoryService categoryService, LinkGenerator linkGenerator)
        {
            this._categoryService = categoryService;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {

            try
            {
                var result = await _categoryService.GetAllCategoriesAsync();

                if (!result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{categoryId:int}")]
        public async Task<ActionResult> Get([FromRoute] int categoryId)
        {

            try
            {
                var result = await _categoryService.GetCategoryAsync(categoryId);
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryInputModel model)
        {

            try
            {
                var result = await _categoryService.AddCategoryAsync(model);
                var uri = _linkGenerator.GetPathByAction("Get",
                    "Categories",
                    new { categoryId = result.CategoryId }
                    );
                return Created(uri, result);
            }
            catch (CategoryNameNotUniqueException ex)
            {
                return BadRequest(ex.Message);
            }            
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{categoryId:int}")]
        public async Task<ActionResult> Put([FromRoute] int categoryId, [FromBody] CategoryInputModel model)
        {
            try
            {
                var result = await _categoryService.UpdateCategoryAsync(categoryId,model);
                return Ok(result);
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpDelete("{categoryId:int}")]
        public async Task<ActionResult> Delete([FromRoute] int categoryId)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(categoryId);
                return Ok();
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CategoryContainsSubCategoriesException ex)
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
