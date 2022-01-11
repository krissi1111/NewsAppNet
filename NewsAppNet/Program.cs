using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data;
using NewsAppNet.Data.Repositories;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Services;
using NewsAppNet.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("NewsAppDb");
builder.Services.AddDbContext<NewsAppDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.

DatabaseManagementService.MigrationInitialisation(app);


app.UseAuthorization();

app.MapControllers();

app.Run();
