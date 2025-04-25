using System.ComponentModel.DataAnnotations;

namespace aspnetcore.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public required string Name { get; set; }

        public string FullName => Name;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}