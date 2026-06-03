using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportCourseRegistrationSystem.Models;
using System.Text.Json;

namespace SportCourseRegistrationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _userFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "useraccounts.json");

        private List<UserAccount> LoadUsers()
        {
            if (!System.IO.File.Exists(_userFilePath))
                return new List<UserAccount>();

            var json = System.IO.File.ReadAllText(_userFilePath);
            return JsonSerializer.Deserialize<List<UserAccount>>(json) ?? new List<UserAccount>();
        }

        private void SaveUsers(List<UserAccount> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_userFilePath, json);
        }

        // =========================
        // KULLANICI REGISTER
        // =========================

        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserRegister(string fullName, string userName, string password)
        {
            var users = LoadUsers();

            if (users.Any(u => u.UserName == userName))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor.";
                return View();
            }

            users.Add(new UserAccount
            {
                FullName = fullName,
                UserName = userName,
                Password = password
            });

            SaveUsers(users);

            HttpContext.Session.SetString("UserName", fullName);
            HttpContext.Session.SetString("LoginName", userName);

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // KULLANICI LOGIN
        // =========================

        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin(string userName, string password)
        {
            var users = LoadUsers();

            var user = users.FirstOrDefault(u =>
                u.UserName == userName &&
                u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("LoginName", user.UserName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UserLogout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("LoginName");

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // ADMIN LOGIN
        // =========================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "1234")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "SportCourses");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}