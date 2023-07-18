using Microsoft.EntityFrameworkCore;
using moviesAPI.Repositories;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Interfaces;
using moviesAPI.Services;
using moviesAPI.Validators;
using Microsoft.AspNetCore.Identity;
using moviesAPI.Areas.Identity.Data;
using PdfSharp.Charting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddDbContext<IdentityContext>();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddScoped<GenericCinemaRepository>();
builder.Services.AddScoped<IPdfTransformService, PdfTransformService>();
builder.Services.AddTransient<EntityValidator>();

builder.Services.AddControllersWithViews();

//resolve fonts once
MyFontResolver.Apply();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();