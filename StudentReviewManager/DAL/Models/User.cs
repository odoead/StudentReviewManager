using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StudentReviewManager.DAL.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        //public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
