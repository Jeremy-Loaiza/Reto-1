using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoBackend.Data;

namespace RetoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecaudoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecaudoController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================================
        // 🔹 1️⃣ GET: /api/Recaudo/conteo/{fecha}?page=1&pageSize=50
        // ==========================================================
        [HttpGet("conteo/{fecha}")]
        public async Task<IActionResult> GetConteoVehiculos(
            DateTime fecha,
            int page = 1,
            int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 500) pageSize = 50;

            var query = _context.Recaudos
                .Where(r => r.Fecha.Date == fecha.Date)
                .GroupBy(r => new { r.EstacionNombre, r.Hora })
                .Select(g => new
                {
                    EstacionNombre = g.Key.EstacionNombre,
                    Hora = g.Key.Hora,
                    TotalVehiculos = g.Sum(x => x.Cantidad)
                });

            var totalRegistros = await query.CountAsync();

            var data = await query
                .OrderBy(r => r.EstacionNombre)
                .ThenBy(r => r.Hora)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!data.Any())
                return NotFound(new { detail = "No hay datos para la fecha seleccionada." });

            return Ok(new
            {
                totalRegistros,
                paginaActual = page,
                tamanioPagina = pageSize,
                totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize),
                resultados = data
            });
        }

        // ==========================================================
        // 🔹 2️⃣ GET: /api/Recaudo/reporte
        // ==========================================================
        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporteMensual()
        {
            var data = await _context.Recaudos
                .GroupBy(r => new
                {
                    Anio = r.Fecha.Year,
                    Mes = r.Fecha.Month,
                    r.EstacionNombre
                })
                .Select(g => new
                {
                    Anio = g.Key.Anio,
                    Mes = g.Key.Mes,
                    EstacionNombre = g.Key.EstacionNombre,
                    TotalCantidad = g.Sum(x => x.Cantidad),
                    TotalValor = g.Sum(x => x.Valor)
                })
                .OrderBy(r => r.Anio)
                .ThenBy(r => r.Mes)
                .ThenBy(r => r.EstacionNombre)
                .ToListAsync();

            if (!data.Any())
                return NotFound(new { detail = "No hay datos disponibles para generar el reporte." });

            return Ok(data);
        }

        // ==========================================================
        // 🔹 3️⃣ GET: /api/Recaudo/count
        // ==========================================================
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalRegistros()
        {
            var total = await _context.Recaudos.CountAsync();
            return Ok(new { totalRegistros = total });
        }
    }
}
