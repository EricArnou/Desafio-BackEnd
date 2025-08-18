using Microsoft.EntityFrameworkCore;
using moto_rent.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
