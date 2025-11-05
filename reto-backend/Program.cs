using Microsoft.EntityFrameworkCore;
using RetoBackend.Data;
using RetoBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// 🔹 1. Configuración de la conexión a SQL Server
// ==========================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ==========================================================
// 🔹 2. Registrar servicios e HttpClient
// ==========================================================
builder.Services.AddScoped<RecaudoService>();
builder.Services.AddHttpClient();

// ==========================================================
// 🔹 3. Habilitar CORS para permitir peticiones desde Angular
// ==========================================================
var corsPolicy = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5500",  // Live Server (VS Code)
                "http://127.0.0.1:5500", // Alternativa local
                "http://localhost:4200"   // Si usas Angular CLI más adelante
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ==========================================================
// 🔹 4. Controladores, Swagger y configuración adicional
// ==========================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==========================================================
// 🔹 5. Middleware de desarrollo
// ==========================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================================
// 🔹 6. Middleware principal del pipeline
// ==========================================================
app.UseCors(corsPolicy);  // ✅ Importante: habilitar CORS antes de autorización
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ==========================================================
// 🔹 7. Iniciar la aplicación
// ==========================================================
app.Run();
