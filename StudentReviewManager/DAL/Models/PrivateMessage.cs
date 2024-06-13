/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentReviewManager.DAL.Models
{
    public class privateMessage
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("SenderId")]
        public User Sender { get; set; }
        public string SenderId { get; set; }
        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }
        public string ReceiverId { get; set; }
    }
}
*/
