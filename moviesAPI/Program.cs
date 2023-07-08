using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using moviesAPI.Repositories;
using moviesAPI.FileTransform;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaContext>(option => option.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddSwaggerGen(d =>
{
    d.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Movies API",
        Version = "v1",
        Description =
                "The system provides the ability to transform Excel to JSON(Import) and JSON to Excel(Export).\n\nAccess to the system functionality is provided using API."
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

builder.Services.AddSingleton<CinemaRepository>();
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
