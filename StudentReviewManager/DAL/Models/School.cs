using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentReviewManager.DAL.Models
{
    [Table("Schools")]
    public class School
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Review> Reviews { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
