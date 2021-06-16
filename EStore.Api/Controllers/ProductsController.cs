using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EStore.API.Data;
using AutoMapper;
using EStore.API.Models;
using Microsoft.AspNetCore.Routing;
using EStore.API.Services;
using System.Diagnostics;

namespace EStore.API.Controllers
{
    [ApiVersion("1.1")]
    [Route("api/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly LinkGenerator _linkGenerator;

        public ProductsController(IProductService productService,ICategoryService categoryService, LinkGenerator linkGenerator)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet("Categories/{nameCat}/SubCategories/{nameSub}/[controller]")]
        public async Task<ActionResult> Get(string nameCat, string nameSub)
        {
            try
            {

                if (!await _categoryService.IsThereThisCategory(nameCat))
                    return BadRequest($"Could not find category with this name '{nameCat}'");

                if (!await _categoryService.IsThereThisSubCategory(nameCat,nameSub))
                    return BadRequest($"Could not find subcategory with this name '{nameSub}' in category '{nameCat}'");

                var result = await _productService.GetProductByNameCategoryAndNameSubCategory(nameCat, nameSub);

                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("Categories/{nameCat}/SubCategories/{nameSub}/[controller]/{idProd:int}")]
        public async Task<ActionResult<ProductModel>> Get(string nameCat, string nameSub,int idProd)
        {
            try
            {
                if (!await _categoryService.IsThereThisCategory(nameCat))
                    return BadRequest($"Could not find category with this name '{nameCat}'");

                if (!await _categoryService.IsThereThisSubCategory(nameCat, nameSub))
                    return BadRequest($"Could not find subcategory with this name '{nameSub}' in category '{nameCat}'");
       
                var result = await _productService.GetProductByNameCategoryAndNameSubCategoryAndIdProduct(nameCat,nameSub, idProd);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("[controller]/{idProd:int}")]
        public async Task<ActionResult<ProductModel>> Get(int idProd)
        {
            try
            {

                var result = await _productService.GetProductById(idProd);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost("[controller]")]
        public async Task<ActionResult<ProductModel>> Post(ProductModel model)
        {
            try
            {
               
                if (await _productService.IsThereThisProduct(model.Name)) 
                    return BadRequest($"There is already a product with this name '{model.Name}'");

                if (!await _categoryService.IsThereThisCategory(model.CategoryName))
                    return BadRequest($"Couldn't find the category with this name '{model.CategoryName}'");

                if (!await _categoryService.IsThereThisSubCategory(model.SubCategoryName))
                    return BadRequest($"Couldn't find the subcategory with this name '{model.SubCategoryName}'");

                var location = _linkGenerator.GetPathByAction("Get",
                    "Products",
                    new { nameCat = model.CategoryName, nameSub = model.SubCategoryName, idProd = model.ProductId }
                    );

                if (string.IsNullOrWhiteSpace(location))               
                    return BadRequest("Could not use curret product");
                
                var product = await _productService.AddProduct(model);

                if (product != null) 
                    return Created($"/api/products/{product.Name}", model);
                
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut("[controller]/{idProd:int}")]
        public async Task<ActionResult<ProductModel>> Put(int idProd, ProductModel model)
        {
            try
            {
                
                if (await _productService.IsThereThisProduct(model.Name)) 
                    return BadRequest($"There is already a product with this name '{model.Name}'");

                if (!await _productService.IsThereThisProduct(idProd))
                    return NotFound($"Could not find product with this id '{idProd}'");

                if (!await _categoryService.IsThereThisCategory(model.CategoryName))
                    return BadRequest($"Couldn't find the category with this name '{model.CategoryName}'");

                if (!await _categoryService.IsThereThisSubCategory(model.SubCategoryName))
                    return BadRequest($"Couldn't find the subcategory with this name '{model.SubCategoryName}'");

                return await _productService.UpdateProduct(idProd, model);                           
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("[controller]/{idProd:int}")]
        public async Task<IActionResult> Delete(int idProd)
        {
            try
            {
                var oldProduct = await _productService.GetProductEntityById(idProd);

                if (oldProduct == null)                
                    return NotFound($"Could not find product with this id '{idProd}'");
                    
                if (await _productService.DeleteProduct(oldProduct))
                    return Ok();
                else
                    return BadRequest("Failed to delete product");
            }
            catch (Exception)
            {               
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}