using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAPI.Wrappers;

namespace HWebAPI.Controllers
{
    [ApiVersion("1.1"), ApiVersion("1.5")]
    [Route("api/v{version:apiVersion}/rarities")]
    [ApiController]
    public class RarityController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public RarityController(IRarityRepository repo, IMapper mapper, IMemoryCache cache)
        {
            _rarityRepo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RarityDTO>>> GetAllRarities([FromServices] IConfiguration config)
        {
            string cacheKey = "Rarities";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<RarityDTO> cachedResult))
            {
                var allRarities = await _rarityRepo.GetAllRarities();

                if (allRarities != null)
                {
                    cachedResult = await allRarities
                        .ProjectTo<RarityDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
                    };
                    _cache.Set(cacheKey, cachedResult, cacheOptions);
                }
                else
                {
                    return NotFound(new Response<RarityDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No rarities were found"
                    });
                }
            }

            return Ok(cachedResult);
        }

    }
}