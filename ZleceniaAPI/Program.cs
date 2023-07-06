using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using ZleceniaAPI;
using ZleceniaAPI.Authorization;
using ZleceniaAPI.Entities;
using ZleceniaAPI.Models;
using ZleceniaAPI.Models.Validators;
using ZleceniaAPI.Seeders;
using ZleceniaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();
// Add services to the container.

var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);


builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer, // wydawca danego tokenu
        ValidAudience = authenticationSettings.JwtIssuer, // dozwolone podmioty do uzycia naszego tokenu
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)) // klucz prywatny generowany na podstawie JwtKey
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsContractor", builder => builder.AddRequirements(new TypeOfAccountRequirement("Wykonawca")));
    options.AddPolicy("IsPrincipal", builder => builder.AddRequirements(new TypeOfAccountRequirement("Zleceniodawca")));
});
builder.Services.AddScoped<IAuthorizationHandler, TypeOfAccountRequirementHandler>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<UserSeeder>();
builder.Services.AddScoped<CategorySeeder>();

builder.Services.AddDbContext<OferiaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OferiaDbConnection")));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHostedService<OrderStatusBackgroundService>();


builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateUserCategoryDto>, CreateUseCategoryDtoValidator>();
builder.Services.AddScoped<IValidator<AddOrderDto>, AddOrderDtoValidator>();
builder.Services.AddScoped<IValidator<AddOfferDto>, AddOfferDtoValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", builder =>
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:5173")
        );
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.MaxDepth = 4;
    });


var app = builder.Build();

var scope = app.Services.CreateScope();
var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
var categorySeeder = scope.ServiceProvider.GetRequiredService<CategorySeeder>();

userSeeder.Seed();
categorySeeder.Seed();

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseCors("FrontendClient");

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI");
});


app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
