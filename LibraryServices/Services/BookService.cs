using LibraryData;
using LibraryData.Models;
using LibraryServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices.Services;

public class BookService : IBookService
{
    private readonly LibraryContext _context;
    private readonly IAuthorService _authorService;
    private readonly ICategoryService _categoryService;

    public BookService(LibraryContext context, IAuthorService authorService, ICategoryService categoryService)
    {
        _context = context;
        _authorService = authorService;
        _categoryService = categoryService;
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        // Проверка автора
        if (book.AuthorId.HasValue && book.AuthorId > 0)
        {
            // Привязываем к существующему автору по AuthorId
            book.Author = await _authorService.GetAuthorByIdAsync(book.AuthorId.Value);
            if (book.Author == null)
            {
                throw new ArgumentException("Specified AuthorId does not exist.");
            }
        }
        else if (book.Author != null && string.IsNullOrEmpty(book.Author.Name) == false)
        {
            // Если объект Author передан, создаем нового автора
            var createdAuthor = await _authorService.CreateAuthorAsync(book.Author);
            book.AuthorId = createdAuthor.Id;
        }
        else
        {
            throw new ArgumentException("Either AuthorId or a valid Author object is required.");
        }

        // Проверка категории
        if (book.CategoryId.HasValue && book.CategoryId > 0)
        {
            // Привязываем к существующей категории по CategoryId
            book.Category = await _categoryService.GetCategoryByIdAsync(book.CategoryId.Value);
            if (book.Category == null)
            {
                throw new ArgumentException("Specified CategoryId does not exist.");
            }
        }
        else if (book.Category != null && string.IsNullOrEmpty(book.Category.Name) == false)
        {
            // Если объект Category передан, создаем новую категорию
            var createdCategory = await _categoryService.CreateCategoryAsync(book.Category);
            book.CategoryId = createdCategory.Id;
        }
        else
        {
            throw new ArgumentException("Either CategoryId or a valid Category object is required.");
        }

        // Добавляем книгу в базу данных
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await _context.Books.Include(b => b.Author).Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> UpdateBookAsync(int id, Book updatedBook)
    {
        var existingBook = await _context.Books.Include(b => b.Author).Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existingBook == null)
        {
            return null;
        }

        existingBook.Title = updatedBook.Title;
        existingBook.ISBN = updatedBook.ISBN;

        if (updatedBook.AuthorId.HasValue && updatedBook.AuthorId > 0)
        {
            existingBook.AuthorId = updatedBook.AuthorId;
            existingBook.Author = await _context.Authors.FindAsync(updatedBook.AuthorId);
        }
        else if (updatedBook.Author != null && !string.IsNullOrEmpty(updatedBook.Author.Name))
        {
            var createdAuthor = await _authorService.CreateAuthorAsync(updatedBook.Author);
            existingBook.AuthorId = createdAuthor.Id;
            existingBook.Author = createdAuthor;
        }

        if (updatedBook.CategoryId.HasValue && updatedBook.CategoryId > 0)
        {
            existingBook.CategoryId = updatedBook.CategoryId;
            existingBook.Category = await _context.Categories.FindAsync(updatedBook.CategoryId);
        }
        else if (updatedBook.Category != null && !string.IsNullOrEmpty(updatedBook.Category.Name))
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(updatedBook.Category);
            existingBook.CategoryId = createdCategory.Id;
            existingBook.Category = createdCategory;
        }

        await _context.SaveChangesAsync();
        return existingBook;
    }

}