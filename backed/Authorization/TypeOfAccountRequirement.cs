using Microsoft.AspNetCore.Authorization;

namespace ZleceniaAPI.Authorization
{
    public class TypeOfAccountRequirement : IAuthorizationRequirement
    {
        public string TypeOfAccountName { get; set; }
        public TypeOfAccountRequirement(string name) {
            TypeOfAccountName = name;
        }
    }
}
