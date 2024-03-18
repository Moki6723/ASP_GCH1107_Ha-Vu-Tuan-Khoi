using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Asmmvc1670.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Asmmvc1670Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Asmmvc1670Context") ?? throw new InvalidOperationException("Connection string 'Asmmvc1670Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IOTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
