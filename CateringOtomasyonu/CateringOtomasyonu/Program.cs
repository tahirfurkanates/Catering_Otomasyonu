using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using CateringOtomasyonu.db; // DbContext'in bulunduðu namespace

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(60);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// DbContext
var cs = builder.Configuration.GetConnectionString("CateringDb");
builder.Services.AddDbContext<CateringDbContext>(opt => opt.UseSqlServer(cs));

// Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Login/Giris";      // yetki yoksa buraya atar
        o.AccessDeniedPath = "/Login/Giris";
        o.Cookie.Name = "CateringAuth";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sýra önemli
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ---- Özel Menü Detay Rotasý ----
// /menu/5/alkollu-kokteyl-menu gibi
app.MapControllerRoute("menu-details", "menu/{id:int}/{slug?}",
    new { controller = "Menu", action = "Details" });
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


app.Run();
