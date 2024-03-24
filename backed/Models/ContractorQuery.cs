using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models
{
    public class ContractorQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SortBy { get; set; } = "LastName";
        public SortDirection? SortDirection { get; set; }
        public int? CategoryId { get; set; }
        public string? Voivodeship { get; set; } = null;
        public string? City { get; set; } = null;
        public string? SearchText { get; set; }
    }
}
