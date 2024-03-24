using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{contractorId}")]
        public ActionResult<UserProfileDto> GetUserProfile([FromRoute] int contractorId)
        {
            var userProfile = _contractorService.GetContractorProfile(contractorId);

            return Ok(userProfile);
        }

        [HttpGet("{contractorId}/area-of-work")]
        public ActionResult<AreaOfWorkDto> GetUserAreaOfWork([FromRoute] int contractorId)
        {
            var areaOfWork = _contractorService.GetContractorAreaOfWork(contractorId);

            return Ok(areaOfWork);
        }

        [HttpGet("{contractorId}/opinions")]
        public ActionResult<IEnumerable<OpinionDto>> GetContractorOpinions([FromRoute] int contractorId)
        {
            var opinions = _orderService.GetContractorOpinions(contractorId);

            return Ok(opinions);
        }
    }
}
