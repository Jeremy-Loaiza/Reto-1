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

        // ==========================================================
        // 🔹 POST: /api/Import/fetch-range
        // ==========================================================
        [HttpPost("fetch-range")]
        public async Task<IActionResult> FetchRange([FromBody] DateRangeRequest request)
        {
            if (request == null || request.From == default || request.To == default)
                return BadRequest(new { error = "Debe proporcionar las fechas 'from' y 'to' correctamente." });

            if (request.From > request.To)
                return BadRequest(new { error = "La fecha inicial no puede ser mayor que la final." });

            try
            {
                var result = await _service.ImportRangeAsync(request.From, request.To);

                return Ok(new
                {
                    desde = request.From.ToString("yyyy-MM-dd"),
                    hasta = request.To.ToString("yyyy-MM-dd"),
                    saved = result,
                    message = $"✅ Se importaron {result} registros entre {request.From:yyyy-MM-dd} y {request.To:yyyy-MM-dd}."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error durante la importación.", detail = ex.Message });
            }
        }
    }

    // ==========================================================
    // 📅 Clase para manejar el cuerpo del request
    // ==========================================================
    public class DateRangeRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
