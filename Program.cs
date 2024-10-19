// File: Program.cs
using Art_Gallery;
using Art_Gallery.Data;
using Art_Gallery.Repositories;
using Art_Gallery.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register ApplicationDbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();

        builder.Services.AddTransient<IHomeRepository, HomeRepository>();
        builder.Services.AddTransient<ICartRepository, CartRepository>();
        builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
        builder.Services.AddTransient<IStockRepository, StockRepository>();
        builder.Services.AddTransient<IGenreRepository, GenreRepository>();
        builder.Services.AddTransient<IFileService, FileService>();
        builder.Services.AddTransient<IArtRepository, ArtRepository>();
        builder.Services.AddTransient<IReportRepository, ReportRepository>();


        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
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

        app.MapRazorPages();

        await app.RunAsync();
    }
}
