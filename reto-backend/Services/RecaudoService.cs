using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using RetoBackend.Data;
using RetoBackend.Models;

namespace RetoBackend.Services
{
    public class RecaudoService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        public RecaudoService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _http = httpClientFactory.CreateClient();
        }

        // ✅ Método principal: importa los datos entre fechas
        public async Task<int> ImportRangeAsync(DateTime start, DateTime end)
        {
            int totalSaved = 0;

            // 1️⃣ Obtener token de autenticación
            var token = await ObtenerTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ No se pudo obtener el token de la API externa.");
                return 0;
            }

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2️⃣ Recorrer las fechas
            for (var fecha = start; fecha <= end; fecha = fecha.AddDays(1))
            {
                try
                {
                    Console.WriteLine($"\n📅 Procesando fecha: {fecha:yyyy-MM-dd}");

                    // --- Obtener datos de conteo ---
                    var urlConteo = $"http://23.23.205.17:5200/api/ConteoVehiculos/{fecha:yyyy-MM-dd}";
                    var conteos = await ObtenerDatosAsync(urlConteo);

                    // --- Obtener datos de recaudo ---
                    var urlRecaudo = $"http://23.23.205.17:5200/api/RecaudoVehiculos/{fecha:yyyy-MM-dd}";
                    var recaudos = await ObtenerDatosAsync(urlRecaudo);

                    // --- Guardar datos de recaudos ---
                    if (recaudos != null)
                    {
                        foreach (var r in recaudos)
                        {
                            var entidad = new RecaudoEntity
                            {
                                Fecha = fecha,
                                EstacionNombre = r.GetProperty("estacion").GetString(),
                                Sentido = r.GetProperty("sentido").GetString(),
                                Categoria = r.GetProperty("categoria").GetString(),
                                Hora = r.GetProperty("hora").GetInt32(),
                                Cantidad = 1, // se puede ajustar si la API entrega conteo
                                Valor = Convert.ToDecimal(r.GetProperty("valorTabulado").GetDecimal())
                            };

                            _context.Recaudos.Add(entidad);
                            totalSaved++;
                        }
                    }

                    // --- Guardar datos de conteos ---
                    if (conteos != null)
                    {
                        foreach (var c in conteos)
                        {
                            var entidad = new RecaudoEntity
                            {
                                Fecha = fecha,
                                EstacionNombre = c.GetProperty("estacion").GetString(),
                                Sentido = c.GetProperty("sentido").GetString(),
                                Categoria = c.GetProperty("categoria").GetString(),
                                Hora = c.GetProperty("hora").GetInt32(),
                                Cantidad = c.GetProperty("cantidad").GetInt32(),
                                Valor = 0 // en conteo no hay valor monetario
                            };

                            _context.Recaudos.Add(entidad);
                            totalSaved++;
                        }
                    }

                    await _context.SaveChangesAsync();
                    Console.WriteLine($"✅ Guardados {totalSaved} registros de {fecha:yyyy-MM-dd}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error procesando {fecha:yyyy-MM-dd}: {ex.Message}");
                }
            }

            Console.WriteLine($"\n🚀 Proceso finalizado. Total registros guardados: {totalSaved}");
            return totalSaved;
        }

        // 🔹 Método auxiliar para obtener token
        private async Task<string?> ObtenerTokenAsync()
        {
            var loginData = new
            {
                userName = "user",
                password = "1234"
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _http.PostAsync("http://23.23.205.17:5200/api/Login", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("❌ Error al autenticar con la API externa.");
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseBody);
                var token = doc.RootElement.GetProperty("token").GetString();

                Console.WriteLine("🔑 Token obtenido correctamente.");
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error obteniendo token: {ex.Message}");
                return null;
            }
        }

        // 🔹 Método auxiliar para consumir endpoints externos (GET)
        private async Task<JsonElement[]>? ObtenerDatosAsync(string url)
        {
            try
            {
                var response = await _http.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ No se pudieron obtener datos desde {url}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    return doc.RootElement.EnumerateArray().ToArray();

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error al obtener datos desde {url}: {ex.Message}");
                return null;
            }
        }
    }
}
