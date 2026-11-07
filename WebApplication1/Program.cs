using System.Diagnostics;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using WebApplication1.Middleware;
using Microsoft.Extensions.Caching.Memory;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

//Logging Serilog(Registation)
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("ShoppingCartConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();

builder.Host.UseSerilog();//use method
//startup
builder.WebHost.UseUrls("http://localhost:5178", "https://localhost:7047");

// Add services to the container.//database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
//Add ShoppingCart to the services that is registering them
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ShoppingCartConnection")
    ));

//Registring Batchfile 

builder.Services.AddScoped<IBatchService, BatchService>();

//HangFire Resgistring

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection"));
});

builder.Services.AddHangfireServer();

// AUTHENTICATION

builder.Services.AddAuthentication(options =>
{
    // Keep JWT as the default authentication
    // scheme for Web API controllers.
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})

// JWT AUTHENTICATION - WEB API

.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.ContentType =
                "application/json";

            return context.Response.WriteAsJsonAsync(
                new
                {
                    message =
                        "Access token is missing or invalid"
                });
        }
    };
})

// ADMIN COOKIE

.AddCookie("AdminCookie", options =>
{
    options.LoginPath = "/AdminLogin";
    options.AccessDeniedPath = "/AdminLogin";
})

// CUSTOMER COOKIE

.AddCookie("CustomerCookie", options =>
{
    options.LoginPath = "/CustomerLogin";
    options.AccessDeniedPath = "/CustomerLogin";
});

//Session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//Register IHttpContextAccessor

builder.Services.AddHttpContextAccessor();

// Register MVC controllers and views

builder.Services.AddControllersWithViews();

// Registering services for dependency injection
//Student
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerAuthService, CustomerAuthService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
//Shopping Cart
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserCartService, UserCartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGuestCartService, GuestCartService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Registring Cache
builder.Services.AddMemoryCache();

//Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "WebApplication1:";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        _ = Task.Run(async () =>
        {
            await Task.Delay(1000);
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://localhost:5178/",
                UseShellExecute = true
            });
        });
    });
}

app.MapGet("/", (HttpContext context) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        return Results.Redirect("/Admin/Dashboard");
    }

    return Results.Redirect("/AdminLogin");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Dashboard}/{id?}");

app.Run();