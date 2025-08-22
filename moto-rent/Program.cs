using System.Reflection;
using Microsoft.EntityFrameworkCore;
using moto_rent.Features.Motors;
using moto_rent.Features.Rentals;
using moto_rent.Features.Rentals.Services;
using moto_rent.Features.Riders;
using moto_rent.Features.Riders.Services;
using moto_rent.Persistence;
using moto_rent.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Adiciona uma descrição geral da API
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Moto Rent API",
        Version = "v1",
        Description = "API para gerenciamento de aluguel de motos, pilotos e entregas."
    });

    // Configura o Swagger para usar os comentários XML
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddScoped<IRiderRepository, RiderRepository>();
builder.Services.AddScoped<RiderService>();

builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<RentalService>();

builder.Services.AddScoped<IMotorRepository, MotorRepository>();
builder.Services.AddScoped<MotorService>();
builder.Services.AddSingleton<MotoEventPublisher>();
builder.Services.AddLogging();

var app = builder.Build();

// --- Lógica de Retentativa para Migração do Banco de Dados ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    int attempts = 0;
    const int maxAttempts = 10;
    const int delayInSeconds = 5;

    while (attempts < maxAttempts)
    {
        try
        {
            Console.WriteLine($"Tentativa {attempts + 1}/{maxAttempts} - Aplicando migrações no banco de dados...");
            db.Database.Migrate();
            Console.WriteLine("Migração do banco de dados concluída com sucesso.");
            break; // Sai do loop se a migração for bem-sucedida
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha ao aplicar as migrações. Tentando novamente em {delayInSeconds}s... Erro: {ex.Message}");
            attempts++;
            if (attempts == maxAttempts)
            {
                Console.WriteLine("Número máximo de tentativas alcançado. Não foi possível conectar ao banco de dados.");
                throw; // Lança a exceção para parar a aplicação
            }
            Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
        }
    }
}
// --- Fim da Lógica de Retentativa ---

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto Rent API V1");
});

app.Urls.Add("http://0.0.0.0:5000");
app.UseAuthorization();
app.MapControllers();

app.Run();

