﻿using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
{
    public class RarityProfile : Profile
    {
        public RarityProfile()
        {
            CreateMap<Rarity, RarityDTO>();

        }

    }
}