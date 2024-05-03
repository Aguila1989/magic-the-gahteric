using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.Shared.Mappings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<CardsContext>
    (options => options.UseSqlServer(config.GetConnectionString("MagicTheGatheringDb")));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(new Type[] { typeof(CardProfile), typeof(ArtistProfile) });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICardRepository, SQLCardRepository>();
builder.Services.AddScoped<IArtistRepository, SQLArtistRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
