using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class AccountService : IAccountService
    {
        private OferiaDbContext _context;
        private IPasswordHasher<User> _passwordHasher;
        private AuthenticationSettings _authenticationSettings;

        public AccountService(OferiaDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public string RegisterUser(RegisterUserDto dto)
        {
            User newUser = new User();
            newUser.Email = dto.Email;
            newUser.FirstName = dto.FirstName;
            newUser.LastName = dto.LastName;
            newUser.PhoneNumber = dto.PhoneNumber;
            newUser.Street = dto.Street;
            newUser.City = dto.City;
            newUser.PostalCode = dto.PostalCode;
            newUser.BuildingNumber = dto.BuildingNumber;
            newUser.Voivodeship = dto.Voivodeship;
            newUser.CompanyName = dto.CompanyName;
            newUser.Description = dto.Description;
            newUser.TaxIdentificationNumber = dto.TaxIdentificationNumber;
            newUser.StatusOfUserId = dto.StatusOfUserId;
            newUser.TypeOfAccountId = dto.StatusOfUserId;

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var loginUserDto = new LoginUserDto
            {
                Email = dto.Email,
                Password = dto.Password
            };

            var jwtToken = GenerateJwt(loginUserDto);
            return jwtToken;
        }

        public string GenerateJwt(LoginUserDto loginUserDto)
        {
            var user = _context.Users
                .Include(x => x.StatusOfUser)
                .Include(x => x.TypeOfAccount)
                .FirstOrDefault(x => x.Email == loginUserDto.Email);

            if (user is null)
            {
                throw new BadRequestException("Nieprawidłowy email lub hasło");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Nieprawidłowy email lub hasło");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Email}"),
                new Claim("TypeOfAccount", $"{user.TypeOfAccount.Name}"),
                new Claim("StatusOfUser", $"{user.StatusOfUser.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
