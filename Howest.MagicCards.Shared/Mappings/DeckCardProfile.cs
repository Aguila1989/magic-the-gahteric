using AutoMapper;
using Howest.MagicCards.DAL.Models.MongoDbModels;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class DeckCardProfile : Profile
    {
        public DeckCardProfile()
        {
            CreateMap<DeckCard, DeckCardDTO>();
        }

    }
}