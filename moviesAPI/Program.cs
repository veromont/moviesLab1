using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using moviesAPI.Repositories;
using moviesAPI.Models.CinemaContext;
using System.Text.Json;
using moviesAPI.Interfaces;
using moviesAPI.Services;
using moviesAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSwaggerGen(d =>
{
    d.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movies API",
        Version = "v2",
        Description =
                "The system provides the ability to transform Excel to JSON(Import) and JSON to Excel(Export).\n\nAccess to the system functionality is provided via API."
    }
    );
});

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddScoped<GenericCinemaRepository>();
builder.Services.AddScoped<IPdfTransformService, PdfTransformService>();
builder.Services.AddTransient<EntityValidator>();
builder.Services.AddEndpointsApiExplorer();

//resolve fonts once
MyFontResolver.Apply();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
