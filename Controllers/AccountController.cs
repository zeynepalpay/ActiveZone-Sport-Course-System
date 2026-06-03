using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SportCourseRegistrationSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            // Sabit admin hesabı kontrolü
            if (username == "admin" && password == "1234")
            {
                // Giriş başarılıysa Session'a "IsAdmin" anahtarını "true" olarak yazıyoruz
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "SportCourses"); // Doğrudan Kurs Paneline yönlendir
            }

            // Hatalı girişte mesaj göster
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Session'ı temizle ve ana sayfaya gönder
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}