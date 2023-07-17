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
        PagedResult<OfferDto> GetAllOffersToOrder(int orderId, OfferQuery? query);
        OfferDto SetWinnerOfferForOrder(int orderId, int offerId);
        OfferDto GetWinnerOfferForOrder(int orderId);
        PagedResult<OrderDto> GetUserOrders(OrderQuery? query);
        void EditOrder(int orderId, EditOrderDto dto);
    }
}