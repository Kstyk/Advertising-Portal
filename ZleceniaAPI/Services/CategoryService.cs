using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Migrations;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly OferiaDbContext _dbContext;
        private IUserContextService _userContextService;


        public CategoryService(IMapper mapper, OferiaDbContext dbContext, IUserContextService userContextService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public List<CategoryDto> GetMainCategories()
        {
            var mainCategories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.
                Where(category => category.ParentCategoryId == null));
            return mainCategories;
        }

        public List<CategoryDto> GetChildCategories(int mainCategoryId)
        {
            var childCategories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.Include(c => c.ParentCategory).ThenInclude(c2 => c2.ParentCategory).
                Where(category => category.ParentCategoryId == mainCategoryId));

            return childCategories;
        }

        public List<CategoryDto> GetAllCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.Include(c => c.ParentCategory)
                .ThenInclude(c2 => c2.ParentCategory));

            return categories;
        }

        public void AddUserCategories(CreateUserCategoryDto dto) {
            var categories = _dbContext.Categories
                .Where(c => dto.Categories.Contains(c.Id))
                .ToList();


            if (categories.Count == 0)
            {
                throw new BadRequestException("Nie znaleziono żadnych kategorii o podanych identyfikatorach.");
            }

            var userId = _userContextService.GetUserId;

            var existingUserCategories = _dbContext.UsersCategories
                .Where(uc => uc.UserId == userId && dto.Categories.Contains(uc.CategoryId))
                .ToList();

            if (existingUserCategories.Count > 0)
            {
                throw new BadRequestException("Użytkownik już posiada jedną lub więcej z tych kategorii.");
            }


            var userCategories = categories.Select(c => new UsersCategories
            {
                UserId = (int)userId,
                CategoryId = c.Id,
                IsMainCategory = false
            }).ToList();


            var actualUserCategories = _dbContext.UsersCategories
                .Where(uc => uc.UserId == userId).ToList();

            if (actualUserCategories.Count == 0)
            {
                userCategories.First().IsMainCategory = true;
                // Tworzenie modelu zakresu lokalizacji pracy
                AreaOfWork areaOfWork = _mapper.Map<CreateUserCategoryDto, AreaOfWork>(dto);
                areaOfWork.UserId = (int)userId;

                // Zapisz do bazy danych
                _dbContext.AreaOfWorks.Add(areaOfWork);
            }

            _dbContext.UsersCategories.AddRange(userCategories);
            _dbContext.SaveChanges();
        }

         public CategoryDto GetCategoryById(int id)
        {
            var category = _mapper.Map<CategoryDto>(_dbContext.Categories
                .Include(c => c.ParentCategory)
                .ThenInclude(c2 => c2.ParentCategory)
                .FirstOrDefault(c => c.Id == id));

            return category;
        }

        public List<UserCategoryDto> GetCategoriesByContractor()
        {
            var userId = _userContextService.GetUserId;


            var categories = _mapper.Map<List<UserCategoryDto>>(_dbContext.UsersCategories
                .Include(c => c.Category)
                .ThenInclude(p => p.ParentCategory)
                .ThenInclude(p2 => p2.ParentCategory)
                .Where(e => e.UserId == userId).ToList());

            return categories;
        }

        public UserCategoryDto DeleteUserCategory(int userCategoryId)
        {
            var userId = _userContextService.GetUserId;
            
            var category = _dbContext.UsersCategories
                .FirstOrDefault(cat => cat.UserId ==  userId && cat.Id == userCategoryId);

            if(category != null)
            {
                _dbContext.UsersCategories.Remove(category);
                _dbContext.SaveChanges();
                return _mapper.Map<UserCategoryDto>(category);
            }
            return null;
        }

    }
}
