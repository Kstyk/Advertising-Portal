using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class ContractorService : IContractorService
    {
        private OferiaDbContext _dbContext;
        private ICategoryService _categoryService;
        private IMapper _mapper;
        public ContractorService(IMapper mapper, OferiaDbContext dbContext, ICategoryService categoryService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public PagedResult<ContractorDto> GetAllContractors(ContractorQuery? query)
        {
            var baseQuery = _dbContext
                .Users
                .Include(r => r.Address)
                .Where(r => r.TypeOfAccount.Name.Equals("Wykonawca"));

            if (query.CategoryId is not null)
            {
                var userCategories = _dbContext.UsersCategories
                    .Where(r => r.CategoryId == query.CategoryId || r.Category.ParentCategoryId == query.CategoryId
                || r.Category.ParentCategory.ParentCategoryId == query.CategoryId);

                baseQuery = baseQuery.Where(user => userCategories.Any(uc => uc.UserId == user.Id));

            }

            if (query.Voivodeship is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Address.Voivodeship.Equals(query.Voivodeship));
            }

            if (query.City is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Address.City.Contains(query.City));
            }

            if (query.SearchText is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.FirstName.Contains(query.SearchText) || r.LastName.Contains(query.SearchText) || r.CompanyName.Contains(query.SearchText) || r.Description.Contains(query.SearchText));
            }

            var columnsSelectors = new Dictionary<string, Expression<Func<User, object>>> {
                    { nameof(User.LastName), r => r.LastName },
                    { nameof(User.FirstName), r => r.FirstName},
                    { nameof(User.CompanyName), r => r.CompanyName},
                };

            var selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                baseQuery.OrderBy(selectedColumn) :
                baseQuery.OrderByDescending(selectedColumn);

            var usersBeforePagination = _mapper.Map<List<ContractorDto>>(baseQuery);

            var users = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();


            var usersDto = _mapper.Map<List<ContractorDto>>(users);

            List<UserCategoryDto> listOfCategories = new List<UserCategoryDto>();

            foreach (var user in usersBeforePagination)
            {
                var list = _categoryService.GetCategoriesByUserId(user.Id);
                foreach(var category in list)
                {
                    if (!listOfCategories.Any(x => x.CategoryId == category.CategoryId))
                    {
                        listOfCategories.Add(category);
                    }
                }
            }

            listOfCategories = listOfCategories.OrderBy((cat) => cat.Name).ToList();

            var result = new PagedResult<ContractorDto>(usersDto, listOfCategories, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }
    }
}
