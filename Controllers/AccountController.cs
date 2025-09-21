using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBooksNook.Models;
using TheBooksNook.Services;
using TheBooksNook.ViewModels;

namespace TheBooksNook.Controllers;

[Route("/account")]
public class AccountController(ApplicationContext context, IPasswordService passwords) : Controller
{
    private readonly ApplicationContext _context = context;
    private readonly IPasswordService _passwords = passwords;
    public const string SessionUserId = "userId";

    [HttpGet("register")] // Register User View
    public IActionResult RegisterForm()
    {
        var vm = new RegisterFormViewModel();
        return View(vm);
    }

    [HttpPost("register")] // Register User Post
    [ValidateAntiForgeryToken]
    public IActionResult ProcessRegisterForm(RegisterFormViewModel vm)
    {
        // Normalize
        vm.Username = (vm.Username ?? "").Trim().ToLowerInvariant();
        vm.Email = (vm.Email ?? "").Trim().ToLowerInvariant();
        vm.Password = (vm.Password ?? "").Trim();
        vm.ConfirmPassword = (vm.ConfirmPassword ?? "").Trim();

        // Check if model is valid
        if (!ModelState.IsValid)
        {
            // if not - return to register
            return View(nameof(RegisterForm), vm);
        }

        // Check if email exists - show error if not
        bool emailExists = _context.Users.Any(user => user.Email == vm.Email);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email already exits");
            return View(nameof(RegisterForm), vm);
        }

        // Hash password
        var hashedPassword = _passwords.Hash(vm.Password);

        // Create and save new user
        var newUser = new User
        {
            Username = vm.Username,
            Email = vm.Email,
            PasswordHash = hashedPassword,
        };

        // Save to db
        _context.Users.Add(newUser);
        _context.SaveChanges();

        // Start sesion
        HttpContext.Session.SetInt32(SessionUserId, newUser.Id);
        return RedirectToAction("AllBooks", "Book");
    }

    [HttpGet("login")] // User Login View
    public IActionResult LoginForm(string? error)
    {
        var vm = new LoginFormViewModel { Error = error };
        return View(vm);
    }

    [HttpPost("login")] // User Login Post
    [ValidateAntiForgeryToken]
    public IActionResult ProcessLoginForm(LoginFormViewModel vm)
    {
        // Normalize
        vm.Email = (vm.Email ?? "").Trim().ToLowerInvariant();
        vm.Password = (vm.Password ?? "").Trim();

        // If invalid input
        if (!ModelState.IsValid)
        {
            return View(nameof(LoginForm), vm);
        }

        // find user in db - if none throw error
        if (!_context.Users.Any(user => user.Email == vm.Email))
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View(nameof(LoginForm), vm);
        }

        // Check if user email exists
        var user = _context.Users.FirstOrDefault(user => user.Email == vm.Email);
        if (user is null)
            return Unauthorized();

        // Check passwords
        if (!_passwords.Verify(vm.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View(nameof(LoginForm), vm);
        }

        // Set session
        HttpContext.Session.SetInt32(SessionUserId, user.Id);
        return RedirectToAction("AllBooks", "Book");
    }

    [HttpGet("logout")] // User Logout View
    public IActionResult ConfirmLogout()
    {
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is null)
            return Unauthorized();

        return View();
    }

    [HttpPost("logout")] // User Logout Post
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // Clear user id from session
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(LoginForm), new { error = "logout-successful" });
    }

    [HttpGet("profile")] // User Profile View
    public IActionResult Profile()
    {
        // Check if signed in
        var userid = HttpContext.Session.GetInt32(SessionUserId);
        if (userid is not int uid)
            return Unauthorized();

        // Check if user exists
        var user = _context.Users.Where((u) => u.Id == uid).FirstOrDefault();
        if (user is null)
            return NotFound();

        // Project user data into vm
        var vm = _context
            .Users.AsNoTracking()
            .Where(user => user.Id == uid)
            .Select(user => new UserProfileViewModel
            {
                Username = user!.Username,
                Email = user.Email,
                BooksAddedCount = user.Books.Count,
                BooksCommentedOnCount = user.Comments.Count,
            })
            .FirstOrDefault();

        return View(vm);
    }
}
