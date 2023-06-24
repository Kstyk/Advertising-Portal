using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            order.StartDate = DateTime.Now;

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
                throw new BadRequestException("Zlecenie nie znalezione.");
            }

            var result = _mapper.Map<OrderDto>(order);

            return result;
        }
    }
}
