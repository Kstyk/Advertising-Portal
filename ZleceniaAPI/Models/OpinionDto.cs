using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class OpinionDto
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int PrincipalId { get; set; }
        public int OrderId { get; set; }
        public string Title { get; set; }
        public int OfferId { get; set; }
        public string Comment { get; set; }
        public int WorkQuality { get; set; }
        public int Punctuality { get; set; }
        public int Communication { get; set; }
        public int MeetingTheConditions { get; set; }
        public double FinalRate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
