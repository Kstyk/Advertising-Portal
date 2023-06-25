using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Linq.Expressions;
using System.Net;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class OrderService : IOrderService
    {
        private IMapper _mapper;
        private OferiaDbContext _dbContext;
        private IUserContextService _userContextService;
        public OrderService(IMapper mapper, OferiaDbContext dbContext, IUserContextService userContextService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public void AddNewOrder(AddOrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            order.StartDate = DateTime.Now;

            var category = _dbContext.Categories.FirstOrDefault(category => category.Id == dto.CategoryId);

            if (category == null)
            {
                throw new BadHttpRequestException("Kategoria o ID " + category.Id + " nie znaleziona.");
            }

            var userId = _userContextService.GetUserId;
            order.UserId = (int)userId;

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void AddOffer(int orderId, AddOfferDto dto)
        {
            var offer = _mapper.Map<Offer>(dto);
            offer.PublicDate = DateTime.Now;

            var userId = _userContextService.GetUserId;
            offer.UserId = (int)userId;

            offer.OrderId = orderId;

            _dbContext.Offers.Add(offer);
            _dbContext.SaveChanges();
        }

        public OrderDto GetById(int id)
        {
            var order = _dbContext
                .Orders
                .Include(o => o.Address)
                .Include(o => o.Category)
                .Include(r => r.Offers).ThenInclude(o => o.User)
                .Include(r => r.User)
                .FirstOrDefault(o => o.Id == id);

            if(order == null)
            {
                throw new BadRequestException("Nie znaleziono zlecenia o ID " + id + ".");
            }

            var result = _mapper.Map<OrderDto>(order);

            return result;
        }

        public PagedResult<OrderDto> GetAll(OrderQuery? query)
        {
            var baseQuery = _dbContext
                .Orders
                .Include(r => r.User)
                .Include(r => r.Address)
                .Include(r => r.Category)
                .Where(r => query.CategoryId == null 
                || r.CategoryId == query.CategoryId 
                || r.Category.ParentCategoryId == query.CategoryId
                || r.Category.ParentCategory.ParentCategoryId == query.CategoryId
                );

            var columnsSelectors = new Dictionary<string, Expression<Func<Order, object>>> {
                    { nameof(Order.StartDate), r => r.StartDate }
                };

            var selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                baseQuery.OrderBy(selectedColumn) :
                baseQuery.OrderByDescending(selectedColumn);

            var orders = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);

            var result = new PagedResult<OrderDto>(ordersDto, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }
    }
}
