using MatureAPI.Rest.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapGet("/books", (HttpContext http) =>
{
    var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";

    var response = BookSeeder.BookList.Select(book => new
    {
        Book = book,
        Links = new List<Link>{
            new Link( "all-books", $"{baseUrl}/books", "GET" ),
            new Link ("self",$"{baseUrl}/books/{book.Id}", "GET"),
            new Link ("update", $"{baseUrl}/books/{book.Id}", "PUT" ),
            new Link ("delete", $"{baseUrl}/books/{book.Id}", "DELETE" )
        }
    });

    return Results.Ok(response);
});

app.MapGet("/books/{id:int}", (int id, HttpContext http) =>
{
    var book = BookSeeder.BookList.FirstOrDefault(b => b.Id == id);
    if (book is null) return Results.NotFound();

    var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";

    var response = new
    {
        Book = book,
        Links = new List<Link>{
            new Link( "all-books", $"{baseUrl}/books", "GET" ),
            new Link ("self",$"{baseUrl}/books/{book.Id}", "GET"),
            new Link ("update", $"{baseUrl}/books/{book.Id}", "PUT" ),
            new Link ("delete", $"{baseUrl}/books/{book.Id}", "DELETE" )
        }
    };

    return Results.Ok(response);
});

app.MapPost("/books", (Book newBook, HttpContext http) =>
{
    var id = BookSeeder.BookList.Max(b => b.Id) + 1;
    BookSeeder.BookList.Add(newBook with { Id = id });

    var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";

    var response = new
    {
        Book = newBook,
        Links = new List<Link>{
            new Link( "all-books", $"{baseUrl}/books", "GET" ),
            new Link ("self",$"{baseUrl}/books/{id}", "GET"),
            new Link ("update", $"{baseUrl}/books/{id}", "PUT" ),
            new Link ("delete", $"{baseUrl}/books/{id}", "DELETE" )
        }
    };

    return Results.Created($"/books/{newBook.Id}", response);
});

app.MapPut("/books/{id:int}", (int id, Book updatedBook, HttpContext http) =>
{
    var existing = BookSeeder.BookList.FirstOrDefault(b => b.Id == id);
    if (existing is null) return Results.NotFound();

    existing = existing with { Name = updatedBook.Name, Author = updatedBook.Author, Price = updatedBook.Price };

    var baseUrl = $"{http.Request.Scheme}://{http.Request.Host}";

    var response = new
    {
        data = existing,
        Links = new List<Link>{
            new Link( "all-books", $"{baseUrl}/books", "GET" ),
            new Link ("self",$"{baseUrl}/books/{id}", "GET"),
            new Link ("update", $"{baseUrl}/books/{id}", "PUT" ),
            new Link ("delete", $"{baseUrl}/books/{id}", "DELETE" )
        }
    };

    return Results.Ok(response);
});

app.MapDelete("/books/{id:int}", (int id) =>
{
    var existing = BookSeeder.BookList.FirstOrDefault(b => b.Id == id);
    if (existing is null) return Results.NotFound();

    BookSeeder.BookList.Remove(existing);
    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.Run();