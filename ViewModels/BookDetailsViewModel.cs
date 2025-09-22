namespace TheBooksNook.ViewModels;

public class BookDetailsViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BookTitle { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";
    public int PublishedYear { get; set; }
    public string Description { get; set; } = "";
    public string ReaderUsername { get; set; } = "";

    // Comments
    public CommentFormViewModel? CommentFormViewModel { get; set; }
    public List<string> Comment { get; set; } = [];
}
