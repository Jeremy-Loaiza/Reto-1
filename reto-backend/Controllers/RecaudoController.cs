using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoBackend.Data;
using System.Globalization;

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

        // 🔹 GET: /api/Recaudo/conteo/{fecha}
        [HttpGet("conteo/{fecha}")]
        public async Task<IActionResult> GetConteoVehiculos(DateTime fecha)
        {
            var data = await _context.Recaudos
                .Where(r => r.Fecha.Date == fecha.Date)
                .GroupBy(r => new { r.EstacionNombre, r.Hora })
                .Select(g => new
                {
                    EstacionNombre = g.Key.EstacionNombre,
                    Hora = g.Key.Hora,
                    TotalVehiculos = g.Sum(x => x.Cantidad)
                })
                .OrderBy(r => r.Hora)
                .ToListAsync();

            if (data.Count == 0)
                return NotFound(new { detail = "No hay datos para la fecha seleccionada." });

            return Ok(data);
        }

        // 🔹 GET: /api/Recaudo/reporte
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

            return Ok(data);
        }
    }
}
