using Microsoft.AspNetCore.Mvc;
using MinimalApisCRUD.Context;
using MinimalApisCRUD.Models;
using MinimalApisCRUD.Services.Interfaces;

namespace MinimalApisCRUD.Services
{
    public class BookService : IBookService
    {

        private readonly ApiContext _context;

        public BookService(ApiContext context)
        {
            _context = context;
        }

        public async Task<Book> CrearLibro(BookRequest request)
        {
            var book = new Book
            {
                Name = request.Name ?? string.Empty,
                ISBN = request.Isbn ?? string.Empty
            };

            var createBook = await _context.BookEntity.AddAsync(book);

            await _context.SaveChangesAsync();

            return createBook.Entity;
        }


    }
}
