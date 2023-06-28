using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IOrderService
    {
        void AddNewOrder(AddOrderDto dto);
        void AddOffer(int orderId, AddOfferDto dto);
        OrderDto GetById(int id);
        PagedResult<OrderDto> GetAll(OrderQuery? query);
        Task CheckAndUpdateOrderStatus();
    }
}