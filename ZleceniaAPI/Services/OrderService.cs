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
        private readonly IServiceScopeFactory _scopeFactory;
        private IUserContextService _userContextService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IMapper mapper, OferiaDbContext dbContext, IServiceScopeFactory scopeFactory, IUserContextService userContextService, ILogger<OrderService> logger)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _userContextService = userContextService;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public void AddNewOrder(AddOrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);
            order.StartDate = DateTime.Now;

            var category = _dbContext.Categories.FirstOrDefault(category => category.Id == dto.CategoryId);

            if (category == null)
            {
                throw new NullReferenceException("Kategoria o ID " + dto.CategoryId + " nie została znaleziona.");
            }

            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new BadRequestException("Błędne dane użytkownika.");
            }

            order.UserId = (int)userId;

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void AddOffer(int orderId, AddOfferDto dto)
        {
            var offer = _mapper.Map<Offer>(dto);
            offer.PublicDate = DateTime.Now;

            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new BadRequestException("Błędne dane użytkownika.");
            }

            offer.UserId = (int)userId;

            var order = _dbContext
                .Orders
                .Include(o => o.Offers)
                .FirstOrDefault(order => order.Id == orderId);

            if (order == null)
            {
                throw new NullReferenceException("Zlecenie o ID " + orderId + " nie zostało znalezione.");
            }

            if (!order.IsActive)
            {
                throw new BadRequestException("To zlecenie już nie jest dostępne.");
            }

            if (order.Offers.Any(of => of.UserId == userId)) {
                throw new BadRequestException("Już złożyłeś ofertę do tego zlecenia.");
            }

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
                throw new NullReferenceException("Nie znaleziono zlecenia o ID " + id + ".");
            }

            var result = _mapper.Map<OrderDto>(order);

            return result;
        }

        public OfferDto? DeleteOffer(int offerId)
        {
            var userId = _userContextService.GetUserId;

            var offer = _dbContext.Offers
                .FirstOrDefault(of => of.UserId == userId && of.Id == offerId);

            if (offer != null)
            {
                _dbContext.Offers.Remove(offer);
                _dbContext.SaveChanges();
                return _mapper.Map<OfferDto>(offer);
            }
            return null;
        }

        public PagedResult<OrderDto> GetAll(OrderQuery? query)
        {
            var baseQuery = _dbContext
                .Orders
                .Include(r => r.User)
                .Include(r => r.Address)
                .Include(r => r.Category)
                .Include(r => r.Offers)
                .Where(r => r.IsActive == query.IsActive);

            if(query.CategoryId is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.CategoryId == query.CategoryId
                || r.Category.ParentCategoryId == query.CategoryId
                || r.Category.ParentCategory.ParentCategoryId == query.CategoryId);
            }

            if(query.Voivodeship is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Address.Voivodeship == query.Voivodeship);
            }

            if(query.City is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Address.City.Contains(query.City));
            }

            if(query.SearchText is not null)
            {
                baseQuery = baseQuery
                    .Where(r => r.Title.Contains(query.SearchText) || r.Description.Contains(query.SearchText));
            }

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

        public PagedResult<OfferByContractorDto> GetAllOffersFromUser(OfferQuery? query)
        {
            var userId = _userContextService.GetUserId;

            var baseQuery = _dbContext.Offers
                .Include(o => o.Order)
                .Include(c => c.Order.Category)
                .Where(r => r.UserId == userId);

            if(query.IsActive is not null)
            {
                baseQuery = baseQuery.Where(r => r.Order.IsActive == query.IsActive);
            }

            var columnsSelectors = new Dictionary<string, Expression<Func<Offer, object>>> {
                    { nameof(Offer.PublicDate), r => r.PublicDate}
                };

            var selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                baseQuery.OrderBy(selectedColumn) :
                baseQuery.OrderByDescending(selectedColumn);

            var offers = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var offersDto = _mapper.Map<List<OfferByContractorDto>>(offers);

            var result = new PagedResult<OfferByContractorDto>(offersDto, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }

        public void EditOffer(int offerId, AddOfferDto dto)
        {
            var offer = _dbContext.Offers.FirstOrDefault(offer => offer.Id == offerId);

            if (offer == null)
            {
                throw new BadRequestException("Nie ma oferty o takim ID.");
            }

            var editOffer = _mapper.Map<AddOfferDto, Offer>(dto, offer);
            editOffer.PublicDate = DateTime.Now;

            _dbContext.Offers.Update(editOffer);
            _dbContext.SaveChanges();
        }

            public async Task CheckAndUpdateOrderStatus()
        {
            var currentDate = DateTime.UtcNow.Date;

            var ordersToCheck = _dbContext.Orders
                .Where(o => o.IsActive && o.StartDate.AddDays(o.PublicationDays) <= currentDate)
                .ToList();

            foreach (var order in ordersToCheck)
            {
                order.IsActive = false;
            }


            await _dbContext.SaveChangesAsync();
        }
    }
}
