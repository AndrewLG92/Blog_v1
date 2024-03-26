using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AdminBlogCategoryController : Controller
    {

        private readonly BlogDbContext _dbBlogContext;

        public AdminBlogCategoryController(BlogDbContext blogDbContext)
        {
            _dbBlogContext = blogDbContext;
        }


        public IActionResult AddCat()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IndexCat()
        {
            var cat = await _dbBlogContext.BlogCategories.ToListAsync();

            return View(cat);
        }

        [HttpGet]
        public async Task<IActionResult> EditCat(Guid id)
        {
            var cat = await _dbBlogContext.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            if(cat != null)
            {
                var viewModel = new UpdateCategoryViewModel
                {
                    Id = cat.Id,
                    Name = cat.Name,
                };
                return await Task.Run(() => View("EditCat", viewModel));
            }
            return RedirectToAction("IndexCat");
        }
        

        [HttpPost]
        public async Task<IActionResult> AddCat(AddBlogCategoryViewModel model)
        {
            //Mapping AddTagRequest to Tag Domain Model.
            var blogCat = new BlogCategory
            {
                Name = model.Name,
            };

            await _dbBlogContext.BlogCategories.AddAsync(blogCat);

            // Need to Save to the DB
            await _dbBlogContext.SaveChangesAsync();

            return RedirectToAction("AddCat");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCat(UpdateCategoryViewModel model)
        {
            var cat = await _dbBlogContext.BlogCategories.FindAsync(model.Id);

            if (cat != null)
            {
                cat.Name = model.Name;

                await _dbBlogContext.SaveChangesAsync();

                return RedirectToAction("IndexCat");
            }

            return RedirectToAction("IndexCat");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCat(BlogCategory model)
        {
            var cat = await _dbBlogContext.BlogCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (cat is not null)
            {
                _dbBlogContext.BlogCategories.Remove(model);
                await _dbBlogContext.SaveChangesAsync();
            }

            return RedirectToAction("IndexCat");
        }
    }
}
