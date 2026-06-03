using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCourseRegistrationSystem.Data;
using SportCourseRegistrationSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportCourseRegistrationSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // DbContext bağımlılık enjeksiyonu
        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Enrollments
        // Kayıtlı tüm üyeleri ve seçtikleri kursları listeler
        public async Task<IActionResult> Index()
        {
            var allEnrollments = await _context.Enrollments
                .Include(e => e.Member)       // Üye bilgilerini bağla
                .Include(e => e.SportCourse)  // Kurs bilgilerini bağla
                .OrderByDescending(e => e.EnrollmentDate) // En yeni kayıt en üstte
                .ToListAsync();

            return View(allEnrollments);
        }

        // GET: /Enrollments/Register?courseId=X
        // Kayıt formunu açar ve seçilen kursun bilgilerini taşır
        public async Task<IActionResult> Register(int courseId)
        {
            var course = await _context.SportCourses.FindAsync(courseId);
            
            if (course == null)
            {
                return NotFound("Kurs bulunamadı.");
            }

            // Kontenjan kontrolü
            if (course.Capacity <= 0)
            {
                return BadRequest("Bu kursun kontenjanı dolmuştur.");
            }

            // Kurs bilgilerini arayüze göndermek için ViewBag'e atıyoruz
            ViewBag.CourseId = course.Id;
            ViewBag.CourseName = course.CourseName;
            ViewBag.InstructorName = course.InstructorName;

            return View();
        }

        // POST: /Enrollments/Register
        // Yeni üye ve kayıt ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int courseId, string fullName, DateTime birthDate, string phone, string healthStatus)
        {
            var course = await _context.SportCourses.FindAsync(courseId);
            if (course == null)
            {
                ModelState.AddModelError("", "Seçilen kurs sistemde bulunamadı.");
                return View();
            }

            // Doğum tarihi kontrolü (Gelecek tarih girilmesini engelle)
            if (birthDate > DateTime.Now)
            {
                ModelState.AddModelError("", "Doğum tarihi bugünden büyük olamaz.");
                ViewBag.CourseId = course.Id;
                ViewBag.CourseName = course.CourseName;
                ViewBag.InstructorName = course.InstructorName;
                return View();
            }

            // Telefon numarası kontrolü (Sadece rakam ve tam 11 hane olmalı)
            if (string.IsNullOrEmpty(phone) || phone.Length != 11 || !phone.All(char.IsDigit))
            {
                ModelState.AddModelError("", "Telefon numarası sadece rakamlardan oluşmalı ve 11 hane olmalıdır (Örn: 05xxxxxxxxx).");
                ViewBag.CourseId = course.Id;
                ViewBag.CourseName = course.CourseName;
                ViewBag.InstructorName = course.InstructorName;
                return View();
            }

            if (course.Capacity <= 0)
            {
                ModelState.AddModelError("", "Seçilen kursun kontenjanı dolmuştur.");
                return View();
            }

            // İsim alanı boş bırakılamaz kontrolü
            if (string.IsNullOrEmpty(fullName))
            {
                ModelState.AddModelError("", "Lütfen adınızı ve soyadınızı giriniz.");
                ViewBag.CourseId = course.Id;
                ViewBag.CourseName = course.CourseName;
                ViewBag.InstructorName = course.InstructorName;
                return View();
            }

            // Rastgele üye numarası üretme (AZ- ve son 5 hane)
            string uniqueId = DateTime.Now.Ticks.ToString();
            string autoMemberNumber = "AZ-" + uniqueId.Substring(uniqueId.Length - 5);

            // Yeni üyeyi oluştur ve veritabanına ekle
            var newMember = new Member
            {
                MemberNumber = autoMemberNumber,
                FullName = fullName,
                BirthDate = birthDate,
                Phone = phone,
                HealthStatus = healthStatus ?? "Belirtilmedi"
            };

            _context.Members.Add(newMember);
            await _context.SaveChangesAsync(); 

            // Kurs kontenjanını 1 düşür ve güncelle
            course.Capacity = course.Capacity - 1; 
            _context.SportCourses.Update(course); 

            // Ara tabloya (Enrollment) kaydı ekle
            var enrollment = new Enrollment
            {
                SportCourseId = course.Id,
                MemberId = newMember.Id,
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync(); 

            // Onay sayfasında göstermek için bilgileri TempData ile taşıyoruz
            TempData["MemberName"] = fullName;
            TempData["MemberNumber"] = autoMemberNumber; 
            TempData["Phone"] = phone;
            TempData["CourseName"] = course.CourseName;
            TempData["Instructor"] = course.InstructorName;

            return RedirectToAction(nameof(Confirmation));
        }

        // GET: /Enrollments/Confirmation
        // Kayıt başarılı sayfası
        public IActionResult Confirmation()
        {
            if (TempData["MemberName"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Enrollments/CancelEnrollment
        // Kayıt iptal etme ve kontenjanı 1 geri artırma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEnrollment(int id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.SportCourse)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
            {
                return NotFound("İptal edilecek kayıt bulunamadı.");
            }

            // Kurs duruyorsa kontenjanı geri iade et
            if (enrollment.SportCourse != null)
            {
                enrollment.SportCourse.Capacity = enrollment.SportCourse.Capacity + 1;
                _context.SportCourses.Update(enrollment.SportCourse);
            }

            // Kaydı sil
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}