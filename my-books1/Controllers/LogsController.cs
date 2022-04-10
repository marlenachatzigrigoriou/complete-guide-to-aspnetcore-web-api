using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books1.Data.Services;

namespace my_books1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private LogsService _logsService;
        public LogsController(LogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet("get-all-logs-from-db")]
        public IActionResult GetAllLogsFromDB()
        {
            try {
                var allLogs = _logsService.GetAllLogsFromDB();
                return Ok(allLogs);
            }
	        catch (Exception ex)
	        {    
		        return BadRequest("Could not load logs from database");
	        }
        }
    }
}
