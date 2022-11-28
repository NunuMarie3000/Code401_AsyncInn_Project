using AsyncInn.Data;
using AsyncInn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AsyncInnDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnectionString")));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<AsyncInnDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
  options.User.RequireUniqueEmail = true;
  // There are other options like this
})
.AddEntityFrameworkStores<AsyncInnDbContext>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
  // Password settings.
  options.Password.RequireDigit = true;
  options.Password.RequireLowercase = true;
  options.Password.RequireNonAlphanumeric = true;
  options.Password.RequireUppercase = true;
  options.Password.RequiredLength = 6;
  options.Password.RequiredUniqueChars = 1;

  // Lockout settings.
  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
  options.Lockout.MaxFailedAccessAttempts = 5;
  options.Lockout.AllowedForNewUsers = true;

  // User settings.
  options.User.AllowedUserNameCharacters =
  "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
  options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
  // Cookie settings
  options.Cookie.HttpOnly = true;
  options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

  options.LoginPath = "/Identity/Account/Login";
  options.AccessDeniedPath = "/Identity/Account/AccessDenied";
  options.SlidingExpiration = true;
});

/*
 // Register the Swagger services
services.AddSwaggerDocument();
 */
builder.Services.AddSwaggerDocument(config =>
config.PostProcess = document =>
{
  document.Info.Title = "Async Inn Project";
  document.Info.Version= "v3";
  document.Info.Description = "Fighting for my life tryig to learn ASP.NET Core";
  document.Info.TermsOfService = "none";
  document.Info.Contact = new NSwag.OpenApiContact
  {
    Name = "Storm",
    Email = "vmarie1997@gmail.com",
    Url = "https://storm-obryant.netlify.app"
  };
  document.Info.License = new NSwag.OpenApiLicense
  {
    Name = "Use under MIT",
    Url = "https://opensource.org/licenses/MIT"
  };
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Register the Swagger generator and the Swagger UI
app.UseOpenApi();
app.UseSwaggerUi3();

app.Run();
