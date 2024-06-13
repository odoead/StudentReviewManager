using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentReviewManager.DAL.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsArchived { get; set; }

        [Range(0, 5, ErrorMessage = "Rating range could be between 0-5")]
        public int Rating { get; set; }
        public bool IsAuthorized { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public string UserId { get; set; }

        [ForeignKey("SchoolId")]
        public School? School { get; set; }
        public int? SchoolId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        public int? CourseId { get; set; }
    }
}
