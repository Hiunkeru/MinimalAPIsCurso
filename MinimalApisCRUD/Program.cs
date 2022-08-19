using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalApisCRUD.Context;
using MinimalApisCRUD.Models;
using MinimalApisCRUD.Services;
using MinimalApisCRUD.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BookConnection") ?? "Data Source=Book.db";

builder.Services.AddDbContext<ApiContext>(opt => opt.UseSqlite(connectionString));

//builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ejemplo minimal APIS", Description = "Codigo de ejemplo"});
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.MapGet("/api/books", async (ApiContext context) => Results.Ok(await context.BookEntity.ToListAsync()));

app.MapGet("/api/book/{id}", async (int id, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);

    if (book != null)
    {
        Results.Ok(book);
    }

    Results.NotFound();
});

app.MapPost("/api/book", async (BookRequest request, IBookService bookService) =>
{
    var createBook = await bookService.CrearLibro(request);

    return Results.Created($"/books/{createBook.Id}", createBook);
});

app.MapDelete("/api/book/{id}", async (int id, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    context.BookEntity.Remove(book);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/api/book", async (int id, BookRequest request, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    if (request.Name != null)
    {
        book.Name = request.Name;
    }

    if (request.Isbn != null)
    {
        book.ISBN = request.Isbn;
    }

    await context.SaveChangesAsync();
    return Results.Ok(book);

});


app.Run();
