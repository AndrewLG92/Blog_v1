using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AdminCategoryController : Controller
    {

        private readonly BlogDbContext _dbBlogContext;

        public AdminCategoryController(BlogDbContext blogDbContext)
        {
            _dbBlogContext = blogDbContext;
        }


        public IActionResult AddCat()
        {
            return PartialView("_AddCatModal");
        }

        [HttpPost]
        public async Task<IActionResult> AddCat(AddBlogCategoryViewModel item)
        {

            //Mapping AddTagRequest to Tag Domain Model.
            var cat = new BlogCategory
            {
                Name = item.Name,
            };

            await _dbBlogContext.BlogCategories.AddAsync(cat);

            // Need to Save to the DB
            await _dbBlogContext.SaveChangesAsync();

            return RedirectToAction("AdminIndex", "AdminPanel");
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
                return PartialView("_EditCatModal", viewModel);
            }
            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var cat = await _dbBlogContext.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (cat != null)
            {
                var item = new UpdateCategoryViewModel()
                {
                    Id = cat.Id,
                    Name = cat.Name,
                };
                return PartialView("_EditCatModal", item);
            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCat(UpdateCategoryViewModel model)
        {
            var cat = await _dbBlogContext.BlogCategories.FindAsync(model.Id);

            if (cat != null)
            {
                cat.Name = model.Name;

                await _dbBlogContext.SaveChangesAsync();
                
            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCat(Guid id)
        {
            var cat = await _dbBlogContext.BlogCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cat is not null)
            {
                _dbBlogContext.BlogCategories.Remove(cat);
                await _dbBlogContext.SaveChangesAsync();
            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cat = await _dbBlogContext.BlogCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (cat is not null)
            {
                return PartialView("_DeleteCatModal", cat);

            }
            return RedirectToAction("AdminIndex", "AdminPanel");

        }
    }
}
