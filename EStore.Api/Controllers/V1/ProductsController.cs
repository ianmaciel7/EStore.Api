using EStore.Api.Exceptions;
using EStore.Api.Services;
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
    [Route("api/v1/[controller]")]
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

        [HttpGet("Categories/{categoryName}/SubCategories/{subCategoryName}/[controller]")]
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
    }
}
