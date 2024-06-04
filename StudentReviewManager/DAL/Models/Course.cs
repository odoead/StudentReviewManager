using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentReviewManager.DAL.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        [ForeignKey("SchoolId")]
        public School School { get; set; }
        public int SchoolId { get; set; }
        public ICollection<Review> Reviews { get; set; }

        [ForeignKey("DegreeId")]
        public Degree Degree { get; set; }
        public int DegreeId { get; set; }

        [ForeignKey("SpecialtyId")]
        public Specialty Specialty { get; set; }
        public int SpecialtyId { get; set; }
    }
}
