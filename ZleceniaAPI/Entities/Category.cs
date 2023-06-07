using Newtonsoft.Json;

namespace ZleceniaAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public virtual Category ParentCategory { get; set; }

        public virtual List<Category> ChildCategories { get; set; }
    }
}
