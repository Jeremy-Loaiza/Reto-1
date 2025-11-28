using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoBackend.Data;
using RetoBackend.Models;

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
        // 🔹 1️⃣ GET: /api/Recaudo/conteo/{fecha}
        // 👉 Conteo con filtros + paginación + rango de hora + VALOR
        // ==========================================================
        [HttpGet("conteo/{fecha}")]
        public async Task<IActionResult> GetConteoVehiculos(
            DateTime fecha,
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null,
            [FromQuery] string? estacion = null,
            [FromQuery] int? horaDesde = null,
            [FromQuery] int? horaHasta = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;

            var query = _context.Recaudos.AsQueryable();

            // ✅ Fecha principal
            query = query.Where(r => r.Fecha.Date == fecha.Date);

            // ✅ Rango de fechas
            if (desde.HasValue)
                query = query.Where(r => r.Fecha.Date >= desde.Value.Date);

            if (hasta.HasValue)
                query = query.Where(r => r.Fecha.Date <= hasta.Value.Date);

            // ✅ Estación
            if (!string.IsNullOrWhiteSpace(estacion))
                query = query.Where(r => r.EstacionNombre == estacion);

            // ✅ Rango de horas
            if (horaDesde.HasValue && horaHasta.HasValue)
                query = query.Where(r => r.Hora >= horaDesde.Value && r.Hora <= horaHasta.Value);
            else if (horaDesde.HasValue)
                query = query.Where(r => r.Hora >= horaDesde.Value);
            else if (horaHasta.HasValue)
                query = query.Where(r => r.Hora <= horaHasta.Value);

            // ✅ Agrupación CON VALOR
            var agrupado = query
                .GroupBy(r => new { r.EstacionNombre, r.Hora })
                .Select(g => new
                {
                    EstacionNombre = g.Key.EstacionNombre ?? "SIN ESTACIÓN",
                    Hora = g.Key.Hora,
                    TotalVehiculos = g.Sum(x => x.Cantidad),
                    TotalValor = g.Sum(x => x.Valor)
                });

            // ✅ Totales generales (sin paginar)
            var totalGeneralVehiculos = await agrupado.SumAsync(x => x.TotalVehiculos);
            var totalGeneralValor = await agrupado.SumAsync(x => x.TotalValor);

            // ✅ Total registros agrupados
            var totalRegistros = await agrupado.CountAsync();

            // ✅ Datos paginados
            var data = await agrupado
                .OrderBy(r => r.EstacionNombre)
                .ThenBy(r => r.Hora)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total = totalRegistros,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(totalRegistros / (double)pageSize),
                totalGeneralVehiculos,
                totalGeneralValor,
                data
            });
        }

        // ==========================================================
        // 🔹 2️⃣ GET: /api/Recaudo/reporte
        // 👉 Reporte mensual
        // ==========================================================
        [HttpGet("reporte")]
        public async Task<IActionResult> GetReporteMensual(
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null)
        {
            if (!desde.HasValue || !hasta.HasValue)
                return BadRequest(new { detail = "Debe especificar el rango de fechas (desde / hasta)." });

            var query = _context.Recaudos
                .Where(r => r.Fecha.Date >= desde.Value.Date &&
                            r.Fecha.Date <= hasta.Value.Date);

            var data = await query
                .GroupBy(r => new
                {
                    Anio = r.Fecha.Year,
                    Mes = r.Fecha.Month,
                    Estacion = r.EstacionNombre
                })
                .Select(g => new
                {
                    g.Key.Anio,
                    g.Key.Mes,
                    EstacionNombre = g.Key.Estacion ?? "SIN ESTACIÓN",
                    TotalCantidad = g.Sum(x => x.Cantidad),
                    TotalValor = g.Sum(x => x.Valor)
                })
                .OrderBy(r => r.Anio)
                .ThenBy(r => r.Mes)
                .ThenBy(r => r.EstacionNombre)
                .ToListAsync();

            return Ok(data);
        }

        // ==========================================================
        // 🔹 3️⃣ GET: /api/Recaudo/reporte-diario
        // 👉 Reporte por día
        // ==========================================================
        [HttpGet("reporte-diario")]
        public async Task<IActionResult> GetReporteDiario(
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null)
        {
            if (!desde.HasValue || !hasta.HasValue)
                return BadRequest(new { detail = "Debe especificar el rango de fechas (desde / hasta)." });

            var query = _context.Recaudos
                .Where(r => r.Fecha.Date >= desde.Value.Date &&
                            r.Fecha.Date <= hasta.Value.Date);

            var data = await query
                .GroupBy(r => new
                {
                    Fecha = r.Fecha.Date,
                    Estacion = r.EstacionNombre
                })
                .Select(g => new
                {
                    Fecha = g.Key.Fecha,
                    EstacionNombre = g.Key.Estacion ?? "SIN ESTACIÓN",
                    TotalCantidad = g.Sum(x => x.Cantidad),
                    TotalValor = g.Sum(x => x.Valor)
                })
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.EstacionNombre)
                .ToListAsync();

            return Ok(data);
        }
    }
}
