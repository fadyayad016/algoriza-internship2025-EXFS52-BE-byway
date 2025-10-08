using Byway.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Byway.Api.New.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly Byway.Application.Interfaces.ICategoryService _categoryService;

        public CategoryController(Byway.Application.Interfaces.ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateAsync([FromBody] CreateOrUpdateCategoryDto dto)
        {
            var newCategory = await _categoryService.CreateAsync(dto);
            return Ok(newCategory);
        }








    }
}
