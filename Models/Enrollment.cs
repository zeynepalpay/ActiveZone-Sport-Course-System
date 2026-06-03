using System.ComponentModel.DataAnnotations;

namespace SportCourseRegistrationSystem.Models
{
    public class Enrollment
    {
        // Her kaydın veri tabanındaki benzersiz id değeri
        public int Id { get; set; }

        // Hangi üyenin kayıt olduğunu bilmek için üye id değerini yabancı anahtar (Foreign Key) olarak tutuyoruz
        [Required]
        [Display(Name = "Üye")]
        public int MemberId { get; set; }

        // Hangi kursa kayıt olunduğunu bilmek için kurs id değerini yabancı anahtar (Foreign Key) olarak tutuyoruz
        [Required]
        [Display(Name = "Kurs Branşı")]
        public int SportCourseId { get; set; }

        // Üyenin kursa tam olarak hangi gün ve saatte kayıt olduğunu otomatik tutmak için
        [Required]
        [Display(Name = "Kayıt Tarihi")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Navigation Properties: Kod tarafında ilişkili üye ve kurs bilgilerine direkt erişebilmek için
        public Member? Member { get; set; }
        public SportCourse? SportCourse { get; set; }
    }
}