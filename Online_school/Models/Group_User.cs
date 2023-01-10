using System.ComponentModel.DataAnnotations.Schema;

namespace Online_school.Models
{
    public class Group_User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string UserId { get; set; }
        public int GroupId { get; set; }

        public virtual Group? Group { get; set; }

        public virtual ApplicationUser User { get; set; }
        public bool moderator { get; set; }
    }
}
