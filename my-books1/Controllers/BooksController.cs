using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books1.Data;
using my_books1.Data.Models;
using my_books1.Data.Services;
using my_books1.Data.ViewModels;

namespace my_books1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        public BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var allBooks = _booksService.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("get-book-by-id/{bookId}")]
        public IActionResult GetBookById(int bookId)
        {
            var book = _booksService.GetBookById(bookId);
            return Ok(book);
        }

        [HttpPost("add-book-with-authors")]
        public IActionResult AddBook([FromBody]BookVM book)
        {
            _booksService.AddBookWithAuthors(book);
            return Ok();
        }

        [HttpPut("update-book-by-id/{bookId}")]
        public IActionResult UpdateBookById(int bookId, [FromBody]BookVM book)
        {
            var updatedBook = _booksService.UpdateBookById(bookId, book);
            return Ok(updatedBook);
        }

        [HttpDelete("delete-book-by-id/{bookId}")]
        public IActionResult DeleteBookById(int bookId)
        {
            _booksService.DeleteBookById(bookId);
            return Ok();
        }

    }
}
