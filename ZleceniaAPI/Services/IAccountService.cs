using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IAccountService
    {
        string RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginUserDto loginUserDto);
    }
}