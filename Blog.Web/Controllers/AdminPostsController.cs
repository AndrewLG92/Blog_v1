using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    
    public class AdminPostsController : Controller
    {
        private readonly BlogDbContext _dbBlogContext;

        private readonly IWebHostEnvironment _env;

        public AdminPostsController(BlogDbContext dbBlogContext, IWebHostEnvironment env)
        {

            _dbBlogContext = dbBlogContext;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> AddBlogs()
        {
            var tags = await _dbBlogContext.Tags.ToListAsync();
            var cats = await _dbBlogContext.BlogCategories.ToListAsync();

            var TagsCats = new AddBlogRequestViewModel
            {
                Tags = tags,
                BlogCategories = cats,
            };

            return View(TagsCats);
        }

        [HttpGet]
        public IActionResult DisplayBlogs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBlogs(AddBlogRequestViewModel model)
        {
            //Mapping AddTagRequest to Tag Domain Model.
            var blog = new BlogPost
            {
                Heading = model.Heading,
                PageTitle = model.PageTitle,
                Content = model.Content,
                UrlHandle = model.UrlHandle,
                PublishedDate = model.PublishedDate,
                Author = model.Author,
                Visible = model.Visible,
                Tags = model.Tags,
                BlogCategories = model.BlogCategories,
                
            };

            await _dbBlogContext.BlogPosts.AddAsync(blog);

            // Need to Save to the DB
            await _dbBlogContext.SaveChangesAsync();

            return RedirectToAction("AddBlogs");
        }

        [HttpPost("/upload")]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            var fileUrls = new List<string>();
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var fileUrl = Url.Content($"~/images/{fileName}");
                fileUrls.Add(fileUrl);
            }

            return Ok(new { locations = fileUrls });
        }
    }
}
