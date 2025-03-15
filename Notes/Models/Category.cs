using System.ComponentModel.DataAnnotations;

namespace Notes.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The {0} field must be between {2} and {1} characters in length.")]
        public string Name { get; set; }

        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public Category()
        {
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}