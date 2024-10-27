using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryData.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The title of the book is required.")]
    [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "ISBN is required.")]
    [RegularExpression(@"\d{3}-\d{2}-\d{6}-\d{2}-\d{1}", ErrorMessage = "ISBN must follow the correct format.")]
    public string ISBN { get; set; }

    [JsonPropertyName("authorId")]
    public int? AuthorId { get; set; }

    [JsonPropertyName("author")] // Удалите [JsonIgnore] временно
    public Author? Author { get; set; }

    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }

    [JsonPropertyName("category")] // Удалите [JsonIgnore] временно
    public Category? Category { get; set; }
}