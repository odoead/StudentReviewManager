using System.ComponentModel.DataAnnotations;

namespace StudentReviewManager.DAL.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<School> Schools { get; set; }
    }
}
