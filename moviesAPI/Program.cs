using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using moviesAPI.FileTransform;
using moviesAPI.Interfaces;
using moviesAPI.Models.dbContext;
using moviesAPI.Services;
using System.Text.Json;
using System.Xml.Xsl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MovieCinemaLabContext>(option => option.UseSqlServer(
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
        Title = "Excel adapter",
        Version = "v1",
        Description =
                "The system provides the ability to transform Excel to JSON(Import) and JSON to Excel(Export).\n\nAccess to the system functionality is provided using API."
    }
    );
});
builder.Services.AddScoped<IMovieFilterService, MovieFilterService>();
builder.Services.AddScoped<ISessionFilterService, SessionFilterService>();
builder.Services.AddScoped<IFileTransform, PdfTransform>();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
