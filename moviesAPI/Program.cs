using Microsoft.EntityFrameworkCore;
using moviesAPI.Repositories;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Interfaces;
using moviesAPI.Services;
using moviesAPI.Validators;
using moviesAPI.Areas.Identity.Data;
using NuGet.Configuration;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddDefaultIdentity<User>(options => 
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
}).AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddScoped<GenericCinemaRepository>();
builder.Services.AddScoped<IPdfTransformService, PdfTransformService>();
builder.Services.AddTransient<EntityValidator>();

builder.Services.AddControllersWithViews();

//resolve fonts once
MyFontResolver.Apply();

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("uk-UA"),
});

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