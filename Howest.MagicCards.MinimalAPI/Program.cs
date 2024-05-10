using BookStoreApi.Models;
using Howest.MagicCards.DAL.Repositories.MongoDB;
using Howest.MagicCards.MinimalAPI.Extensions;

const string commonPrefix = "/api";

var (builder, services, conf) = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MtgDatabase"));
builder.Services.AddSingleton<DeckCardRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string urlPrefix = conf["ApiPrefix"] ?? commonPrefix;

var studentGroup = app.MapGroup($"{urlPrefix}/deckCards")
                      .WithTags("DeckCards"); ;
var deckCardRepository = app.Services.GetRequiredService<DeckCardRepository>();

studentGroup.MapDeckCardsEndpoints(urlPrefix, deckCardRepository);


app.Run();

