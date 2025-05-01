using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocial.Domain.Entities
{
    [Table("Messages")]
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("SenderId")]
        public string SenderId { get; set; }
        [ForeignKey("ReceiverId")]
        public string ReceiverId { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
