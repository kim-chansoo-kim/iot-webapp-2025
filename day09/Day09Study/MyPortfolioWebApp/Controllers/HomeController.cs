using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolioWebApp.Models;
using System.Diagnostics;

namespace MyPortfolioWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; // DB연동 가능

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            // DB에서 데이터를 불러온 뒤 About, Skill 객체에 데이터 담아서 뷰로 넘겨줘야 함
            var skillCount = _context.Skill.Count();
            var skill = await _context.Skill.ToListAsync();

            var about = await _context.About.ToListAsync();

            ViewBag.SkillCount = skillCount;
            ViewBag.ColNum = (skillCount / 2) + (skillCount % 2); // 3(7/2) + 1(7%2)

            var model = new AboutModel();
            model.About = about;
            model.Skill = skill;

            return View(model);
        }

        public IActionResult Contact()
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
