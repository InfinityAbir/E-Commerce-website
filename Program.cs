using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceDbContext")
    ?? throw new InvalidOperationException("Connection string 'EcommerceDbContext' not found.")));

// Add session support
builder.Services.AddSession();

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Enable serving static files
app.UseStaticFiles();

// Enable session before routing
app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Route config
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
