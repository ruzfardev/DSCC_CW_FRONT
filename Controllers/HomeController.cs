using DSCC_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace DSCC_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly string baseUrl = "http://localhost:7777/api/";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> books = new List<Book>();

            using (var client = new HttpClient())
            {
                // Set the base address for your API
                client.BaseAddress = new Uri(baseUrl);

                // Clear the default request headers
                client.DefaultRequestHeaders.Clear();

                // Send an HTTP GET request to the "Book" endpoint
                HttpResponseMessage response = await client.GetAsync("Book");

                if (response.IsSuccessStatusCode)
                {
                    // Read the content as JSON and deserialize it into a List<Book>
                    books = await response.Content.ReadFromJsonAsync<List<Book>>();
                }
            }

            // Pass the list of books to the view
            return View(books);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}