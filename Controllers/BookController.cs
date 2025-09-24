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
    public async Task<IActionResult> AllBooks(string? genre)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Filter by Genre
        var filteredBook = _context.Books.Select(b => b);
        if (!string.IsNullOrEmpty(genre))
        {
            if (
                genre.Equals("fantasy", StringComparison.CurrentCultureIgnoreCase)
                || genre.Equals("sci-fi", StringComparison.CurrentCultureIgnoreCase)
                || genre.Equals("fiction", StringComparison.CurrentCultureIgnoreCase)
                || genre.Equals("nonfiction", StringComparison.CurrentCultureIgnoreCase)
                || genre.Equals("horror", StringComparison.CurrentCultureIgnoreCase)
            )
                filteredBook = _context.Books.Select(b => b).Where(b => b.Genre.ToLower() == genre);
        }

        // Format book based on filter
        var BookCards = await filteredBook
            .AsNoTracking()
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

        //

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
                Comment = book.Comments.Select((c) => c.CommentContext).ToList(),
                CommentFormViewModel = new() { BookId = book.Id },
            })
            .FirstOrDefaultAsync();

        return View(vm);
    }

    [HttpPost("{id}/comment")] // Book Comment Post
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookComment(int id, CommentFormViewModel vm)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // If invalid - return book details
        if (!ModelState.IsValid)
        {
            var book = await _context.Books.Where((book) => book.Id == id).FirstOrDefaultAsync();
            if (book is null)
                return NotFound();

            var viewModel = await _context
                .Books.Where(book => book.Id == id)
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
                    Comment = book.Comments.Select((c) => c.CommentContext).ToList(),
                    CommentFormViewModel = vm,
                })
                .FirstOrDefaultAsync();

            return View("BookDetails", viewModel);
        }

        var newComment = new Comment
        {
            CommentContext = vm.CommentContext,
            UserId = uid,
            BookId = id,
        };
        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(BookDetails), new { id });
    }

    [HttpGet("{id}/edit")] // Edit Book View
    public async Task<IActionResult> EditBook(int id)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Check if book exists
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
            return NotFound();

        // Only edit if users book
        if (uid != book.UserId)
            return new StatusCodeResult(403);

        // Fill form with book info
        var vm = new BookFormViewModel
        {
            Id = book.Id,
            BookTitle = book.BookTitle,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear,
            Description = book.Description,
        };
        return View(vm);
    }

    [HttpPost("{id}/update")] // Edit book post
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBookPost(int id, BookFormViewModel vm)
    {
        // Normalize inputs
        vm.BookTitle = (vm.BookTitle ?? "").Trim();
        vm.Author = (vm.Author ?? "").Trim();
        vm.Genre = (vm.Genre ?? "").Trim();
        vm.Description = (vm.Description ?? "").Trim();

        // Basic Validation
        if (!ModelState.IsValid)
            return View(nameof(EditBook), vm);

        // Id matches request id
        if (id != vm.Id)
            return BadRequest();

        // Check if book exists
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
            return NotFound();

        // Update data
        book.BookTitle = vm.BookTitle;
        book.Author = vm.Author;
        book.Genre = vm.Genre;
        book.PublishedYear = vm.PublishedYear;
        book.Description = vm.Description;

        // Save to db
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(AllBooks), new { id });
    }

    [HttpGet("{id}/delete")] // Delete Book View
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Check if book exists
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
            return NotFound();

        // Only delete if users book
        if (uid != book.UserId)
            return new StatusCodeResult(403);

        var vm = new ConfirmDeleteViewModel { Id = book.Id, BookTitle = book.BookTitle };
        return View(vm);
    }

    [HttpPost("{id}/delete")] // Delete Book Post
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBook(int id, ConfirmDeleteViewModel vm)
    {
        // Check if user is logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is not int uid)
            return RedirectToAction("LoginForm", "Account", new { error = "not-authenticated" });

        // Id matches request id
        if (id != vm.Id)
            return BadRequest();

        // Check if book exists
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
        if (book is null)
            return NotFound();

        // Update db
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(AllBooks));
    }
}
