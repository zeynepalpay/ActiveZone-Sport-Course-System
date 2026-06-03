using Microsoft.EntityFrameworkCore;
using SportCourseRegistrationSystem.Models;

namespace SportCourseRegistrationSystem.Data
{
    // Veri tabanı yönetimini ve tablolar arası köprüleri kuran ana merkez sınıfımız
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veri tabanında oluşacak tablolarımızı (DbSet) ekiyoruz
        public DbSet<Member> Members { get; set; }
        public DbSet<SportCourse> SportCourses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        // Fluent API kullanarak tablolar arasındaki ilişkileri ve kuralları netleştiriyoruz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enrollment (Kayıt) ara tablosundaki ilişkileri tanımlıyoruz
            
            // 1. Bir kaydın mutlaka bir üyesi olmalı ve üye silinirse kayıtları da temizlenmeli
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Member)
                .WithMany(m => m.Enrollments)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Bir kaydın mutlaka bir kursu olmalı ve kurs silinirse kayıtları da temizlenmeli
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.SportCourse)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.SportCourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}