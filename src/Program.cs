using food_detective.Services.FoodCentralApi;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IFoodCentralApiService, FoodCentralApiService>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    public static void Configure(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=ToggleDarkMode}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Foods}/{action=Index}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Foods}/{action=GetFoods}");

            app.MapControllerRoute(
                name: "getAllIngredients",
                pattern: "{controller=Ingredients}/{action=GetHarmfulIngredients}");
        });
    }
}
