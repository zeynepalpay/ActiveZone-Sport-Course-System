using System.ComponentModel.DataAnnotations;

namespace SportCourseRegistrationSystem.Models
{
    // ActiveZone Spor Salonu bünyesinde açılacak olan ana branşları (Fitness, Pilates, Havuz vb.) temsil eden modelimiz
    public class SportCourse
    {
        // Her kursun veri tabanında benzersiz şekilde tutulmasını sağlayan birincil anahtarımız (Primary Key)
        public int Id { get; set; }

        // Salonun prestijini ve üyelerin neye kaydolacağını belirleyen kurs branş adı (Örn: Crossfit, Yoga)
        [Required(ErrorMessage = "Kurs adı zorunludur.")]
        [Display(Name = "Kurs Branşı")]
        public string CourseName { get; set; } = string.Empty;

        // Kursun kalitesini ve takibini sağlayan uzman eğitmenin ad soyad bilgisi
        [Required(ErrorMessage = "Eğitmen adı zorunludur.")]
        [Display(Name = "Eğitmen")]
        public string InstructorName { get; set; } = string.Empty;

        // Salon güvenliği, ekipman sınırlılığı ve ders kalitesi için belirlenen maksimum üye limiti
        [Required(ErrorMessage = "Kontenjan zorunludur.")]
        [Range(1, 100, ErrorMessage = "Kontenjan 1 ile 100 arasında olmalıdır.")]
        [Display(Name = "Kontenjan")]
        public int Capacity { get; set; }

        // Üyelerin üyelik planlamasını (aylık/dönemlik) yapabilmesi için kursun toplam kaç hafta süreceği bilgisi
        [Required(ErrorMessage = "Kurs süresi zorunludur.")]
        [Range(1, 52, ErrorMessage = "Süre 1 ile 52 hafta arasında olmalıdır.")]
        [Display(Name = "Kurs Süresi (Hafta)")]
        public int DurationWeeks { get; set; }

        // Çoka çok (Many-to-Many) ilişki mimarisi: Bu kursa kayıt yaptıran tüm üyeleri ara tablo (Enrollment) üzerinden bağlayan koleksiyonumuz
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}