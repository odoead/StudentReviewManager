using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentReviewManager.DAL.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        //public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
