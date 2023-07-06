using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginUserDto loginUserDto);
        List<StatusOfUserDto> GetAllStatusesOfUser();
        List<TypeOfAccountDto> GetAllTypesOfAccount();
        UserProfileDto GetLoggedUserProfile();
    }
}