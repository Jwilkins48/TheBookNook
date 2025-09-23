using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using TheBooksNook.Models;

namespace TheBooksNook.Controllers;

public class BookApiController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

    [HttpGet("apiBooks")]
    public async Task<IActionResult> GetBook()
    {
        try
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.GetAsync(
                "https://www.googleapis.com/books/v1/volumes/zyTCAlFPjgYC?key=AIzaSyCvb7yPRUxdI5Bqyv5NP6aWV3-i7jQ5-Og"
            );
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var book = JsonSerializer.Deserialize<BookApi>(jsonString);

            Console.WriteLine("*******************************************************");
            Console.WriteLine(book);

            return View(book);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while fetching the joke: {ex.Message}");
        }
    }
}
