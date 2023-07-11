namespace ZleceniaAPI.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public List<UserCategoryDto>? Categories { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }

        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            PageNumber = pageNumber;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public PagedResult(List<T> items, List<UserCategoryDto>? categories, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            Categories = categories;
            TotalItemsCount = totalCount;
            PageNumber = pageNumber;
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
