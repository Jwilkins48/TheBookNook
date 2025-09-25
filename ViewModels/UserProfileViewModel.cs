using TheBooksNook.Models;

namespace TheBooksNook.ViewModels;

public class UserProfileViewModel
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public int BooksAddedCount { get; set; }
    public int BooksCommentedOnCount { get; set; }
    public List<Book> BookNames { get; set; } = [];
}
