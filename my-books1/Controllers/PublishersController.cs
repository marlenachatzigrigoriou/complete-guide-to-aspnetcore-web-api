using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books1.Data.Services;
using my_books1.Data.ViewModels;
using my_books1.Exceptions;

namespace my_books1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {

        public PublishersService _publishersService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
        {
            _publishersService = publishersService;
            _logger = logger;
        }


        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string? sortBy, string? searchString, int pageNumber)
        {
            try
            {
                _logger.LogInformation("This is just a log in GetAllPublishers().");
                var _result = _publishersService.GetAllPublishers(sortBy, searchString, pageNumber);
                return Ok(_result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the publishers");
            }
        }


        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest("Sorry dude, smth went wrong.." + ex.Message);
            }
        }

        [HttpGet("get-publisher-by-id/{publisherId}")]
        public IActionResult GetPublisherById(int publisherId)
        {
            var _response = _publishersService.GetPublisherById(publisherId);
            if (_response != null)
            {
                return Ok(_response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get-publisher-books-with-authors/{publisherId}")]
        public IActionResult GetPublisherData(int publisherId)
        {
            var _response = _publishersService.GetPublisherData(publisherId);
            return Ok(_response);
        }


        [HttpDelete("delete-publisher-by-id")]
        public IActionResult DeletePublisherById(int publisherId)
        {
            try
            {
                _publishersService.DeletePublisherById(publisherId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Sorry dude, smth went wrong.." + ex.Message);
            }
        }
    }
}
