using System.ComponentModel;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models
{
    public class OrderQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? CategoryId { get; set; }
        public string? Voivodeship { get; set; } = null;
        public string? City { get; set; } = null;
        public string? SortBy { get; set; } = "StartDate";
        public string? SearchText { get; set; }
        public Boolean IsActive { get; set; } = true;
        public SortDirection? SortDirection { get; set; }
    }
}
