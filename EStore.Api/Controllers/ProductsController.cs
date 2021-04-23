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

namespace EStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
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
    }
}