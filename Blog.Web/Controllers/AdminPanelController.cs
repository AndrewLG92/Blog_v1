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
    }
}
