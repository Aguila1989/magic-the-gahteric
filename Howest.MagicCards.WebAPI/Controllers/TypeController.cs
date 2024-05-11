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
    [Route("api/v{version:apiVersion}/types")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeRepository _typeRepo;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public TypeController(ITypeRepository repo, IMapper mapper, IMemoryCache cache)
        {
            _typeRepo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetNormalTypes([FromServices] IConfiguration config)
        {
            string cacheKey = "NormalTypes";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TypeDTO> cachedResult))
            {
                var allTypes = await _typeRepo.GetNormalTypes();

                if (allTypes != null)
                {
                    cachedResult = await allTypes
                        .ProjectTo<TypeDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

                    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) // Adjust cache expiration time as needed
                    };
                    _cache.Set(cacheKey, cachedResult, cacheOptions);
                }
                else
                {
                    return NotFound(new Response<TypeDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No types were found"
                    });
                }
            }

            return Ok(cachedResult);
        }

    }
}