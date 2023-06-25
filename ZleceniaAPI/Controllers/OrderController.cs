using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add")]
        //[Authorize]
        [Authorize(Policy = "IsPrincipal")]
        public ActionResult AddOrder([FromBody] AddOrderDto dto)
        {
            _orderService.AddNewOrder(dto);

            return Ok();
        }

        [HttpPost("{orderId}/add-offer")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult AddOfferToOrder([FromRoute] int orderId, [FromBody] AddOfferDto dto)
        {
            _orderService.AddOffer(orderId, dto);

            return Ok();
        }

        [HttpGet("{orderId}")]
        public ActionResult<OrderDto> GetOrder([FromRoute] int orderId)
        {
            try
            {
                var order = _orderService.GetById(orderId);
                return Ok(order);
            } catch(BadRequestException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<OrderDto>> GetAllRestaurants([FromQuery] OrderQuery? query)
        {
            var ordersDtos = _orderService.GetAll(query);

            return Ok(ordersDtos);
        }
    }
}
