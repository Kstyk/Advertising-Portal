using System.Security.Claims;

namespace ZleceniaAPI.Services
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal? User { get; }
    }
}