namespace ZleceniaAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public decimal? Budget { get; set; }
        public int PublicationDays { get; set; }
    }
}
