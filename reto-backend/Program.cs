using Microsoft.EntityFrameworkCore;
using RetoBackend.Data;
using RetoBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión a SQL Server desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔹 Registrar servicios e HttpClient
builder.Services.AddScoped<RecaudoService>();
builder.Services.AddHttpClient();

// 🔹 Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
