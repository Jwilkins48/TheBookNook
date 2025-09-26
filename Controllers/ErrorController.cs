using Microsoft.AspNetCore.Mvc;

namespace TheBooksNook.Controllers;

public class ErrorController : Controller
{
    [HttpGet("error/{code}")]
    public IActionResult Handle(int code)
    {
        if (code == 404)
        {
            return View("PageNotFound");
        }
        else if (code == 401)
        {
            return View("Unauthorized");
        }
        else if (code == 403)
        {
            return View("Forbidden");
        }

        return View("ServerError");
    }

    [HttpGet("error/boom")]
    public IActionResult Boom()
    {
        return new StatusCodeResult(500);
    }

    [HttpGet("error/forbid")]
    public IActionResult Forbidden()
    {
        return new StatusCodeResult(403);
    }
}
