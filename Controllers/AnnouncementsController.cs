using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportCourseRegistrationSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SportCourseRegistrationSystem.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "announcements.json");

        // Yardımcı Metot: Dosyadan duyuruları oku
        private List<Announcement> LoadAnnouncements()
        {
            //  Çakışmayı önlemek için System.IO.File olarak güncellendi
            if (!System.IO.File.Exists(_filePath)) return new List<Announcement>();
            var json = System.IO.File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Announcement>>(json) ?? new List<Announcement>();
        }

        // Yardımcı Metot: Dosyaya duyuruları kaydet
        private void SaveAnnouncements(List<Announcement> list)
        {
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            
            // Çakışmayı önlemek için System.IO.File olarak güncellendi
            System.IO.File.WriteAllText(_filePath, json);
        }

        // GET: /Announcements (Herkese Açık Duyuru Listesi)
        public IActionResult Index()
        {
            var announcements = LoadAnnouncements();
            announcements.Reverse();
            return View(announcements);
        }

        // GET: /Announcements/Manage (Sadece Admin Yönetim Paneli)
        public IActionResult Manage()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }
            var announcements = LoadAnnouncements();
            announcements.Reverse();
            return View(announcements);
        }

        // POST: /Announcements/Create (Duyuru Ekleme İşlemi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string title, string content)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                var announcements = LoadAnnouncements();
                announcements.Add(new Announcement
                {
                    Title = title,
                    Content = content,
                    Date = DateTime.Now
                });
                SaveAnnouncements(announcements);
            }

            return RedirectToAction("Manage");
        }

        // POST: /Announcements/DeleteAll (Tüm Duyuruları Temizle)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            //  Çakışmayı önlemek için System.IO.File olarak güncellendi
            if (System.IO.File.Exists(_filePath))
            {
                System.IO.File.Delete(_filePath);
            }

            return RedirectToAction("Manage");
        }
    }
}