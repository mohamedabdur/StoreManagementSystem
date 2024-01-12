

// Project Title        :  Store Management System
// author               :  Mohamed Ashiq Ilahi M A
// created at           :  15-02-2023
// last modified date   :  09-03-2023
// Reviewed by          :  Anitha Manogaran.
// Reviewed Date        :  15-03-2023


#nullable disable
using Microsoft.EntityFrameworkCore;

using practice.Data;
using practice.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<StoreMS>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddSingleton<IRepository, Repository>();

builder.Services.AddSession(options =>
{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
