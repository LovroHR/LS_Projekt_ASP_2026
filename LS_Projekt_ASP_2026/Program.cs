using LS_Projekt_ASP_2026.Data;

var builder = WebApplication.CreateBuilder(args);

// Dodaj Razor Pages
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IRepository, MockRepository>();

// Dodaj session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

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

// Mapiranje stranica
app.MapRazorPages();

// Pokretanje aplikacije
app.Run();