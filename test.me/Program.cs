using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using testme;
using testme.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddMemoryCache();



var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();


app.MapControllerRoute(name: "default", pattern: "/Auth", defaults: new { controller = "Auth", action = "Index" });

app.MapControllerRoute(name: "Auth", pattern: "/Auth", defaults: new { controller = "Auth", action = "Index" });
app.MapControllerRoute(name: "Logout", pattern: "/Logout", defaults: new { controller = "Auth", action = "Logout" });

app.MapControllerRoute(name: "Home", pattern: "/Home", defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(name: "Detail", pattern: "/Detail/{id}", defaults: new { controller = "Test", action = "Index" });
app.MapControllerRoute(name: "DetailSend", pattern: "/Detail/{id}/Send", defaults: new { controller = "Test", action = "Check" });

app.MapControllerRoute(name: "Account", pattern: "/Account", defaults: new { controller = "Account", action = "Index" });

app.MapControllerRoute(name: "Users", pattern: "/AllUsers", defaults: new { controller = "User", action = "AllUsers" });
app.MapControllerRoute(name: "UsersDelete", pattern: "/AllUsers/Delete", defaults: new { controller = "User", action = "Delete" });
app.MapControllerRoute(name: "UsersCreate", pattern: "/AllUsers/Create", defaults: new { controller = "User", action = "Create" });

app.MapControllerRoute(name: "AllTests", pattern: "/AllTests", defaults: new { controller = "Test", action = "AllTests" });
app.MapControllerRoute(name: "MyTests", pattern: "/MyTests", defaults: new { controller = "Test", action = "MyTests" });
app.MapControllerRoute(name: "MyTestsCreate", pattern: "/MyTests/Create", defaults: new { controller = "Test", action = "Create" });

app.Run();