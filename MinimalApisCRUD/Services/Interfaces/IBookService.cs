using MinimalApisCRUD.Models;

namespace MinimalApisCRUD.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CrearLibro(BookRequest request);
    }
}
