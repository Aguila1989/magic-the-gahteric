using GraphQL.Server;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL.GraphQL.Schemas;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.DAL.DBContext;
using GraphQL.Server.Ui.Playground;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<MTGContext>(options =>
        options.UseSqlServer(config.GetConnectionString("MagicCardsDb")));

builder.Services.AddScoped<ICardRepository, SQLCardRepository>();
builder.Services.AddScoped<IArtistRepository, SQLArtistRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
        .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
        .AddDataLoader()
        .AddSystemTextJson();

var app = builder.Build();

app.UseGraphQL<RootSchema>();
app.UseGraphQLPlayground(
    "/ui/playground", 
    new PlaygroundOptions()
{
    EditorTheme = EditorTheme.Dark
});


app.Run();

