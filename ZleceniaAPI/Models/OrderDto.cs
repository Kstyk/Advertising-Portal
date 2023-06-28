using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public virtual UserDto User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual CategoryDto Category { get; set; }
        public virtual AddressDto Address { get; set; }
        public decimal? Budget { get; set; }
        public int PublicationDays { get; set; } = 0;
        public bool AllowRemotely { get; set; }
        public DateTime StartDate { get; set; }
        public Boolean IsActive { get; set; }
        public virtual List<OfferDto> Offers { get; set; }
    }
}
