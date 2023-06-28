using System.ComponentModel;
using ZleceniaAPI.Enums;

namespace ZleceniaAPI.Models
{
    public class OrderQuery
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; } = "StartDate";
        public Boolean IsActive { get; set; } = true;
        public SortDirection? SortDirection { get; set; }
    }
}
