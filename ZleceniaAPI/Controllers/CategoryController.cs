using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Exceptions;
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

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategoryById([FromRoute] int id)
        {
            var category = _categoryService.GetCategoryById(id);

            return Ok(category);
        }

        [HttpGet("{parentCategoryId}/childCategories")]
        public ActionResult<List<CategoryDto>> GetChildCategories([FromRoute] int parentCategoryId)
        {
            var categories = _categoryService.GetChildCategories(parentCategoryId);

            return Ok(categories);
        }

        [HttpGet("allCategories")]
        public ActionResult<List<CategoryDto>> GetAllCategories()
        {
            var categories = _categoryService.GetAllCategories();

            return Ok(categories);
        }


        [HttpPost("user/add")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult AddUserCategories([FromBody] CreateUserCategoryDto dto)
        {
            try
            {
                _categoryService.AddUserCategories(dto);

                return Ok();
            } catch(BadRequestException ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpGet("userCategories")]
        public ActionResult<List<UserCategoryDto>> GetCategoriesByContractor()
        {
            var categories = _categoryService.GetCategoriesByContractor();

            return Ok(categories);
        }

        [HttpDelete("delete/{userCategoryId}")]
        [Authorize]
        public ActionResult<UserCategoryDto> DeleteUserCategory([FromRoute] int userCategoryId) { 
            var cat = _categoryService.DeleteUserCategory(userCategoryId);
        
            return Ok(cat);
        }

    }
}
