using AutoMapper;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryModel[]>> Get(bool includeSubCategories = false)
        {
            try
            {
                var results = await categoryRepository.AllAsync(includeSubCategories);
                return mapper.Map<CategoryModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }               
    }
}
