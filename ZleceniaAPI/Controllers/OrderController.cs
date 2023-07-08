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
            try
            {
                _orderService.AddNewOrder(dto);

                return Ok();
            } catch (NullReferenceException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost("{orderId}/add-offer")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult AddOfferToOrder([FromRoute] int orderId, [FromBody] AddOfferDto dto)
        {
            try
            {
                _orderService.AddOffer(orderId, dto);

                return Ok();
            } catch (NullReferenceException ex)
            {
                return NotFound(ex);
            } catch (BadRequestException ex)
            {
                return BadRequest(ex);
            }
            }

        [HttpGet("{orderId}")]
        public ActionResult<OrderDto> GetOrder([FromRoute] int orderId)
        {
            try
            {
                var order = _orderService.GetById(orderId);
                return Ok(order);
            } catch(NullReferenceException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrders([FromQuery] OrderQuery? query)
        {
            var ordersDtos = _orderService.GetAll(query);

            return Ok(ordersDtos);
        }

        [HttpGet("offers/all")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<IEnumerable<OfferByContractorDto>> GetAllOffersFromOneUser([FromQuery] OfferQuery? query)
        {
            var offersDtos = _orderService.GetAllOffersFromUser(query);

            return Ok(offersDtos);
        }

        [HttpDelete("delete-offer/{offerId}")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<OfferByContractorDto> DeleteOffer([FromRoute] int offerId)
        {
            var offer = _orderService.DeleteOffer(offerId);

            return Ok(offer);
        }


        [HttpPut("edit-offer/{offerId}")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<OfferDto> EditOffer([FromRoute] int offerId, [FromBody] AddOfferDto dto)
        {
            try
            {
                _orderService.EditOffer(offerId, dto);
            } catch (BadRequestException ex)
            {
                return NotFound(ex.Message);
            }
             
            return Ok();
        }

    }
}
