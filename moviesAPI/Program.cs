using Microsoft.EntityFrameworkCore;
using moviesAPI.Repositories;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Interfaces;
using moviesAPI.Services;
using moviesAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

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