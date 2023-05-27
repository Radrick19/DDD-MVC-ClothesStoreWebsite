namespace Store.API.Models
{
    public class BreadcrumbItem
    {
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }

        public BreadcrumbItem(string title, bool isActive, string url = null)
        {
            Title = title;
            IsActive = isActive;
            Url = url;
        }
    }
}
