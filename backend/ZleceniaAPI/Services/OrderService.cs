using AutoMapper;
using Azure;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using MimeKit;
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
            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                throw new BadRequestException("Błędne dane użytkownika.");
            }

            var order = new Order();
            if (dto.AddressId == false)
            {
                order = _mapper.Map<Order>(dto);
            } else
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                order.AddressId = (int)user.AddressId;
                order.Title = dto.Title;
                order.Description = dto.Description;
                order.PublicationDays = dto.PublicationDays;
                order.AllowRemotely = dto.AllowRemotely;
                order.Budget = dto.Budget;
            }

            order.StartDate = DateTime.Now;

            var category = _dbContext.Categories.FirstOrDefault(category => category.Id == dto.CategoryId);

            if (category == null)
            {
                throw new NullReferenceException("Kategoria o ID " + dto.CategoryId + " nie została znaleziona.");
            }

            order.CategoryId = category.Id;

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
                throw new BadRequestException("Już złożyłeś ofertę do tego zlecenia.", HttpStatusCode.Conflict);
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

        public PagedResult<OrderDto> GetAll(OrderQuery query)
        {
            var baseQuery = _dbContext
                .Orders
                .Include(r => r.User)
                .Include(r => r.Address)
                .Include(r => r.Category)
                .Include(r => r.WinnerOffer)
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
                    { nameof(Order.StartDate), r => r.StartDate },
                    { nameof(Order.Title), r => r.Title },
                    { nameof(Order.Budget), r => r.Budget }
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
                    { nameof(Offer.PublicDate), r => r.PublicDate},
                {nameof(Offer.Price), r => r.Price }
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

        public void EditOrder(int orderId, EditOrderDto dto)
        {
            var userId = _userContextService.GetUserId;
            var order = _dbContext.Orders.FirstOrDefault(order => order.Id == orderId);

            if (order == null)
            {
                throw new BadRequestException("Nie ma zlecenia o takim ID.");
            }

            if(order.UserId != userId)
            {
                throw new BadRequestException("Nie możesz edytować tego zlecenia.", HttpStatusCode.Forbidden);
            }

            var editOrder = _mapper.Map<EditOrderDto, Order>(dto, order);

            var address = _dbContext.Addresses.FirstOrDefault(address => address.Id == order.AddressId);

            if(address != null) {
                address.Voivodeship = dto.Voivodeship;
                address.City = dto.City;
                address.Street = dto.Street;
                address.BuildingNumber = dto.BuildingNumber;
                address.PostalCode = dto.PostalCode;

                _dbContext.Addresses.Update(address);
            }


            _dbContext.Orders.Update(editOrder);
            _dbContext.SaveChanges();
        }

        public void EndOrder(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
            order.IsActive = false;

            _dbContext.Update(order);
            _dbContext.SaveChanges();
        }

        public PagedResult<OfferDto> GetAllOffersToOrder(int orderId, OfferQuery? query)
        {
            var userId = _userContextService.GetUserId;
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            
            if (order == null)
            {
                throw new BadRequestException("Nie ma zlecenia o tym Id.");
            }

            if(order.UserId != userId)
            {
                throw new UnauthorizedAccessException("To nie jest twoje zlecenie.");
            }


            var baseQuery = _dbContext.Offers
                .Include(u => u.User)
                .Where(r => r.OrderId == orderId);

            if (query.IsActive is not null)
            {
                baseQuery = baseQuery.Where(r => r.Order.IsActive == query.IsActive);
            }


            var columnsSelectors = new Dictionary<string, Expression<Func<Offer, object>>> {
                    { nameof(Offer.PublicDate), r => r.PublicDate},
                    { nameof(Offer.Price), r => r.Price }
                };

            var selectedColumn = columnsSelectors[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.ASC ?
                baseQuery.OrderBy(selectedColumn) :
                baseQuery.OrderByDescending(selectedColumn);

            var offers = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var offersDto = _mapper.Map<List<OfferDto>>(offers);

            var result = new PagedResult<OfferDto>(offersDto, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }

        public OfferDto SetWinnerOfferForOrder(int orderId, int offerId) {
            var userId = _userContextService.GetUserId;
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            var offer = _dbContext.Offers
                .Include(u => u.User)
                .FirstOrDefault(o => o.Id == offerId);

            if(order != null && order.UserId == userId && offer != null && offer.OrderId ==  orderId)
            {
                order.WinnerOfferId = offerId;
                offer.IsWinner = true;

                if(order.IsActive)
                {
                    order.IsActive = false;
                }
                
                _dbContext.Update(order);
                _dbContext.Update(offer);
                _dbContext.SaveChanges();

                var offerdto = _mapper.Map<OfferDto>(offer);

                SnedConfirmationToWinnerOfferOwner(order, offer);

                return offerdto;
            }

            return null;
        }

        public OfferDto GetWinnerOfferForOrder(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            if(order  == null)
            {
                throw new BadRequestException("Nie ma zlecenia o takim ID.");
            }

            var offer = _mapper.Map<OfferDto>(_dbContext.Offers
                .Include(u => u.User)
                .FirstOrDefault(o => o.Id == order.WinnerOfferId));

            if (offer != null)
            {
                var ifRated = _dbContext.Opinions.FirstOrDefault(o => o.OfferId == offer.Id);

                if (ifRated != null)
                {
                    offer.IsRated = true;
                }

                return offer;
            }

            return null;
        }

        public PagedResult<OrderDto> GetUserOrders(OrderQuery? query)
        {
            var userId = _userContextService.GetUserId;

            var baseQuery = _dbContext
                .Orders
                .Include(r => r.User)
                .Include(r => r.Address)
                .Include(r => r.Category)
                .Include(r => r.WinnerOffer)
                .Include(r => r.Offers)
                .Where(o => o.UserId == userId);

            if (query.IsActive != null)
            {
                baseQuery = baseQuery.Where(r => r.IsActive == query.IsActive);
            } 

            var columnsSelectors = new Dictionary<string, Expression<Func<Order, object>>> {
                    { nameof(Order.StartDate), r => r.StartDate },
                    {nameof(Order.Title), r => r.Title },
                    {nameof(Order.Budget), r => r.Budget }
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

        public void AddOpinion(int offerId, AddOpinionDto dto)
        {
            var userId = _userContextService.GetUserId;

            var offer = _dbContext.Offers.FirstOrDefault(o => o.Id == offerId);

            if(offer == null)
            {
                throw new BadRequestException("Nie istnieje oferta o takim ID.");
            }

            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == offer.OrderId);

            var opinionIfExists = _dbContext.Opinions.FirstOrDefault(o => o.OfferId == offerId);

            if(opinionIfExists != null)
            {
                throw new BadRequestException("Już oceniłeś tego wykonawcę za to zlecenie.");
            }

            if(order.IsActive)
            {
                throw new BadRequestException("Nie możesz jeszcze dodać opinii zlekonawcy - twoje zlecenie wciąż trwa.");
            }


            if(!(offer.IsWinner == true && order.WinnerOfferId == offerId))
            {
                throw new BadRequestException("Ta oferta nie została wybrana jako wygrana - nie możesz ocenić wykonawcy.");
            }

            if(order == null)
            {
                throw new BadRequestException("Zlecenie o takim ID nie istnieje.");
            }

            var opinion = _mapper.Map<Opinion>(dto);
            opinion.OrderId = order.Id;
            opinion.OfferId = offer.Id;
            opinion.PrincipalId = (int)userId;
            opinion.ContractorId = offer.UserId;
            opinion.FinalRate = (dto.WorkQuality + dto.Punctuality + dto.Communication + dto.MeetingTheConditions) / 4.00;
            opinion.CreatedAt = DateTime.Now;

            _dbContext.Opinions.Add(opinion);
            _dbContext.SaveChanges();
        }

        public List<OpinionDto> GetContractorOpinions(int contractorId)
        {
            var opinions = _dbContext.Opinions
                .Include(r => r.Order)
                .Include(r => r.Offer)
                .Where(r => r.ContractorId == contractorId);

            var opinionsDto = _mapper.Map<List<OpinionDto>>(opinions);

            return opinionsDto;
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

        public StatisticDto GetStatistics() {
            StatisticDto dto = new StatisticDto();

            List<Order> orders = _dbContext.Orders.ToList();
            var sum = 0.00;
            foreach(var order in orders)
            {
                if (order.Budget != null)
                {
                    sum = ((double)order.Budget) + sum;
                }
            }

            dto.TotalValueOfOrders = (decimal)sum;
            dto.AmountOfOrders = orders.Count;

            List<User> contractors = _dbContext.Users.Where(u => u.TypeOfAccount.Name == "Wykonawca").ToList();
            dto.AmountOfContractors = contractors.Count;

            List<Offer> offers = _dbContext.Offers.ToList();

            if (orders.Count == 0)
            {
                dto.AverageOffersForOneOrder = 0;
            }
            else
            {
                double avg = (double)offers.Count / (double)orders.Count;

                dto.AverageOffersForOneOrder = (int)Math.Ceiling(avg);
            }

            return dto;
        }

        private void SnedConfirmationToWinnerOfferOwner(Order order, Offer offer)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("zlecenia.azurewebsites.net", "zleceniainfo@gmail.com"));
            email.To.Add(new MailboxAddress(offer.User.Email, offer.User.Email));

            email.Subject = "Twoja oferta została wybrana jako zwycięska!";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<h1>Gratulacje!</h1><br/>Twoja oferta do zlecenia:" +
                $" <a href='https://zlecenia.azurewebsites.net/orders/order/{order.Id}'>{order.Title}</a> - została wybrana jako wygrana! <br/>" +
                $"Wejdź na <a href='https://zlecenia.azurewebsites.net'>zlecenia.azurewebsites.net</a> i przekonaj się sam.";

            email.Body = bodyBuilder.ToMessageBody();

            var smtpSettings = GetSmtpSettingsFromConfig();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(smtpSettings.SmtpServer, smtpSettings.SmtpPort, false);
                smtp.Authenticate(smtpSettings.Username, smtpSettings.Password);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        private SmtpSettings GetSmtpSettingsFromConfig()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("smtpsettings.json")
                .Build();

            var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            return smtpSettings;
        }
    }
}
