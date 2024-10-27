using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models;

public class Author
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The author's name is required.")]
    [StringLength(50, ErrorMessage = "The author's name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "The biography cannot exceed 500 characters.")]
    public string Biography { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}