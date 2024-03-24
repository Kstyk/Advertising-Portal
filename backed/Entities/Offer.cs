namespace ZleceniaAPI.Entities
{
    public class Offer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Content { get; set; }
        public Decimal? Price { get; set; }
        public string? PriceFor { get; set; } // całość, za godzinę, za sztukę
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public DateTime PublicDate { get; set; }
        public Boolean? IsWinner { get; set; } = null;
    }
}
