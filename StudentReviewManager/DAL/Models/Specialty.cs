using System.ComponentModel.DataAnnotations;

namespace StudentReviewManager.DAL.Models
{
    public class Specialty
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
