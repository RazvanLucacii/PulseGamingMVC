using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("SqlPulseGaming");

builder.Services.AddDbContext<PulseGamingContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IRepositoryJuegos, RepositoryJuegosSqlServer>();


var app = builder.Build();

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
    pattern: "{controller=Juegos}/{action=Home}/{id?}");

app.Run();
