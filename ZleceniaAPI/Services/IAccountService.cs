﻿using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginUserDto loginUserDto);
    }
}