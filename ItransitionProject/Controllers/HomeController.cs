using ItransitionProject.Models;
using ItransitionProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ItransitionProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SiteReviewContext _context;

        
        

        

        public HomeController(ILogger<HomeController> logger,SiteReviewContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View(_context.Overviews.ToList());
        }
        

        public IActionResult CreateOverview(/*int id*/)
        {
            
            ViewBag.TagList = new MultiSelectList(_context.TagOverviews, "Idtag", "TagName");
            
            ViewData["Fkgroup"] = new SelectList(_context.GroupOverviews, "Idgroup", "GroupName");
            
            return View();
        }
        

        [HttpPost]
        public IActionResult CreateOverview(Overview overview, int[] tags, TagOverview tag)
        {
            
            _context.Add(overview);
            _context.SaveChanges();
            
            foreach (var item in tags) 
            {
                _context.OverTags.Add(new OverTag { Fktag=item,Fkoverview=overview.Idoverview});
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ViewOverview(int button)
        {
            var result = _context.Overviews.Include(o => o.FkgroupNavigation).Include(o=>o.OverTags).ThenInclude(o=>o.FktagNavigation).FirstOrDefault(p => p.Idoverview == button);
            return View(result);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}