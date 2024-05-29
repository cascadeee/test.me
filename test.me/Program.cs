using Microsoft.EntityFrameworkCore;
using testme;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "/Auth", defaults: new { controller = "Auth", action = "Index" });

app.MapControllerRoute(name: "Auth", pattern: "/Auth", defaults: new { controller = "Auth", action = "Index" });

app.MapControllerRoute(name: "Home", pattern: "/Home", defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(name: "Detail", pattern: "/Detail/{id}", defaults: new { controller = "Test", action = "Index" });

app.Run();