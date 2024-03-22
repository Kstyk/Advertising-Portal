using Newtonsoft.Json;
using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Models
{
    public class UserCategoryDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual CategoryDto ParentCategory { get; set; }
        [JsonIgnore]
        public virtual List<CategoryDto> ChildCategories { get; set; }
    }
}
