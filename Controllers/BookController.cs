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
    public IActionResult AllBooks()
    {
        return View();
    }

    [HttpGet("new")]
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
}
