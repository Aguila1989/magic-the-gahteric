using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Wrappers;

namespace HWebAPI.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository _artistRepo;
        private readonly IMapper _mapper;

        public ArtistController(IArtistRepository repo, IMapper mapper)
        {
            _artistRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtists([FromServices] IConfiguration config)
        {
            return (_artistRepo.GetArtists() is IQueryable<Artist> allArtists)
                ? Ok(await allArtists.ProjectTo<ArtistDTO>(_mapper.ConfigurationProvider).ToListAsync())
                : NotFound(new Response<ArtistDTO>()
                    {
                        Succeeded = false,
                        Errors = new string[] { "404" },
                        Message = $"No artists where found"
                    });
        }
    }
}