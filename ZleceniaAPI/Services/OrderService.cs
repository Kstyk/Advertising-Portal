using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using ZleceniaAPI.Entities;
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

            var category = _dbContext.Categories.FirstOrDefault(category => category.Id == dto.CategoryId);

            if (category == null)
            {
                throw new BadRequestException("Kategoria o ID " + dto.CategoryId + " nie istnieje.", "categoryId");
            }

            var userId = _userContextService.GetUserId;
            order.UserId = (int)userId;

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }
    }
}
