namespace MatureAPI.Rest.Models;

public static class BookSeeder
{
    public static List<Book> BookList = [
        new Book(1, "Angels and Demons", "Dan Brown", 15),
        new Book(2, "Sapeins: A Brief History of Mankind", "Yuval Noah Harari", 25),
        new Book(3, "Harry Poter and the Prisoner of Azkaban", "J. K. Rowling", 18)
    ];
}