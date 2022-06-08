using ItransitionProject.CloudStorage;
using ItransitionProject.Models;
using ItransitionProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Web;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using ClosedXML.Excel;

namespace ItransitionProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SiteReviewContext _context;
        private readonly ICloudStorage _cloudStorage;

        public HomeController(ILogger<HomeController> logger, ICloudStorage cloudStorage, SiteReviewContext context)
        {
            _logger = logger;
            _cloudStorage = cloudStorage;
            _context = context;
        }
        private async Task UploadFile(Overview overview)
        {

            string fileNameForStorage = FormFileName(overview.Title, overview.ImageFile.FileName);
            overview.ImageUrl = await _cloudStorage.UploadFileAsync(overview.ImageFile, fileNameForStorage);
            overview.StorageName = fileNameForStorage;
        }

        private static string FormFileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        public IActionResult Index(string searchString)
        {
            ViewBag.Tags = _context.TagOverviews.Select(o => o.TagName).ToList();
            List<Overview> posts = _context.Overviews.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = _context.Overviews.Where(u => u.Title.ToLower().Contains(searchString.ToLower()) || u.TextOverview.ToLower().Contains(searchString.ToLower())).ToList();
                return View(posts);
            }
            var sort = posts.OrderByDescending(o => o.DateOverview);
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

        [HttpGet]
        public IActionResult EditOverview(int? id)
        {

            ViewBag.TagList = new MultiSelectList(_context.TagOverviews, "Idtag", "TagName");

            ViewData["Fkgroup"] = new SelectList(_context.GroupOverviews, "Idgroup", "GroupName");
            if (id != null)
            {
                Overview? overview = _context.Overviews.FirstOrDefault(p => p.Idoverview == id);
                if (overview != null)
                    return View(overview);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EditOverview(Overview overview, OverTag overTag, int[] tags, int id)
        {
            await _cloudStorage.DeleteFileAsync(overview.StorageName);
            await UploadFile(overview);

            var oldoverview = _context.Overviews.FirstOrDefault(o => o.Idoverview == id);
            if (oldoverview != null)
            {
                oldoverview.Title = overview.Title;
                oldoverview.Fkgroup = overview.Fkgroup;
                oldoverview.TextOverview = overview.TextOverview;
                oldoverview.ImageUrl = overview.ImageUrl;
                oldoverview.StorageName = overview.StorageName;
                oldoverview.RateOverview = overview.RateOverview;
            }
            _context.Update(oldoverview);
            _context.SaveChanges();

            if (tags != null)
            {
                _context.OverTags.RemoveRange(_context.OverTags.Where(i => i.Fkoverview == id));
                _context.SaveChanges();
                foreach (var item in tags)
                {
                    _context.OverTags.Add(new OverTag { Fktag = item, Fkoverview = id });
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult CreateOverview()
        {

            ViewBag.TagList = new MultiSelectList(_context.TagOverviews, "Idtag", "TagName");
            ViewData["Fkgroup"] = new SelectList(_context.GroupOverviews, "Idgroup", "GroupName");

            return View();
        }

        public IActionResult CreateComment()
        {
            return View();
        }

        public IActionResult CreateTag()
        {
            return View();
        }

        public IActionResult CreateGroup()
        {
            return View();
        }

        public IActionResult TagList()
        {
            return View(_context.TagOverviews.ToList());
        }

        public IActionResult GroupList()
        {
            return View(_context.GroupOverviews.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupOverview groupOverview)
        {
            _context.Add(groupOverview);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(TagOverview tagOverview)
        {
            _context.Add(tagOverview);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteTag(int? id)
        {
            var result = _context.TagOverviews.FirstOrDefault(p => p.Idtag == id);

            return View(result);
        }

        [HttpGet]
        public IActionResult DeleteGroup(int? id)
        {
            var result = _context.GroupOverviews.FirstOrDefault(p => p.Idgroup == id);

            return View(result);
        }

        [HttpPost]
        public IActionResult SetTheme(string data)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires=DateTime.Now.AddDays(1);
            Response.Cookies.Append("theme", data, cookie);
            return Ok();
        }
        

        [HttpPost]
        public IActionResult ConfirmDeleteTag(int? id)
        {
            TagOverview tagOverview = new TagOverview { Idtag = id.Value };
            _context.Entry(tagOverview).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("TagList");
        }

        [HttpPost]
        public IActionResult ConfirmDeleteGroup(int? id)
        {
            GroupOverview groupOverview = new GroupOverview { Idgroup = id.Value };
            _context.Entry(groupOverview).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("GroupList");
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentOverview commentOverview, int? id) 
        {
            _context.Add(commentOverview);
            _context.SaveChanges();
            int user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Iduser;
            _context.OverComs.Add(new OverCom { Fkcomment = commentOverview.IdcommentOverview, Fkoverviews = id ?? 10, Fkusers=user });
            _context.SaveChanges();
            return RedirectToAction("ViewOverview",new {id=id});
        }

        [HttpGet]
        public IActionResult DeleteComment(int? id)
        {
            var result = _context.CommentOverviews.Include(o=>o.OverComs).ThenInclude(o=>o.FkusersNavigation).ThenInclude(o=>o.Overviews).FirstOrDefault(p=>p.IdcommentOverview==id);
            
            return View(result);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteComment(int? id)
        {
            CommentOverview commentOverview = new CommentOverview {  IdcommentOverview = id.Value };
            _context.Entry(commentOverview).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditComment(int? id)
        {

            
            if (id != null)
            {
                CommentOverview commentOverview = _context.CommentOverviews.Include(o => o.OverComs).ThenInclude(o => o.FkusersNavigation).ThenInclude(o => o.Overviews).FirstOrDefault(p => p.IdcommentOverview == id);
                if (commentOverview != null)
                    return View(commentOverview);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EditComment(CommentOverview commentOverview, int id)
        {
            var oldcommentOverview = _context.CommentOverviews.FirstOrDefault(o => o.IdcommentOverview == id);
            if (oldcommentOverview != null)
            {
                oldcommentOverview.Comment = commentOverview.Comment;
            }
            _context.Update(oldcommentOverview);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOverview(Overview overview, int[] tags)
        {
            await UploadFile(overview);
            _context.Add(overview);
            _context.SaveChanges();
            overview.Fkuser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Iduser;

            foreach (var item in tags)
            {
                _context.OverTags.Add(new OverTag { Fktag = item, Fkoverview = overview.Idoverview });

            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteOverview(int? id)
        {
            var result = _context.Overviews.Include(o => o.FkgroupNavigation).Include(o => o.OverTags).ThenInclude(o => o.FktagNavigation).Include(o => o.FkuserNavigation).FirstOrDefault(p => p.Idoverview == id);
            return View(result);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            Overview overview = new Overview { Idoverview = id.Value };
            _cloudStorage.DeleteFileAsync(overview.StorageName);
            _context.Entry(overview).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Personal");
        }

        [HttpGet]
        [ActionName("DeleteUser")]
        public IActionResult ConfirmDeleteUser(int? id)
        {
            if (id != null)
            {
                User? user = _context.Users.FirstOrDefault(p => p.Iduser == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteUser(int? id)
        {
            if (id != null)
            {
                User user = new User { Iduser = id.Value };
                _context.Entry(user).State = EntityState.Deleted;
                _context.SaveChanges();
                return RedirectToAction("Users");
            }
            return NotFound();
        }

        public IActionResult ViewOverview(int id)
        {
            var comm = _context.OverComs.Include(o => o.FkcommentNavigation).Include(o=>o.FkusersNavigation).Where(o=>o.Fkoverviews==id);
            var result = _context.Overviews.Include(o => o.FkgroupNavigation)
                .Include(o => o.OverTags).ThenInclude(o => o.FktagNavigation)
                .Include(o => o.FkuserNavigation).Include(o=>o.OverComs).ThenInclude(o=>o.FkusersNavigation).Include(o => o.OverComs).ThenInclude(o => o.FkcommentNavigation).FirstOrDefault(p => p.Idoverview == id);
            
            return View(result);
        }

        public IActionResult GetReport() 
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет");
                var currentRow = 1;

                #region Header
                worksheet.Cell(1, 1).Value = "Количество созданных рецензий";
                worksheet.Cell(2, 1).Value = "Количество созданных комментариев";
                #endregion

                #region Body
                worksheet.Cell(1,2).Value = _context.Overviews.Include(x=>x.FkuserNavigation).Where(x=>x.FkuserNavigation.UserName==User.Identity.Name).Count();
                worksheet.Cell(2, 2).Value = _context.OverComs.Include(x =>x.FkusersNavigation).Where(x => x.FkusersNavigation.UserName == User.Identity.Name).Count();
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlfornats-officedocument.spreadsheetml.sheet",
                        "Отчет.xlsx"
                        );
                }
            }
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