using Microsoft.EntityFrameworkCore;
using moto_rent_consumer.Features.Motors;
using moto_rent_consumer.Persistence;
using moto_rent_consumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddOpenApi();

builder.Services.AddScoped<IMotorRepository, MotorRepository>();
builder.Services.AddScoped<MotorService>();
builder.Services.AddHostedService<Consumer>();
builder.Services.AddLogging();

var app = builder.Build();

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
            Console.WriteLine($"Attempt {attempts + 1}/{maxAttempts} - Applying migrations to the database...");
            db.Database.Migrate();
            Console.WriteLine("Database migration completed successfully.");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to apply migrations. Retrying in {delayInSeconds}s... Error: {ex.Message}");
            attempts++;
            if (attempts == maxAttempts)
            {
                Console.WriteLine("Max attempts reached. Could not connect to the database.");
                throw;
            }
            Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();