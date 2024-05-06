using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Wrappers;

namespace HWebAPI.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ArtistController(IArtistRepository repo, IMapper mapper, IMemoryCache cache)
        {
            _artistRepo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtists([FromServices] IConfiguration config)
        {
            string cacheKey = "Artists";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<ArtistDTO> cachedResult))
            {
                var allArtists = _artistRepo.GetArtists();

                if (allArtists != null)
                {
                    cachedResult = await allArtists
                        .ProjectTo<ArtistDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                    };
                    _cache.Set(cacheKey, cachedResult, cacheOptions);
                }
                else
                {
                    return NotFound(new Response<ArtistDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No artists were found"
                    });
                }
            }

            return Ok(cachedResult);
        }

    }
}