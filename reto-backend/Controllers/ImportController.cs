using Microsoft.AspNetCore.Mvc;
using RetoBackend.Services;

namespace RetoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly RecaudoService _service;

        public ImportController(RecaudoService service)
        {
            _service = service;
        }

        [HttpPost("fetch-range")]
        public async Task<IActionResult> FetchRange([FromBody] DateRangeRequest request)
        {
            var total = await _service.ImportRangeAsync(request.From, request.To);
            return Ok(new { saved = total });
        }
    }

    public class DateRangeRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
