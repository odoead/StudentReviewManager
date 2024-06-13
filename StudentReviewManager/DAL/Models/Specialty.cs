using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentReviewManager.DAL.Models
{
    [Table("Specialties")]
    public class Specialty
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
