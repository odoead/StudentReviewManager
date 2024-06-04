using Microsoft.AspNetCore.Identity;

namespace StudentReviewManager.DAL.Models
{
    public class User : IdentityUser
    {
        public string Nickname {  get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
