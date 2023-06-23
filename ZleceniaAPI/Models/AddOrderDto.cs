using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class AddOrderDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Voivodeship { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public decimal? Budget { get; set; }
        public int PublicationDays { get; set; }
        public bool AllowRemotely { get; set; }

    }
}
