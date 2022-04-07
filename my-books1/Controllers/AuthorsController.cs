using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books1.Data.Services;
using my_books1.Data.ViewModels;

namespace my_books1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorsService _authorsService;
        public AuthorsController(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }


        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AuthorVM author)
        {
            _authorsService.AddAuthor(author);
            return Ok();
        }

        [HttpGet("get-author-with-books-by-id/{authorId}")]
        public IActionResult GetAuthorWithBooks(int authorId)
        {
            var response = _authorsService.GetAuthorWithBooks(authorId);
            return Ok(response);
        }






    }
}
