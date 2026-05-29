using Microsoft.EntityFrameworkCore;
using DL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5174", "https://localhost:7123", "http://127.0.0.1:5174", "https://127.0.0.1:7123")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Registrar controladores de la API
builder.Services.AddControllers();

// Registrar servicios de la capa de Negocio (BL)
builder.Services.AddScoped<BL.Alumno>();
builder.Services.AddScoped<BL.AsignacionDocente>();
builder.Services.AddScoped<BL.Calificacion>();
builder.Services.AddScoped<BL.Empleado>();
builder.Services.AddScoped<BL.Especialidad>();
builder.Services.AddScoped<BL.Grupo>();
builder.Services.AddScoped<BL.Materia>();
builder.Services.AddScoped<BL.Rol>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowMvcApp");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
