using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PulseGamingMVC.Data;
using PulseGamingMVC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

string connectionString = builder.Configuration.GetConnectionString("SqlPulseGaming");
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddTransient<IRepositoryJuegos, RepositoryJuegosSqlServer>();
builder.Services.AddDbContext<PulseGamingContext>(options => options.UseSqlServer(connectionString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath)),

});
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Juegos}/{action=Home}/{id?}");

app.Run();
