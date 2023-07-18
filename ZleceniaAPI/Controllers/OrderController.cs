using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        [HttpPut("{orderId}/edit")]
        [Authorize(Policy ="IsPrincipal")]
        public ActionResult EditOrder([FromBody] EditOrderDto dto, [FromRoute] int orderId) {
            try
            {
                _orderService.EditOrder(orderId, dto);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ObjectResult(ex.Message)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                return BadRequest(ex);
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

        [HttpGet("logged-user/orders")]
        [Authorize(Policy = "IsPrincipal")]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrdersFromUser([FromQuery] OrderQuery? query)
        {
            var ordersDtos = _orderService.GetUserOrders(query);

            return Ok(ordersDtos);
        }

        [HttpGet("{orderId}/offers")]
        [Authorize(Policy = "IsPrincipal")]
        public ActionResult<IEnumerable<OfferDto>> GetAllOffersToOrder([FromRoute] int orderId, [FromQuery] OfferQuery? query)
        {
            try
            {
                var offersDtos = _orderService.GetAllOffersToOrder(orderId, query);

                return Ok(offersDtos);
            } catch(BadRequestException ex)
            {
                return NotFound(ex);
            } catch(UnauthorizedAccessException ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
            }

        [HttpDelete("delete-offer/{offerId}")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<OfferByContractorDto> DeleteOffer([FromRoute] int offerId)
        {
            var offer = _orderService.DeleteOffer(offerId);

            return Ok(offer);
        }

        [HttpGet("offers/all")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<IEnumerable<OfferByContractorDto>> GetAllOffersFromOneUser([FromQuery] OfferQuery? query)
        {
            var offersDtos = _orderService.GetAllOffersFromUser(query);

            return Ok(offersDtos);
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

        [HttpPut("{orderId}/set-winner/{offerId}")]
        [Authorize(Policy = "IsPrincipal")]
        public ActionResult<OfferDto> SetWinner([FromRoute] int orderId, [FromRoute] int offerId)
        {
            try
            {
                var dto = _orderService.SetWinnerOfferForOrder(orderId, offerId);

                return Ok(dto);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{orderId}/get-winner")]
        public ActionResult<OfferDto> GetWinner([FromRoute] int orderId)
        {
            try
            {
                var dto = _orderService.GetWinnerOfferForOrder(orderId);
                return Ok(dto);
            } catch(BadRequestException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost("{offerId}/add-opinion")]
        public ActionResult AddOpinion([FromRoute] int offerId, [FromBody] AddOpinionDto dto)
        {
            try
            {
                _orderService.AddOpinion(offerId, dto);
                return Ok();
            } catch(BadRequestException ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
