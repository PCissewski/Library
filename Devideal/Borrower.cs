namespace Devideal;

public record Borrower(List<string> Books)
{
    public List<string> Books { get; } = Books;
}