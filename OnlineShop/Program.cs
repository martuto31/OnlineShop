using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.DAL.Data;
using OnlineShop.DAL.Repository.Product;
using OnlineShop.DAL.Repository.User;
using OnlineShop.Services.Product;
using OnlineShop.Services.User;
using OnlineShop.Shared.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(OnlineShop.Constants.ConnectionString)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidAudience = configuration[SportsHubConstants.JwtAudience],
        //ValidIssuer = configuration[SportsHubConstants.JwtIssuer],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[SportsHubConstants.JwtKey]))
    };
});

builder.Services.Configure<JsonTokenOptions>(
    builder.Configuration.GetSection(JsonTokenOptions.Jwt));

// Data repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();

// Application services
builder.Services.AddTransient<IJsonTokenService, JsonTokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReviewService, ReviewService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
