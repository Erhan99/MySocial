using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocial.Domain.Entities
{
    [Table("FriendRequests")]
    public class FriendRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("SenderId")]
        public string SenderId { get; set; }
        [ForeignKey("ReceiverId")]
        public string ReceiverId { get; set; }
        public string Status { get; set; }
        public bool isSeen { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
