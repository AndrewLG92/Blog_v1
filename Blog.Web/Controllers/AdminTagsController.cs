using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext blogDbContext;

        public AdminTagsController(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_AddModel");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequestViewModel addTagRequest)
        {

            //Mapping AddTagRequest to Tag Domain Model.
            var tag = new Tag
            {
                Name = addTagRequest.Name,
            };

            await blogDbContext.Tags.AddAsync(tag);

            // Need to Save to the DB
            await blogDbContext.SaveChangesAsync();

            return RedirectToAction("AdminIndex", "AdminPanel");
        }


        [HttpGet]
        public async Task<IActionResult> EditTag(Guid id)
        {
            var tag = await blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag != null)
            {
                var item = new UpdateTagViewModel()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                };
                return PartialView("_TagModal", item);
            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTag(UpdateTagViewModel model)
        {
            var tag = await blogDbContext.Tags.FindAsync(model.Id);

            if (tag != null)
            {
                tag.Name = model.Name;

                await blogDbContext.SaveChangesAsync();

            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await blogDbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tag is not null)
            {
                blogDbContext.Tags.Remove(tag);
                await blogDbContext.SaveChangesAsync();
            }

            return RedirectToAction("AdminIndex", "AdminPanel");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await blogDbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tag is not null)
            {
                return PartialView("_DeleteModel", tag);
                
            }
            return RedirectToAction("AdminIndex", "AdminPanel");

        }
    }
}
