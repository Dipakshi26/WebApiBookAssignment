using Api3Assignment.Data;
using Api3Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api3Assignment.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private DemoDbContext _context;

        public BookController(DemoDbContext demoDbContext)
        {
            _context = demoDbContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var bookDetails = _context.BookDetails.ToList();
            _context.SaveChanges();
            if (bookDetails != null)
            {
                string jsondata = JsonConvert.SerializeObject(bookDetails);
                return Ok(jsondata);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetBookDetails(int Id)
            {
                try
                {
                    var books = _context.BookDetails.Find(Id);
                    _context.SaveChanges();
                    var data = JsonConvert.SerializeObject(books);
                    return Ok(data);

                }
                catch
                {
                    return NotFound();

                }

                }
            

            // GET: EmployeesController/Create
            [HttpPost]
            public ActionResult CreateBook(Book book)
            {
                if (book == null)
                {
                    return BadRequest();
                }
                else
                {
                    _context.BookDetails.Add(book);
                    _context.SaveChanges();
                    return Ok("Created Successfully");
                }
            }


                [HttpDelete("{id}")]
                public ActionResult DeleteBook(int id)
            {
                if (id <= 0)
                {
                    return NotFound();
                }
                else
                {
                    var delBook = _context.BookDetails.Find(id);
                    _context.BookDetails.Remove(delBook);
                    _context.SaveChanges();
                    return Ok("Deleted Successfully");
                }

            }

            [HttpPut("{id}")]
            public async  Task<IActionResult> Edit(Book book)
            {
            _context.Entry(book).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok("Updated");
            

            }

        //        [HttpGet]
        //        public IActionResult Search(string Query)
        //        {
        //            var querystring = _context.BookDetails.Where(b => b.Name == Query || b.Zoner == Query).ToList();
        //            var json = JsonConvert.SerializeObject(querystring);
        //            return Ok(json);
        //        }

        [HttpGet("{searchByNameOrZoner}")]
        public async Task<ActionResult<IEnumerable<Book>>> Search(string searchByNameOrZoner)
        {

            if (searchByNameOrZoner == null)
                return NotFound();
            var books = _context.BookDetails.Where(e => e.Name.Contains(searchByNameOrZoner) || e.Zoner.Contains(searchByNameOrZoner)).ToListAsync();
            if (books != null)
                return await books;
            return NotFound();
        }
    }
}
