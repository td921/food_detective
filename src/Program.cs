using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Foods}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Foods}/{action=GetFoods}");

app.MapControllerRoute(
    name: "getAllIngredients",
    pattern: "{controller=Ingredients}/{action=GetHarmfulIngredients}");

app.Run();
