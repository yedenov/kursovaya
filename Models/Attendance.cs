using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnetcore.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите студента")]
        [Display(Name = "Студент")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public required Student Student { get; set; }

        [Required(ErrorMessage = "Выберите занятие")]
        [Display(Name = "Занятие")]
        public int LessonId { get; set; }

        [ForeignKey("LessonId")]
        public required Lesson Lesson { get; set; }

        [Display(Name = "Присутствовал")]
        public bool IsPresent { get; set; }
    }
}