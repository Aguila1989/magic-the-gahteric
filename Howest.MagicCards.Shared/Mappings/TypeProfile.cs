using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using Type = Howest.MagicCards.DAL.Models.Type;

namespace Howest.MagicCards.Shared.Mappings
{
    internal class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<Type, TypeDTO>();
        }
    }
}