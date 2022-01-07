using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data;
using NewsAppNet.Data.Repositories;
using NewsAppNet.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("NewsAppDb");
builder.Services.AddDbContext<NewsAppDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
