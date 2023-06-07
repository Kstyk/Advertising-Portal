using Newtonsoft.Json;

namespace ZleceniaAPI.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual CategoryDto ParentCategory { get; set; }
        [JsonIgnore]
        public virtual List<CategoryDto> ChildCategories { get; set; }
    }
}
