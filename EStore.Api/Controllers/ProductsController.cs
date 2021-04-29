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

namespace EStore.API.Controllers
{
    [ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ProductsController(
            IProductRepository productRepository,
            IMapper mapper,LinkGenerator linkGenerator)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> Get()
        {
            try
            {
                var results = await _productRepository.AllProductsAsync();
                return _mapper.Map<ProductModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<ProductModel>> Get(string name)
        {
            try
            {
                var results = await _productRepository.GetProductByNameAsync(name);

                if (results == null) return NotFound();

                return _mapper.Map<ProductModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost()]
        public async Task<ActionResult<ProductModel>> Post(ProductModel model)
        {
            try
            {
                var existing = await _productRepository.GetProductByNameAsync(model.Name);
                if (existing != null)
                {
                    return BadRequest("There is already a product with this name");
                }

                var location = _linkGenerator.GetPathByAction("Get",
                    "Products",
                    new { name = model.Name }
                    );

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use curret product");
                }

                var product = _mapper.Map<Product>(model);
                _productRepository.AddProducts(product);
                if (await _productRepository.SaveChangesAsync())
                {
                    return Created($"/api/products/{product.Name}", _mapper.Map<ProductModel>(product));
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<ProductModel>> Put(string name, ProductModel model)
        {
            try
            {
                var oldProduct = await _productRepository.GetProductByNameAsync(name);
                if (oldProduct == null)
                    return NotFound($"Could not find product with this name '{name}'");
                _mapper.Map(model, oldProduct);

                if (await _productRepository.SaveChangesAsync())
                {
                    return _mapper.Map<ProductModel>(oldProduct);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                var oldProduct = await _productRepository.GetProductByNameAsync(name);
                if (oldProduct == null)
                    return NotFound($"Could not find product with this name '{name}'");
                _productRepository.DeleteProduct(oldProduct);

                if (await _productRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
    }
}