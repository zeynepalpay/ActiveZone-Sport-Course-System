using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourseRegistrationSystem.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SportCourseRegistrationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: / (Ana Sayfa Vitrini)
        public async Task<IActionResult> Index(string searchString)
        {
            // Veritabanındaki tüm kursları çekmek için sorguyu başlatıyoruz
            var coursesQuery = from c in _context.SportCourses select c;

            // Arama kutusuna bir şey yazıldıysa kurs adına göre filtreleme yapıyoruz
            if (!string.IsNullOrEmpty(searchString))
            {
                coursesQuery = coursesQuery.Where(s => s.CourseName.Contains(searchString));
                
                // Arama terimini kutunun içinde yazılı tutabilmek için ViewData'ya atıyoruz
                ViewData["CurrentFilter"] = searchString;
            }

            // Filtrelenmiş veya tüm listeyi asenkron olarak listeleyip View'a gönderiyoruz
            var filteredCourses = await coursesQuery.ToListAsync();
            return View(filteredCourses);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}