using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBooksNook.Models;
using TheBooksNook.ViewModels;

namespace TheBooksNook.Controllers;

[Route("books/")]
public class BookController(ApplicationContext context) : Controller
{
    private readonly ApplicationContext _context = context;
    private const string SessionUserId = "userId";

    [HttpGet("")] // All Books View
    public async Task<IActionResult> AllBooks()
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Format Book
        var BookCards = await _context
            .Books.AsNoTracking()
            .Select(book => new BookCardViewModel
            {
                Id = book.Id,
                BookTitle = book.BookTitle,
                Author = book.Author,
                Genre = book.Genre,
                PublishedYear = book.PublishedYear,
                ReaderUsername = book.User!.Username,
                CreatedAt = book.CreatedAt,
                UserId = book.User.Id,
            })
            .OrderByDescending(book => book.CreatedAt)
            .ToListAsync();

        // Add books into list
        var vm = new BookIndexViewModel { AllBooks = BookCards };
        return View(vm);
    }

    [HttpGet("new")] // New Book View
    public IActionResult AddBook()
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is null)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        var vm = new BookFormViewModel();
        return View(vm);
    }

    [HttpPost("new/post")] // New Book Post
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBookPost(BookFormViewModel vm)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Normalize inputs
        vm.BookTitle = (vm.BookTitle ?? "").Trim();
        vm.Author = (vm.Author ?? "").Trim();
        vm.Genre = (vm.Genre ?? "").Trim();
        vm.Description = (vm.Description ?? "").Trim();

        // Check Input Validations
        if (!ModelState.IsValid)
            return View(nameof(AddBook), vm);

        // Create Book
        var newBook = new Book
        {
            UserId = uid,
            BookTitle = vm.BookTitle,
            Author = vm.Author!,
            Genre = vm.Genre,
            PublishedYear = vm.PublishedYear,
            Description = vm.Description,
            // BookImageSrc = vm.BookImageSrc,
        };

        // Save to db
        await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(AllBooks));
    }

    [HttpGet("{id}")] // Book Details View
    public async Task<IActionResult> BookDetails(int id)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Project data into view model
        var vm = await _context
            .Books.AsNoTracking()
            .Where(book => book.Id == id)
            .Select(book => new BookDetailsViewModel
            {
                Id = book.Id,
                UserId = book.UserId,
                BookTitle = book.BookTitle,
                Author = book.Author,
                Genre = book.Genre,
                PublishedYear = book.PublishedYear,
                Description = book.Description,
                ReaderUsername = book.User!.Username,
                // CommentFormViewModel = new() { BookId = book.Id },
            })
            .FirstOrDefaultAsync();

        return View(vm);
    }
}
