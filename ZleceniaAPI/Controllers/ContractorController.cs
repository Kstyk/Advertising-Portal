using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Models;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Controllers
{
    [Route("api/contractor")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private IOrderService _orderService;
        private IContractorService _contractorService;

        public ContractorController(IOrderService orderService, IContractorService contractorService)
        {
            _orderService = orderService;
            _contractorService = contractorService;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<ContractorDto>> GetAllContractors([FromQuery] ContractorQuery? query)
        {
            var result = _contractorService.GetAllContractors(query);

            return Ok(result);
        }
    }
}
