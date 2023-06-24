using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class AddOfferDto
    {
        public string Content { get; set; }
        public Decimal Price { get; set; }
        public string PriceFor { get; set; } // całość, za godzinę, za sztukę
    }
}
