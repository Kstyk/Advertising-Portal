using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public virtual AddressDto Address { get; set; }
        public virtual TypeOfAccountDto TypeOfAccount { get; set; }
        public virtual StatusOfUserDto StatusOfUser { get; set; }

    }
}
