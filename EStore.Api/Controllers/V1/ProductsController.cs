using EStore.Api.Exceptions;
using EStore.Api.InputModel;
using EStore.Api.Services;
using EStore.Api.ViewModel;
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
    [Route("api/v1/Categories/{categoryName}/SubCategories/{subCategoryName}/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        
        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public ProductsController(ICategoryService categoryService, LinkGenerator linkGenerator)
        {           
            this._categoryService = categoryService;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(
                        [FromRoute] string categoryName,
                        [FromRoute] string subCategoryName,
                        [FromQuery, Range(1,int.MaxValue)] int page = 1, 
                        [FromQuery, Range(1, 50)] int quantity = 5)
        {
            try
            {
                var result = await _categoryService.GetAllProductAsync(categoryName, subCategoryName,page,quantity);
                
                if (!result.Any())
                    return NoContent();

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

        [HttpGet("{productId:int}")]
        public async Task<ActionResult> Get(
                        [FromRoute] string categoryName,
                        [FromRoute] string subCategoryName,
                        [FromRoute] int productId)
        {
            try
            {
                var result = await _categoryService.GetProduct(categoryName, subCategoryName,productId);
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
            catch (ProductNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Post(
            [FromRoute] string categoryName,
            [FromRoute] string subCategoryName, 
            [FromBody] ProductInputModel model)
        {
            try
            {
                var result = await _categoryService.AddProductAsync(categoryName, subCategoryName, model);
                var uri = _linkGenerator.GetPathByAction("Get",
                    "Products",
                    new { categoryName = categoryName, subCategoryName = subCategoryName, productId = result.ProductId }
                    );
                return Created(uri, result);
            }
            catch (CategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SubCategoryNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ProductNameNotUniqueException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }           
        }

        [HttpPut("{productId:int}")]
        public async Task<ActionResult<ProductViewModel>> Put(
            [FromRoute] string categoryName,
            [FromRoute] string subCategoryName,
            [FromRoute] int productId,
            [FromBody] ProductInputModel model)
        {
            try
            {
                var result = await _categoryService.UpdateProductAsync(categoryName, subCategoryName,productId,model);
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
            catch (ProductNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ProductNameNotUniqueException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult> Delete(
            [FromRoute] string categoryName,
            [FromRoute] string subCategoryName,
            [FromRoute] int productId)
        {
            try
            {
                await _categoryService.DeleteProductAsync(categoryName, subCategoryName, productId);
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
            catch (ProductNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }            
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
