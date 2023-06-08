using ZleceniaAPI.Entities;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface ICategoryService
    {
        List<CategoryDto> GetMainCategories();
        List<CategoryDto> GetChildCategories(int mainCategoryId);

        void AddUserCategories(CreateUserCategoryDto dto);
    }
}