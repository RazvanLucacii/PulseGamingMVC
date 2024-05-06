using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MvcCoreAzureStorage.Services;
using PulseGamingMVC.Data;
using PulseGamingMVC.Repositories;
using Microsoft.Extensions.Azure;
using PulseGamingMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

// Add services to the container.
builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false);
builder.Services.AddSession();

builder.Services.AddTransient<ServicePulseGaming>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

string CacheRedis = builder.Configuration.GetValue<string>("AzureKeys:CacheRedis");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = CacheRedis;
});

builder.Services.AddTransient<ServiceStorageBlobs>();
string azureKeys = builder.Configuration.GetValue<string>("AzureKeys:StorageAccount")!;
BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>(x => blobServiceClient);

string connectionString = builder.Configuration.GetConnectionString("SqlPulseGaming")!;
builder.Services.AddTransient<IRepositoryUsuarios, RepositoryUsuarios>();
builder.Services.AddTransient<IRepositoryJuegos, RepositoryJuegosSqlServer>();
builder.Services.AddDbContext<PulseGamingContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["StorageConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["StorageConnectionString:queue"]!, preferMsi: true);
});



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

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMvc(routes =>
{
    routes.MapRoute(
    name: "default",
    template: "{controller=Juegos}/{action=Inicio}/{id?}");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Juegos}/{action=Inicio}/{id?}");

app.Run();
