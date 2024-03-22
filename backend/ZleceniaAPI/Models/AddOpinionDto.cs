using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class AddOpinionDto
    {
        public string Comment { get; set; }
        public int WorkQuality { get; set; }
        public int Punctuality { get; set; }
        public int Communication { get; set; }
        public int MeetingTheConditions { get; set; }
    }
}
