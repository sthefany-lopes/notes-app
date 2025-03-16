using System.ComponentModel.DataAnnotations;

namespace Notes.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title field is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The title field must be between {2} and {1} characters in length.")]
        public string Title { get; set; }

        [StringLength(10000, ErrorMessage = "The content field must be at most {1} characters in length.")]
        public string? Content { get; set; }

        [Display(Name = "Created At")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public Category? Category { get; set; }

        public Note()
        {
        }

        public Note(int id, string title, string? content, Category category)
        {
            Id = id;
            Title = title;
            Content = content;
            Category = category;
        }
    }
}