using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourseRegistrationSystem.Data;
using SportCourseRegistrationSystem.Models;
using System.Threading.Tasks;

namespace SportCourseRegistrationSystem.Controllers
{
    public class SportCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SportCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /SportCourses (Kurs Listesi)
        public async Task<IActionResult> Index()
        {
            return View(await _context.SportCourses.ToListAsync());
        }

        // GET: /SportCourses/Create (Yeni Kurs Formu)
        public IActionResult Create()
        {
            return View();
        }

        // POST: /SportCourses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportCourse sportCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportCourse);
        }

        // GET: /SportCourses/Edit/5 (Kurs Düzenleme Formu)
        public async Task<IActionResult> Edit(int id)
        {
            var sportCourse = await _context.SportCourses.FindAsync(id);
            if (sportCourse == null)
            {
                return NotFound();
            }
            return View(sportCourse);
        }

        // POST: /SportCourses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SportCourse sportCourse)
        {
            if (id != sportCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.SportCourses.AnyAsync(e => e.Id == sportCourse.Id))
                    {
                        return NotFound();
                    }
                    else { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sportCourse);
        }

        // POST: /SportCourses/Delete/5 (Kurs Silme)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var sportCourse = await _context.SportCourses.FindAsync(id);
            if (sportCourse != null)
            {
                // Cascade delete ile bağlı kayıtlar otomatik temizleniyor
                _context.SportCourses.Remove(sportCourse);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}