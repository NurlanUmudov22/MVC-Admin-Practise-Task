using Fiorello_PB101.Models;
using Fiorello_PB101.Services;
using Fiorello_PB101.Services.Interfaces;
using Fiorello_PB101.ViewModels.Blog;
using Fiorello_PB101.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello_PB101.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }


        [HttpGet]

        public async Task<IActionResult> Index()
        {
            return View( await _blogService.GetAllAsync());
        }






        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            bool existBlog = await _blogService.ExistAsync(blog.Title, blog.Description);

            if (existBlog)
            {
                ModelState.AddModelError("Title", "This blog already exist");
                return View();
            }
            if (existBlog)
            {
                ModelState.AddModelError("Description", "This blog already exist");
                return View();
            }


            await _blogService.CreateAsync(new Blog { Title = blog.Title, 
                                                      Description=blog.Description,
                                                      Image= "blog-feature-img-1.jpg"
                                                     });
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return NotFound();

            await _blogService.DeleteAsync(blog);

            return RedirectToAction("Index");

        }


        //[HttpGet]
        //public async Task<IActionResult> Detail()
        //{
        //    return View();
        //}
    }
}
