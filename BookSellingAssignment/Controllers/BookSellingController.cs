using BookSellingAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;



namespace BookSellingAssignment.Controllers
{
    
    
    public class BookSellingController : Controller
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        Uri baseAddress = new Uri("https://localhost:7094/api");
        HttpClient client;

        
        public BookSellingController(IWebHostEnvironment webHostEnvironment)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public ActionResult AddBookDetails(BookViewModel book)
        {
            if (book.Image != null)
            {
                string folder = "Book/Images/";
                folder += Guid.NewGuid().ToString() + "_" + book.Image.FileName;
                book.ImageUrl = folder;
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                book.Image.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }

            var postTask = client.PostAsJsonAsync<BookViewModel>(baseAddress + "/Book/CreateBook", book);
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View("book");
        }


        public ActionResult Index()
        {
            List<BookViewModel>? bookList = new List<BookViewModel>();

            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/Book/Index").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string bookData = responseMessage.Content.ReadAsStringAsync().Result;
                bookList = JsonConvert.DeserializeObject<List<BookViewModel>>(bookData);
                return View(bookList);

            }
            ModelState.AddModelError(string.Empty, "Server Error");
            return View();
        }

    

        public IActionResult CreateNewBooks()
        {
            return View();
        }

        public ActionResult DeleteBook(int id)
        {

            //HTTP DELETE
            var deleteTask = client.DeleteAsync(baseAddress + "/book/DeleteBook/" + id);
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }



        public ActionResult UpdateBookDetails(int id)
        {
            BookViewModel? book = new();

            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/book/GetBookDetails/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookViewModel>(data);
            }
            return View(book);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBookDetails(BookViewModel book)
        {

            var putTask = client.PutAsJsonAsync<BookViewModel>(baseAddress + "/book/Edit/" + book.Id.ToString(), book);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(book);
        }


        public ActionResult Details(int id)
        {
            BookViewModel? book = new ();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/book/GetBookDetails/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<BookViewModel>(data);
            }
            return View(book);
        }
        [HttpGet]
        public ActionResult SearchBook(string searchByNameOrZoner)
        {
            ViewData["BookInfo"] = searchByNameOrZoner;
            List<BookViewModel>? bookList = new List<BookViewModel>();
            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + $"/book/Search/{searchByNameOrZoner}").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                bookList = JsonConvert.DeserializeObject<List<BookViewModel>>(data);
            }
            return View("Index", bookList);
        }
        public static List<BookViewModel>? cartList = new List<BookViewModel>();

        public ActionResult Cartlist()
        {
            return View("AddToCart", cartList);
        }
        public ActionResult AddToCart(int id)
        {

            HttpResponseMessage responseMessage = client.GetAsync(baseAddress + "/book/GetBookDetails/" + id.ToString()).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                var book = JsonConvert.DeserializeObject<BookViewModel>(data);
                if (book != null)
                    cartList.Add(book);
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                    return View("AddToCart");
                }

            }
            return View("AddToCart", cartList);

        }
    }
}
