using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.Mappings;
using Microsoft.OpenApi.Models;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<MTGContext>
    (options => options.UseSqlServer(config.GetConnectionString("MagicTheGatheringDb")));
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(new Type[] { typeof(CardProfile), typeof(ArtistProfile) });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICardRepository, SQLCardRepository>();
builder.Services.AddScoped<IArtistRepository, SQLArtistRepository>();
builder.Services.AddScoped<ITypeRepository, SQLTypeRepository>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "Cards API version 1.1",
        Version = "v1.1",
        Description = "API for card management"
    });
    c.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "Cards API version 1.5",
        Version = "v1.5",
        Description = "API for card management: with extra sorting and card detail"
    });
});

builder.Services.AddApiVersioning(
    options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1.1);
    }
    ).AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1.1/swagger.json", "v1.1");
        c.SwaggerEndpoint("/swagger/v1.5/swagger.json", "v1.5");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
