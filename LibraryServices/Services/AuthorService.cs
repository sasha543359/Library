using LibraryData;
using LibraryData.Models;
using LibraryServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices.Services;

public class AuthorService : IAuthorService
{
    private readonly LibraryContext _context;

    public AuthorService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> CreateAuthorAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAuthorAsync(int id, Author updatedAuthor)
    {
        var existingAuthor = await _context.Authors.FindAsync(id);
        if (existingAuthor == null)
        {
            return null;
        }

        existingAuthor.Name = updatedAuthor.Name;
        existingAuthor.Biography = updatedAuthor.Biography;

        await _context.SaveChangesAsync();
        return existingAuthor;
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return false;
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return true;
    }
}
