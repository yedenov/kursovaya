using System.ComponentModel.DataAnnotations;

namespace aspnetcore.Models
{
    public class Lesson
    {
        public Lesson()
        {
            Attendances = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Предмет' обязательно для заполнения")]
        [Display(Name = "Предмет")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Поле 'Дата' обязательно для заполнения")]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}