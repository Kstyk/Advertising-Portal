using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly OferiaDbContext _dbContext;

        public CategoryService(IMapper mapper, OferiaDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public List<CategoryDto> GetMainCategories()
        {
            var mainCategories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.
                Where(category => category.ParentCategoryId == null));
            return mainCategories;
        }

        public List<CategoryDto> GetChildCategories(int mainCategoryId)
        {
            var childCategories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.Include(c => c.ParentCategory).
                Where(category => category.ParentCategoryId == mainCategoryId));

            return childCategories;
        }
    }
}
