using Microsoft.EntityFrameworkCore;
using Web_RealEstate;
using Web_RealEstate.Models;
using Web_RealEstate.Reposistory;
using Web_RealEstate.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Ntphu24072001CnaContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefautConnection")
        ));//setup syntax to use DbContext 

builder.Services.AddTransient<IPostReposistory, PostReposistory>();
builder.Services.AddTransient<ISellerReposistory, SellerReposistory>();
builder.Services.AddTransient<IPropertyReposistory, PropertyReposistory>();
builder.Services.AddTransient<INewsReposistory, NewsReposistory>();
builder.Services.AddTransient<IUserReposistory, UserReposistory>();

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
app.Run();
