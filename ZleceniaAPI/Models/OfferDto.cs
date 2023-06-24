using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class OfferDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? CompanyName { get; set; }
        public string Content { get; set; }
        public Decimal Price { get; set; }
        public string PriceFor { get; set; } // całość, za godzinę, za sztukę
        public DateTime PublicDate { get; set; }
    }
}
