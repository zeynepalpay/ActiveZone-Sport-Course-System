using System.ComponentModel.DataAnnotations;

namespace SportCourseRegistrationSystem.Models
{
    // ActiveZone Spor Salonu'na kayıt yaptıran, kurslara katılım sağlayacak olan üyelerin profil ve veri yönetim modelidir
    public class Member
    {
        // Her üyenin veri tabanında benzersiz (unique) olmasını sağlayan birincil anahtar değerimiz
        public int Id { get; set; }

        // Salon içi operasyonlarda, turnikelerde veya üye sorgulamalarında kullanılacak kurumsal üye kodu (Örn: AZ-2026-001)
        [Required(ErrorMessage = "Üye numarası zorunludur.")]
        [Display(Name = "Üye Numarası")]
        public string MemberNumber { get; set; } = string.Empty;

        // Üyenin resmi kayıtlarda ve listelerde görünecek olan ad soyad bilgisi
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        // Üyenin yaş sınırlamalarına ve uygun seanslara atanabilmesi için tutulan doğum tarihi verisi
        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }

        // Acil durumlarda üyelere ulaşabilmek veya bilgilendirme SMS'leri gönderebilmek için kritik iletişim kanalı
        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        [Display(Name = "İletişim Numarası")]
        public string Phone { get; set; } = string.Empty;

        // Spor salonu güvenliği ve antrenör bilgilendirmesi için üyenin kronik rahatsızlık veya alerji durumunu tutan alan
        [Required(ErrorMessage = "Sağlık durumu seçilmelidir.")]
        [Display(Name = "Sağlık Durumu")]
        public string HealthStatus { get; set; } = string.Empty;

        // Çoka çok (Many-to-Many) ilişki mimarisi: Bu üyenin hangi spor kurslarına kayıt yaptırdığını ara tablo (Enrollment) üzerinden bağlayan koleksiyon
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}