using System;

namespace SportCourseRegistrationSystem.Models
{
    public class Announcement
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}