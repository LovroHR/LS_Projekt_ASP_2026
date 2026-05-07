using LS_Projekt_ASP_2026.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
    builder.Services.AddRazorPages();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IRepository, EfRepository>();

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
        var services = scope.ServiceProvider;
        SeedData.Initialize(services);
        Log.Information("Seed podaci su učitani u bazu");
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

    app.UseRouting();

    // Koristi session
    app.UseSession();

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
    Log.Fatal(ex, "!!! KRITIČNA GREŠKA - Aplikacija se srušila !!!");
    Console.WriteLine(ex.ToString());
}
finally
{
    Log.Information("=== Aplikacija zaustavljena ===");
    await Log.CloseAndFlushAsync();
}