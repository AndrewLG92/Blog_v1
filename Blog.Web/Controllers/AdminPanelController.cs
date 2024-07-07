using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using System.Runtime.InteropServices;
using Blog.Web.Models.ViewModels;

namespace Blog.Web.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly BlogDbContext _context;

        public AdminPanelController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: AdminPanel
        public async Task<IActionResult> AdminIndex()
        {
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlogCategory> categories = await _context.BlogCategories.ToListAsync();

            var model = new List<MultipleModels>
            {
                new MultipleModels
                {
                    Tags = tags,
                    BlogCategories = categories
                }

            };

            return View(model);
        }

        // GET: AdminPanel/Create
        public IActionResult CreateTag()
        {
            return View();
        }

        // POST: AdminPanel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTag(AddTagRequestViewModel tag)
        {
            if (ModelState.IsValid)
            {
                var item = new Tag
                {
                    Name = tag.Name,
                };
                await _context.Tags.AddAsync(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminIndex");
            }
            return RedirectToAction("AdminIndex");
        }

        // POST: AdminPanel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Heading,PageTitle,Content,UrlHandle,PublishedDate,Author,Visible")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        // GET: AdminPanel/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: AdminPanel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(Guid id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }
    }
}
