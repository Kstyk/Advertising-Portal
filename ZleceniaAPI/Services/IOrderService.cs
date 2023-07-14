using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IOrderService
    {
        void AddNewOrder(AddOrderDto dto);
        void AddOffer(int orderId, AddOfferDto dto);
        OrderDto GetById(int id);
        PagedResult<OrderDto> GetAll(OrderQuery? query);
        PagedResult<OfferByContractorDto> GetAllOffersFromUser(OfferQuery? query);
        OfferDto? DeleteOffer(int offerId);
        Task CheckAndUpdateOrderStatus();
        void EditOffer(int offerId, AddOfferDto dto);
        List<OfferDto> GetAllOffersToOrder(int orderId);
    }
}