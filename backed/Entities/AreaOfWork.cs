namespace ZleceniaAPI.Entities
{
    public class AreaOfWork
    {
        public int Id { get; set; }
        public string? Voivodeship { get; set; }
        public string? WholeCountry { get; set; }
    
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
