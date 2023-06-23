namespace ZleceniaAPI.Models
{
    public class CreateUserCategoryDto
    {
            public List<int> Categories { get; set; }
            public string? Voivodeship { get; set; }
            public string? WholeCountry { get; set; }
    }
}
