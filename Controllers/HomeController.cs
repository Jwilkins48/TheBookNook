using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TheBooksNook.Models;

namespace TheBooksNook.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private const string SessionUserId = "userId";

    [HttpGet("")]
    public IActionResult Index()
    {
        // Change buttons based on user logged in
        var userId = HttpContext.Session.GetInt32(SessionUserId);
        if (userId is null)
        {
            ViewData["Message"] = "No user!";
        }
        else
        {
            ViewData["Message"] = userId;
        }
        return View();
    }

    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
