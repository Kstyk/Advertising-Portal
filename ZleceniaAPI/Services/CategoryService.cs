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
            var childCategories = _mapper.Map<List<CategoryDto>>(_dbContext.Categories.Include(c => c.ParentCategory).
                Where(category => category.ParentCategoryId == mainCategoryId));

            return childCategories;
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

            userCategories.First().IsMainCategory = true;

            // Zapisz do bazy danych
            _dbContext.UsersCategories.AddRange(userCategories);
            _dbContext.SaveChanges();
        }
    }
}
