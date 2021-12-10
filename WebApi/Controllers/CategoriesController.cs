using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{categoryId}")]
        public IActionResult Get(Guid categoryId)
        {
            var result = _categoryService.Get(categoryId);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet]
        public IActionResult GetAll(string searchTerm = "")
        {
            var result = _categoryService.GetAll(searchTerm);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add(CategoryCreateViewModel newCategory)
        {
            var result = _categoryService.Add(newCategory);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPut("{categoryId}")]
        public IActionResult Put(Guid categoryId, CategoryCreateViewModel category)
        {
            var result = _categoryService.Update(categoryId, category);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpDelete("{categoryId}")]
        public IActionResult Delete(Guid categoryId)
        {
            var result = _categoryService.Delete(categoryId);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
