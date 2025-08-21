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
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRiderRepository, RiderRepository>();
builder.Services.AddScoped<RiderService>();

builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<RentalService>();

builder.Services.AddScoped<IMotorRepository, MotorRepository>();
builder.Services.AddScoped<MotorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://localhost:5000");
app.Urls.Add("https://localhost:5001");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
