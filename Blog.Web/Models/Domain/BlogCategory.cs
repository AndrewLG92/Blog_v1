namespace Blog.Web.Models.Domain
{
    public class BlogCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }

    }
}
