using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService account)
        {
            _accountService = account;
        }

        [HttpPost("register")]
        public ActionResult ReqisterUser([FromBody] RegisterUserDto dto)
        {
            string token = _accountService.RegisterUser(dto);

            return Ok(token);
        }

        [HttpPost("login")]
        public ActionResult LoginUser([FromBody] LoginUserDto dto) {
            try
            {
                string token = _accountService.GenerateJwt(dto);

                return Ok(token);
            } catch(BadRequestException ex)
            {
                return Unauthorized(ex);
            }
        }
    }
}
