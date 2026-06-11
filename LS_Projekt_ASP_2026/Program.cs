using LS_Projekt_ASP_2026.Data;
using LS_Projekt_ASP_2026.Model;
using LS_Projekt_ASP_2026.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

// Odredi putanju za log datoteku
var logPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "..",
    "lab-3",
    "agent_log.txt"
);

// Kreiraj folder ako ne postoji
var logDirectory = Path.GetDirectoryName(logPath);
if (logDirectory != null && !Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Konfiguracija Serilog-a za file logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(
        path: logPath,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true
    )
    .CreateLogger();

try
{
    Log.Information("=== Aplikacija pokrenuta ===");
    
    var builder = WebApplication.CreateBuilder(args);
    
    // Postavi Serilog kao logger
    builder.Host.UseSerilog();

    // Dodaj MVC i Razor Pages
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services
        .AddIdentity<IdentityAppUser, IdentityRole<int>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.Cookie.Name = "LStudio.Identity";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

    var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
    var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
    {
        builder.Services
            .AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = googleClientId;
                options.ClientSecret = googleClientSecret;
                options.CallbackPath = "/signin-google";
                options.SaveTokens = true;
            });
    }

    builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityAppUser>, AppUserClaimsPrincipalFactory>();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("ProducerOrAdmin", policy => policy.RequireRole("Producer", "Admin"));
        options.AddPolicy("AuthenticatedUser", policy => policy.RequireRole("Client", "Producer", "Admin"));
    });

    builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AllowAnonymousToPage("/Index");
        options.Conventions.AllowAnonymousToPage("/Privacy");
        options.Conventions.AllowAnonymousToFolder("/Auth");
        options.Conventions.AuthorizeFolder("/Bookings", "AuthenticatedUser");
        options.Conventions.AuthorizeFolder("/Projects", "AuthenticatedUser");
        options.Conventions.AuthorizeFolder("/Player", "AuthenticatedUser");
        options.Conventions.AuthorizeFolder("/Profile", "AuthenticatedUser");
        options.Conventions.AuthorizeFolder("/Clients", "AdminOnly");
        options.Conventions.AuthorizeFolder("/Producers", "AdminOnly");
        options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
        options.Conventions.AuthorizeFolder("/StudioRooms", "ProducerOrAdmin");
    });
    builder.Services.AddScoped<IRepository, EfRepository>();

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("hr-HR"),
            new CultureInfo("en-US")
        };

        options.DefaultRequestCulture = new RequestCulture("hr-HR");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
    });

    // Dodaj session
    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    var app = builder.Build();

    // Inicijalizacija baze s seed podacima
    using (var scope = app.Services.CreateScope())
    {
        if (!app.Environment.IsEnvironment("Testing"))
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            SeedData.Initialize(services);
            Log.Information("Seed podaci su učitani u bazu");
        }
    }

    // Error handling
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Middleware
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRequestLocalization();

    app.UseRouting();

    // Koristi session
    app.UseSession();

    app.UseAuthentication();
    app.UseAuthorization();

    // Mapiranje
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    // Pokretanje aplikacije
    Log.Information("Počinje slušanje na portu...");
    Console.WriteLine("APP STARTED");
    app.Run();
}
catch (Exception ex)
{
    if (ex.GetType().Name == "HostAbortedException")
    {
        throw;
    }
    Log.Fatal(ex, "!!! KRITIČNA GREŠKA - Aplikacija se srušila !!!");
    Console.WriteLine(ex.ToString());
}
finally
{
    Log.Information("=== Aplikacija zaustavljena ===");
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
}
