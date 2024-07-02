using Blog.Web.Models.Domain;

namespace Blog.Web.Models.ViewModels
{
    public class AddBlogRequestViewModel
    {
        public string Heading { get; set; }

        public string PageTitle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool Visible { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<BlogCategory> BlogCategories { get; set; }
    }
}
