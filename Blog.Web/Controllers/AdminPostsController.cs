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

        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            var filepath = "";
            foreach (IFormFile photo in files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "Images", photo.FileName);
                using(var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                filepath = "https://localhost:7168/" + "Images/" + photo.FileName;
            }
            return Json(new { url = filepath });
        }
    }
}
