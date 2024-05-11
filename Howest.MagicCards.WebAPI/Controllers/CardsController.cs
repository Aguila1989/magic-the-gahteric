using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared.Extensions;
using WebAPI.Wrappers;


namespace WebAPI.Controllers.V1_1
{
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public CardsController(IMapper mapper, ICardRepository repo, IMemoryCache cache)
        {
            _cardRepo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<CardDTO>>>> GetCards([FromQuery] CardFilter filter)
        {
            IQueryable<Card> cards = await _cardRepo.GetAllCards();

            if (cards == null)
            {
                return BadRequest(new Response<CardDTO>()
                {
                    Succeeded = false,
                    Errors = ["400"],
                    Message = "No cards were found."
                });
            }

            string cacheKey = $"Cards_{filter.PageNumber}_{filter.PageSize}_{filter.SetCode}_{filter.Type}_{filter.Name}_{filter.Text}_{filter.Artist}_{filter.RarityCode}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<CardDTO> cachedResult))
            {
                cachedResult = await cards
                    .ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                    .ToPagedList(filter.PageNumber, filter.PageSize)
                    .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                };

                _cache.Set(cacheKey, cachedResult, cacheOptions);
            }

            int totalRecords = cards
                .ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                .Count();

            int totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            return Ok(new PagedResponse<IEnumerable<CardDTO>>(cachedResult, filter.PageNumber, filter.PageSize)
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages
            });
        }
    }

    namespace WebAPI.Controllers.V1_5
    {
        [ApiVersion("1.5")]
        [Route("api/v{version:apiVersion}/[controller]")]
        [ApiController]
        public class CardsController : ControllerBase
        {
            private readonly ICardRepository _cardRepo;
            private readonly IMapper _mapper;
            private readonly IMemoryCache _cache;
            public CardsController(IMapper mapper, ICardRepository repo, IMemoryCache cache)
            {
                _cardRepo = repo;
                _mapper = mapper;
                _cache = cache;
            }

            [HttpGet]
            public async Task<ActionResult<PagedResponse<IEnumerable<CardDTO>>>> GetCards([FromQuery] CardFilter filter)
            {

                string cacheKey = $"Cards_{filter.PageNumber}{filter.PageSize}{filter.SetCode}{filter.Type}{filter.Name}{filter.Text}{filter.Artist}_{filter.RarityCode}";

                PagedResponse<IEnumerable<CardDTO>> cachedResponse = _cache.Get<PagedResponse<IEnumerable<CardDTO>>>(cacheKey);

                if (cachedResponse != null)
                {
                    return Ok(cachedResponse);
                }

                IQueryable<Card> cards = await _cardRepo.GetAllCards();

                if (cards == null)
                {
                    return BadRequest(new Response<CardDTO>()
                    {
                        Succeeded = false,
                        Errors = ["400"],
                        Message = "No cards were found."
                    });
                }

                IEnumerable<CardDTO> searchResult = await cards
                        .ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                        .ToPagedList(filter.PageNumber, filter.PageSize)
                        .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

                int totalRecords = cards
                    .ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                    .Count();

                int totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

                PagedResponse<IEnumerable<CardDTO>> response = new PagedResponse<IEnumerable<CardDTO>>(searchResult, filter.PageNumber, filter.PageSize)
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages
                };

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                };

                _cache.Set(cacheKey, response, cacheOptions);

                return Ok(response);

            }


            [HttpGet("{id:int}")]
            public async Task<ActionResult<CardDetailDTO>> GetCard(int id)
            {
                string cacheKey = $"Card_{id}";

                if (!_cache.TryGetValue(cacheKey, out CardDetailDTO cachedResult))
                {
                    var card = await _cardRepo.GetCardById(id);

                    if (card != null)
                    {
                        cachedResult = _mapper.Map<CardDetailDTO>(card);
                        MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                        };
                        _cache.Set(cacheKey, cachedResult, cacheOptions);
                    }
                    else
                    {
                        return NotFound(new Response<CardDetailDTO>()
                        {
                            Succeeded = false,
                            Errors = new string[] { "404" },
                            Message = $"No card found with id {id}"
                        });
                    }
                }

                return Ok(cachedResult);
            }



        }
    }
}