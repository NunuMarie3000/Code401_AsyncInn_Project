using AsyncInn.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AsyncInnDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnectionString")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Register the Swagger generator and the Swagger UI
app.UseOpenApi();
app.UseSwaggerUi3();

app.Run();
