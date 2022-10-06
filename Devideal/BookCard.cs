namespace Devideal;

public record BookCard(string Book, Borrower Borrower, DateTime BorrowDate, DateTime ReturnDate)
{
    public readonly string Book = Book;
}