using Microsoft.EntityFrameworkCore;
using TheBooksNook.Models;
using TheBooksNook.Services;

var builder = WebApplication.CreateBuilder(args);
var dbPassword = builder.Configuration["DbPassword"];
var connectionString =
    $"Server=localhost;port=3306;userid=root;password={dbPassword};database=TheBookNook_db;";

builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<IPasswordService, BcryptPasswordService>();
builder.Services.AddDbContext<ApplicationContext>(
    (options) =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseStatusCodePagesWithReExecute("/error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
