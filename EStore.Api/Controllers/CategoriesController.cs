using AutoMapper;
using EStore.API.Data;
using EStore.API.Data.Entities;
using EStore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.API.Controllers
{
    [ApiVersion("1.1")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(bool includeSubCategories = false)
        {
            try
            {
                var results = await _categoryRepository.AllCategoriesAsync(includeSubCategories);
                var result = new
                {
                    Count = results.Count(),
                    Results = _mapper.Map<CategoryModel[]>(results)
                };

                return Ok(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost()]
        public async Task<ActionResult<CategoryModel>> Post(CategoryModel model)
        {
            try
            {
                var existing = await _categoryRepository.GetCategoryByNameAsync(model.Name);
                if (existing != null) return BadRequest("There is already a product with this name");

                var category = _mapper.Map<Category>(model);

                _categoryRepository.AddCategory(category);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(
                        HttpContext,
                        "Get",
                        values: new { name = category.Name });

                    return Created(url, _mapper.Map<CategoryModel>(category));
                }
                else
                {
                    return BadRequest("Failed to save new category");
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{name}")]
        public async Task<ActionResult<CategoryModel>> Put(string name,CategoryModel model)
        {
            try
            {
                var existing = await _categoryRepository.GetCategoryByNameAsync(model.Name);
                if (existing != null) return BadRequest("There is already a product with this name");

                var oldCategory = await _categoryRepository.GetCategoryByNameAsync(name);
                if (oldCategory == null)
                    return NotFound($"Could not find category with this name '{name}'");
                _mapper.Map(model, oldCategory);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CategoryModel>(oldCategory);
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
                var cat = await _categoryRepository.GetCategoryByNameAsync(name);
                if (cat == null) return NotFound("Couldn't find the category");
                _categoryRepository.DeleteCategory(cat);

                if (await _categoryRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to delete category");
                }

            }
            catch (DbUpdateException)
            {
                return BadRequest("You cannot remove the category because there is a reference in one or more sub categories");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
