using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_school.Models
{
    public class Group
    {   
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Name of the group")]
        public string Group_Name { get; set; }

        public DateTime Created { get; set; }


        public string? Description { get; set; }

        public virtual ICollection<Group_User>? Users { get; set; }

        [Required(ErrorMessage = "Category was not selected")]
        public int? CategoryId { get; set; }  
        public virtual Category? Category { get; set; }

        //public bool moderator { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }


    }
}
