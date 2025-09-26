using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TheBooksNook.Models;

namespace TheBooksNook.Controllers;

public class BookApiController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    [HttpGet("apiBooks")]
    public async Task<IActionResult> GetBook()
    {
        // Random book IDs
        string[] authors =
        {
            "fswbNQEACAAJ",
            "AAjjRgAACAAJ",
            "at4lQwAACAAJ",
            "rpLxEAAAQBAJ",
            "C8NVhWNU_uIC",
            "oZRlYDfxXpQC",
            "Ak11sHG2uWIC",
            "NRWlitmahXkC",
            "5P9QEQAAQBAJ",
            "GfcIEAAAQBAJ",
        };

        // Create a Random object to get random index
        Random rand = new();
        int index = rand.Next(authors.Length);

        try
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.GetAsync(
                $"https://www.googleapis.com/books/v1/volumes/{authors[index]}?key=AIzaSyCvb7yPRUxdI5Bqyv5NP6aWV3-i7jQ5-Og"
            );
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var book = JsonSerializer.Deserialize<BookApi>(jsonString);

            return View(book);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching the book: {ex.Message}");
        }
    }
}
