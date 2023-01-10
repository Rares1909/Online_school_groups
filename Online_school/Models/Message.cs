
using System.ComponentModel.DataAnnotations;

namespace Online_school.Models
{
    public class Message
    {   [Key]
        public int MessageId { get; set; } 
        [Required(ErrorMessage ="Continutul mesajului este obligatoriu")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int? GroupId { get; set; }

        public string? UserId { get; set; } 

        public virtual Group? Group { get; set; }
 
        public virtual ApplicationUser? User { get; set; }
    }
}
