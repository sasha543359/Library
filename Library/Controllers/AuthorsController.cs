using LibraryData.Models;
using LibraryServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _authorService.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        var author = await _authorService.GetAuthorByIdAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] Author author)
    {
        if (author == null)
        {
            return BadRequest("Author data is invalid.");
        }

        var createdAuthor = await _authorService.CreateAuthorAsync(author);
        return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedAuthor = await _authorService.UpdateAuthorAsync(id, author);
        if (updatedAuthor == null)
        {
            return NotFound();
        }

        return Ok(updatedAuthor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var result = await _authorService.DeleteAuthorAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
