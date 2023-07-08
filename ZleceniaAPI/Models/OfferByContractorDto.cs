namespace ZleceniaAPI.Models
{
    public class OfferByContractorDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Decimal? Price { get; set; }
        public string? PriceFor { get; set; } // całość, za godzinę, za sztukę
        public DateTime PublicDate { get; set; }
        public int OrderId { get; set; }
        public string OrderTitle { get; set; }
        public virtual CategoryDto Category { get; set; }
        public bool AllowRemotely { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public int PublicationDays { get; set; }

    }
}
