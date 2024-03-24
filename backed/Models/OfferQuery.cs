using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models
{
    public class OfferQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SortBy { get; set; } = "PublicDate";
        public SortDirection? SortDirection { get; set; }
        public Boolean? IsActive { get; set; }
    }
}
