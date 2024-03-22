using Microsoft.AspNetCore.Authorization;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Authorization
{
    public class TypeOfAccountRequirementHandler : AuthorizationHandler<TypeOfAccountRequirement>
    {
        private OferiaDbContext _dbContext;
        private IUserContextService _userContextService;

        public TypeOfAccountRequirementHandler(OferiaDbContext dbContext, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TypeOfAccountRequirement requirement)
        {
            var userId = _userContextService.GetUserId;

            if (userId == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var user = _dbContext.Users.Find(userId);

            var typeOfAccountName = _dbContext.TypesOfAccounts.Find(user.TypeOfAccountId);

            if(typeOfAccountName.Name == requirement.TypeOfAccountName)
            {
                context.Succeed(requirement);
            } 
            return Task.CompletedTask;
        }
    }
}
