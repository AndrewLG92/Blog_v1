﻿namespace Blog.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public ICollection<BlogPost>? BlogPosts { get; set; }
    }
}
