using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        void EditUser(EditUserDto dto);
        string GenerateJwt(LoginUserDto loginUserDto);
        List<StatusOfUserDto> GetAllStatusesOfUser();
        List<TypeOfAccountDto> GetAllTypesOfAccount();
        UserProfileDto GetLoggedUserProfile();
        AreaOfWorkDto GetUserAreaOfWork(int? userId);
        AreaOfWorkDto EditAreaOfWork(AreaOfWorkDto dto);
    }
}