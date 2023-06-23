using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IOrderService
    {
        void AddNewOrder(AddOrderDto dto);
    }
}