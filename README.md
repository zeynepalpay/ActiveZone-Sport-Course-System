# 🏋️ ActiveZone Spor Kursu Kayıt ve Takip Otomasyonu

ASP.NET Core MVC kullanılarak geliştirilmiş spor kursu kayıt ve takip sistemidir.

## 🚀 Proje Özeti

ActiveZone, spor merkezlerinin kurs yönetimi, üye kayıt işlemleri ve kurs takip süreçlerini dijital ortamda yönetebilmesi amacıyla geliştirilmiştir.

Kullanıcılar sistem üzerinden kursları görüntüleyebilir, kayıt olabilir ve kendi kayıtlarını takip edebilirler. Yönetici paneli sayesinde kurs, kayıt ve duyuru yönetimi gerçekleştirilebilir.

---

## 🛠 Kullanılan Teknolojiler

### Backend
- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQLite
- Session Management

### Frontend
- HTML5
- CSS3
- Bootstrap 5
- Bootstrap Icons
- JavaScript

---

## ✨ Özellikler

- Kullanıcı Kayıt Sistemi
- Kullanıcı Giriş Sistemi
- Kayıtlarım Sayfası
- Kurs Listeleme
- Kurs Arama ve Filtreleme
- Online Kurs Kaydı
- Kontenjan Takibi
- Dinamik Ücret Hesaplama
- Duyuru Yönetim Sistemi
- Yönetici Giriş Paneli
- CRUD İşlemleri
- Veri Doğrulama (Validation)
- Responsive Tasarım

---

## 🗄 Veritabanı Yapısı

### Member
- Id
- MemberNumber
- FullName
- Phone
- BirthDate

### SportCourse
- Id
- CourseName
- InstructorName
- Capacity
- DurationWeeks

### Enrollment
- Id
- MemberId
- SportCourseId
- EnrollmentDate

---

## 🔐 Güvenlik

- Session tabanlı yönetici giriş sistemi
- Yetkisiz erişim koruması
- ValidateAntiForgeryToken kullanımı
- Form doğrulama işlemleri

---

## 📚 Ders Bilgisi

Web Programlama Dersi Dönem Projesi

2025-2026
