﻿using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Mappings
{
    public class CardProfile : Profile
    {
        public CardProfile() 
        {
            CreateMap<Card, CardDTO>()
                .ForMember(dto => dto.ImageUrl,
                            opt => opt.MapFrom(c => c.OriginalImageUrl));

            CreateMap<Card, CardDetailDTO>()
                .ForMember(dto => dto.ImageUrl,
                            opt => opt.MapFrom(c => c.OriginalImageUrl));

        }

    }
}