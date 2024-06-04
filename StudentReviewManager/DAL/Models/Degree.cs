using System.ComponentModel.DataAnnotations;

namespace StudentReviewManager.DAL.Models
{
    public class Degree
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
