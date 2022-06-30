using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NewsAppNet.Data;
using NewsAppNet.Data.Repositories;
using NewsAppNet.Data.Repositories.Interfaces;
using NewsAppNet.Services;
using NewsAppNet.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NewsAppNet.Models.DataModels;
using Microsoft.AspNetCore.Identity;
using NewsAppNet.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

var docker = (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true");

string connectionString;

if (docker)
{
    connectionString = configuration.GetConnectionString("NewsAppDb");
    builder.Services.AddDbContext<NewsAppDbContext>(x => x.UseSqlServer(connectionString));
}
else
{
    connectionString = configuration.GetConnectionString("Sqlite");
    builder.Services.AddDbContext<NewsAppDbContext>(x => x.UseSqlite(connectionString));
}

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<NewsAppDbContext>();

var jwtSection = configuration.GetSection("JwtBearerTokenSettings");
builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);
var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();
var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = jwtBearerTokenSettings.Issuer,
            ValidAudience = jwtBearerTokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });
/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,

            IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWTSecretKey"))
                        )
        };
    });
*/
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<INewsFeedService, NewsFeedService>();
builder.Services.AddScoped<INewsFeedRepository, NewsFeedRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowAllOrigins",
                                  builder =>
                                  {
                                      builder
                                        .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
});

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.

DatabaseManagementService.MigrationInitialisation(app, docker);

app.UseCors("MyAllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
