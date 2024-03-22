namespace ZleceniaAPI.Entities
{
    public class Opinion
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public virtual User Contractor { get; set; }
        public int PrincipalId { get; set; }
        public virtual User Principal { get; set;}
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
        public string Comment { get; set; }
        public int WorkQuality { get; set; }
        public int Punctuality { get; set; }
        public int Communication { get; set; }
        public int MeetingTheConditions { get; set; }
        public double FinalRate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
