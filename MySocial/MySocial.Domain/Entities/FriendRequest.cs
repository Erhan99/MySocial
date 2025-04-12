using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocial.Domain.Entities
{
    [Table("FriendRequests")]
    public class FriendRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [ForeignKey("SenderId")]
        public int SenderId { get; set; }
        [ForeignKey("ReceiverId")]
        public int ReceiverId { get; set; }
        public string Status { get; set; } 
        public bool isSeen { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
