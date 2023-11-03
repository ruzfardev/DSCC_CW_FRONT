using DSCC_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;

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
        public async Task<IActionResult> BookDetail(int id)
        {
            Book book = new Book();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response = await client.GetAsync($"Book/{id}");

                if (response.IsSuccessStatusCode)
                {
                    book = await response.Content.ReadFromJsonAsync<Book>();
                }
            }
            return View(book);
        }
        
 
        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();
                    HttpResponseMessage response = await client.PostAsJsonAsync("Book", book);

                    if (response.IsSuccessStatusCode)
                    {
                        // Retrieve the created book from the response
                        var newBook = await response.Content.ReadFromJsonAsync<Book>();

                        // Redirect to the "BookDetail" action with the new book's ID
                        return RedirectToAction("BookDetail", new { id = newBook.Id });
                    }
                }
            }
            return View(book);
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response = await client.DeleteAsync($"Book/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            // Handle errors or invalid model state here
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBook(int id)
        {
            Book book = new Book();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response = await client.GetAsync($"Book/{id}");

                if (response.IsSuccessStatusCode)
                {
                    book = await response.Content.ReadFromJsonAsync<Book>();
                }
            }

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();

                    HttpResponseMessage response = await client.PutAsJsonAsync($"Book/{book.Id}", book);

                    if (response.IsSuccessStatusCode)
                    {
                        // Redirect to the "BookDetail" action after successful update
                        return RedirectToAction("BookDetail", new { id = book.Id });
                    }
                }
            }

            return View(book);
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