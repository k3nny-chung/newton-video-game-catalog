using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VideoGamesCatalog.Core.Data.Context;
using VideoGamesCatalog.Core.Data.Repository;
using VideoGamesCatalog.Core.Services;
using VideoGamesCatalog.Server.Mapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conString = builder.Configuration.GetConnectionString("VideoGameDatabase") ??
     throw new InvalidOperationException("Connection string VideoGameDatabase not found");
builder.Services.AddDbContext<VideoGameContext>(options =>
    options.UseSqlServer(conString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVideoGameService, VideoGameService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IVideoGameService, VideoGameService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAgeRatingService, AgeRatingService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
