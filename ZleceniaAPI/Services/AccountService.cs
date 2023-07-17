using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Enums;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public class AccountService : IAccountService
    {
        private OferiaDbContext _context;
        private IPasswordHasher<User> _passwordHasher;
        private AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        private IUserContextService _userContextService;


        public AccountService(OferiaDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper, IUserContextService userContext)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
            _userContextService = userContext;
        }

        public List<StatusOfUserDto> GetAllStatusesOfUser()
        {
            var statuses = _mapper.Map<List<StatusOfUserDto>>(_context.StatusOfUsers);

            return statuses;
        }

        public List<TypeOfAccountDto> GetAllTypesOfAccount()
        {
            var types = _mapper.Map<List<TypeOfAccountDto>>(_context.TypesOfAccounts);

            return types;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.Password = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public void EditUser(EditUserDto dto)
        {
            var userId = _userContextService.GetUserId;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var editUser = _mapper.Map<EditUserDto, User>(dto, user);

            var address = _context.Addresses.First(a => a.Id == editUser.AddressId);

            if (address != null)
            {
                address.Voivodeship = dto.Voivodeship;
                address.City = dto.City;
                address.Street = dto.Street;
                address.BuildingNumber = dto.BuildingNumber;
                address.PostalCode = dto.PostalCode;

                _context.Addresses.Update(address);
            }

            _context.Users.Update(editUser);
            _context.SaveChanges();
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
                new Claim("Id", $"{user.Id}"),
                new Claim("Email", $"{user.Email}"),
                new Claim("FirstName", $"{user.FirstName}"),
                new Claim("LastName", $"{user.LastName}"),
                new Claim("TypeOfAccount", $"{user.TypeOfAccount.Name}"),
                new Claim("StatusOfUser", $"{user.StatusOfUser.Name}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            

            return tokenHandler.WriteToken(token);
        }

        public UserProfileDto GetLoggedUserProfile()
        {
            var userId = _userContextService.GetUserId;

            var userProfile = _mapper.Map<UserProfileDto>(_context.Users
                .Include(a => a.Address)
                .Include(s => s.StatusOfUser)
                .Include(t => t.TypeOfAccount)
                .FirstOrDefault(u => u.Id == userId));

            return userProfile;
        }

        public AreaOfWorkDto GetUserAreaOfWork(int? userId)
        {
            var userArea = _userContextService.GetUserId;

            var areaOfWork = _mapper.Map<AreaOfWorkDto>(
                _context.AreaOfWorks.FirstOrDefault(a => a.UserId == userArea));

            return areaOfWork;
        } 

        public AreaOfWorkDto EditAreaOfWork(AreaOfWorkDto dto)
        {
            var userId = _userContextService.GetUserId;
            var area = _mapper.Map<AreaOfWork>(dto);

            var areaOfWork = _context.AreaOfWorks.FirstOrDefault(a => a.UserId == userId);

            if(areaOfWork == null)
            {
                area.UserId = (int)userId;
                _context.AreaOfWorks.Add(area);
                _context.SaveChanges();

                return dto;
            } else
            {
                dto.Id = areaOfWork.Id;
                areaOfWork.Voivodeship = area.Voivodeship;
                areaOfWork.WholeCountry = area.WholeCountry;

                _context.AreaOfWorks.Update(areaOfWork);
                _context.SaveChanges();

                return dto;
            }
        }
    }
}
