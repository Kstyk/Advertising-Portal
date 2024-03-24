using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IPasswordResetService
    {
        void ForgotPassword(ForgotPasswordDto dto);
        void ResetPassword(ResetPasswordDto dto);
    }
}