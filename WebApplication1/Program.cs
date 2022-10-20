using Microsoft.EntityFrameworkCore;
using WebApplication1.Clients;
using WebApplication1.Controllers;
using WebApplication1.Data;
using Microsoft.Extensions.Configuration.UserSecrets;
using RestSharp;
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var DefaultConnection = builder.Configuration["ConnectionStringAzure"];

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


// Use Local Connection String if in development
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration["ConnectionStringAzure"]
));


builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth",options =>
{
    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/Login";
    
}); // Cookie Authentication first is schema, then defining the cookie

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("user"));
});



//builder.Services.AddHttpClient<IAnimeClient,AnimeClient>(); // Jikan Rapid API v2 (Depreciated)

builder.Services.AddSingleton<IJikanApiClient, JikanApiClient>();
builder.Services.AddSingleton<IMyAnimeClient, MyAnimeClient>();
builder.Services.AddHttpClient<IMyAnimeClient, MyAnimeClient>(); 



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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
