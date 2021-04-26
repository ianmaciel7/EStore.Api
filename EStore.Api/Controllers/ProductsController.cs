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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public ProductsController(
            IProductRepository productRepository, 
            IMapper mapper,LinkGenerator linkGenerator)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ProductModel[]>> Get()
        {
            try
            {
                var results = await productRepository.AllAsync();
                return mapper.Map<ProductModel[]>(results);
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
                var results = await productRepository.GetByNameAsync(name);

                if (results == null) return NotFound();

                return mapper.Map<ProductModel>(results);
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
                var existing = await productRepository.GetByNameAsync(model.Name);
                if (existing != null)
                {
                    return BadRequest("There is already a product with this name");
                }

                var location = linkGenerator.GetPathByAction("Get",
                    "Products",
                    new { name = model.Name }
                    );

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use curret product");
                }

                var product = mapper.Map<Product>(model);
                productRepository.Add(product);
                if (await productRepository.SaveChangesAsync())
                {
                    return Created($"/api/products/{product.Name}", mapper.Map<ProductModel>(product));
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