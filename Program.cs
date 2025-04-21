using Microsoft.AspNetCore.Authentication.Cookies;
using Rentacar.Models;
using Rentacar.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
x.Cookie.Name = "LoginCookie";
x.LoginPath = "/Login/Login";
x.AccessDeniedPath = "/Login/Login";
x.LogoutPath = "/Login/LogOut";
x.ExpireTimeSpan = TimeSpan.FromMinutes(20);
x.SlidingExpiration = true;
});

builder.Services.Configure<IyzicoOptions>(builder.Configuration.GetSection("IyzicoOptions"));
builder.Services.AddScoped<PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
