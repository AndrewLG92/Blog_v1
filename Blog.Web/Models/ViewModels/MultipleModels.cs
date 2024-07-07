using Blog.Web.Models.Domain;

namespace Blog.Web.Models.ViewModels
{
    public class MultipleModels
    {
        public ICollection<BlogCategory>? BlogCategories { get; set; }

        public ICollection<Tag>? Tags { get; set; }
    }
}
