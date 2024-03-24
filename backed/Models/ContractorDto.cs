namespace ZleceniaAPI.Models
{
    public class ContractorDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public virtual AddressDto Address { get; set; }
        public virtual List<UserCategoryDto> UserCategories { get; set; }
        public Double? AverageRate { get; set; }
        public int? CountOpinions { get; set; }
    }
}
