using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Models;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("main")]
        public ActionResult<List<CategoryDto>> GetMainCategories()
        {
            var categories = _categoryService.GetMainCategories();

            return Ok(categories);
        }

        [HttpGet("{parentCategoryId}/childCategories")]
        public ActionResult<List<CategoryDto>> GetChildCategories([FromRoute] int parentCategoryId)
        {
            var categories = _categoryService.GetChildCategories(parentCategoryId);

            return Ok(categories);
        }

        [HttpPost("user/add")]
        public ActionResult AddUserCategories([FromBody] CreateUserCategoryDto dto)
        {
            _categoryService.AddUserCategories(dto);

            return Ok();
        }

    }
}
