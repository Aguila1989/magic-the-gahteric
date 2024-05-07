using GraphQL.Server;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL.GraphQL.Schemas;
using Microsoft.EntityFrameworkCore;
using Howest.MagicCards.DAL.DBContext;
using GraphQL.Server.Ui.Playground;

var builder = WebApplication.CreateBuilder(args);

ConfigureDatabase(builder.Services, builder.Configuration);
ConfigureGraphQL(builder.Services);

var app = builder.Build();
app.UseGraphQLPlayground();
app.UseGraphQL<RootSchema>();

void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    services.AddDbContext<MTGContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("MagicCardsDb")));

    services.AddScoped<ICardRepository, SQLCardRepository>();
    services.AddScoped<IArtistRepository, SQLArtistRepository>();
}

void ConfigureGraphQL(IServiceCollection services)
{
    services.AddScoped<RootSchema>();
    services.AddGraphQL()
            .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
            .AddDataLoader()
            .AddSystemTextJson();
}

app.Run();

