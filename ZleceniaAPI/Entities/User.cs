namespace ZleceniaAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int? AddressId { get; set; }
        public virtual Address? Address { get; set; }
        public string? Description { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public int TypeOfAccountId { get; set; }
        public virtual TypeOfAccount TypeOfAccount { get; set; } // zlecający czy wykonawca
        public int StatusOfUserId { get; set; }
        public virtual StatusOfUser StatusOfUser { get; set; } // osoba prywatna czy firma
        public List<Category> Categories{ get; set; }
        public User()
        {

        }
    }
}
