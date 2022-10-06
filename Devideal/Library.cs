using System.Text.RegularExpressions;

namespace Devideal;

public class Library
{
    private List<BookCard> _bookCards;
    private readonly Dictionary<string, int> _bookCategoriesPenalties;

    private readonly string[] _bookCategories = 
    {
        "IT",
        "History",
        "Classics",
        "Law",
        "Medical",
        "Philosophy"
    };
    public Library()
    {
        _bookCards = new List<BookCard>();
        _bookCategoriesPenalties = new Dictionary<string, int>
        {
            {"IT", 5},
            {"History", 3},
            {"Classics", 2},
            {"Law", 2},
            {"Medical", 2},
            {"Philosophy", 2}
        };
    }

    public void Run()
    {
        PrintAvailableBooks();
        var borrower = new Borrower(new List<string>());

        var bookNumber = ChooseBookCategory();
        if (bookNumber == -1) return;
        
        Console.WriteLine("Please write the day, month, year of the borrow");
        var borrowDate = ProvideDate();
        Console.WriteLine("Please write the day, month, year of the return");
        var returnDate = ProvideDate();
        
        var diffDates = DateTime.Compare(returnDate, borrowDate);
        if (diffDates < 0)
        {
            Console.WriteLine("Return date can't be earlier than borrow date");
            return;
        }
        
        var book = _bookCategories[bookNumber - 1];
        borrower.Books.Add(book);
        var bookCard = new BookCard(book, borrower, borrowDate, returnDate);
        
        _bookCards.Add(bookCard);

        var fee = CalculateBorrowPenalty(borrower);
        Console.WriteLine(fee != 0 ? $"Borrower fee is: {fee}" : "Borrower has no fee to pay");
    }

    private void PrintAvailableBooks()
    {
        var str = "";
        var counter = 1;
        foreach (var bookCategory in _bookCategoriesPenalties)
        {
            str += $" [{counter}] {bookCategory.Key},";
            counter++;
        }

        str = str.TrimEnd(',');
        Console.WriteLine($"Please pick a book category:{str}");
    }

    private int ChooseBookCategory()
    {
        var key = Console.ReadLine();
        try
        {
            if (!Regex.Match(key, "^[1-9]\\d*$").Success)
            {
                Console.WriteLine("Incorrect format: only accepts numbers");
            }

            var value = int.Parse(key);
            if (value <= _bookCategoriesPenalties.Count) return value;
            Console.WriteLine("Number you provided is too large");
            return -1;
        }
        catch (FormatException _)
        {
            Console.WriteLine("Incorrect format: does not accept whitespaces ");
        }
        catch (OverflowException _)
        {
            Console.WriteLine("Number you provided is too large");
        }

        return -1;
    }
    
    private DateTime ProvideDate()
    {
        var dateInput = Console.ReadLine();
        DateTime dateTime;
        try
        {
            dateTime = DateTime.Parse(dateInput);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return dateTime;
    }

    private int CalculateBorrowPenalty(Borrower borrower)
    {
        int fee = 0;
        foreach (var bookCard in _bookCards)
        {
            if (bookCard.Borrower == borrower)
            {
                var diffOfDates = bookCard.ReturnDate - bookCard.BorrowDate;
                fee += _bookCategoriesPenalties[bookCard.Book] * diffOfDates.Days;
            }
        }
        return fee;
    }
}