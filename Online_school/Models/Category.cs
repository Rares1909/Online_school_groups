using System.ComponentModel.DataAnnotations;

namespace Online_school.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name of the Category")]
        public string? Category_Name { get; set; }

        public virtual ICollection<Group>? Groups { get; set; }
    }
}
