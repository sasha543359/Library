using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The category name is required.")]
    [StringLength(50, ErrorMessage = "The category name cannot exceed 50 characters.")]
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}