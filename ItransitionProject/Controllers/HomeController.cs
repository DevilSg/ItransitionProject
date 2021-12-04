using ItransitionProject.Models;
using ItransitionProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

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
        
        public IActionResult Index(string searchString)
        {


            ViewBag.Tags = _context.TagOverviews.Select(o => o.TagName).ToList();
            var posts = from m in _context.Overviews
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString == _context.Overviews.FirstOrDefault(u => u.Title == searchString)?.Title)
                {
                    posts = posts.Where(s => s.Title!.Contains(searchString));
                }
                else
                    posts = posts.Where(s => s.TextOverview!.Contains(searchString));

                return View(posts);
            }
            List<Overview>sort = posts.OrderByDescending(o => o.DateOverview).ToList();
            return View(sort);
        }
        public IActionResult Users() 
        {
            return View(_context.Users.ToList());
        }
        
        public IActionResult Personal(int id)
        {
            
            var userName = User.Identity.Name;
            if (this.User.IsInRole("admin"))
            {

                return View(_context.Overviews.Where(u => u.Fkuser == id).ToList());
            }
            return View(_context.Overviews.Where(u => u.FkuserNavigation.UserName == userName).ToList());
        }
        

        public IActionResult CreateOverview()
        {
            ViewBag.data = _context.TagOverviews.ToList();
            ViewBag.TagList = new MultiSelectList(_context.TagOverviews, "Idtag", "TagName");
            
            ViewData["Fkgroup"] = new SelectList(_context.GroupOverviews, "Idgroup", "GroupName");
            
            return View();
        }
        

        [HttpPost]
        public IActionResult CreateOverview(Overview overview, int[] tags)
        {
            
            _context.Add(overview);
            _context.SaveChanges();
            overview.Fkuser =  _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Iduser;
            
            foreach (var item in tags) 
            {
                _context.OverTags.Add(new OverTag { Fktag=item,Fkoverview=overview.Idoverview});
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult ViewOverview(int button)
        { 
            var result = _context.Overviews.Include(o => o.FkgroupNavigation).Include(o=>o.OverTags).ThenInclude(o=>o.FktagNavigation).Include(o=>o.FkuserNavigation).FirstOrDefault(p => p.Idoverview == button);
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