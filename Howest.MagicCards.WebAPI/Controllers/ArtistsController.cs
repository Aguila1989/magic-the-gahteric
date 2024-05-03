using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IQueryable<ArtistDTO>> GetArtists([FromServices] IConfiguration config)
        {
            return (_artistRepo.GetArtists() is IQueryable<Artist> allArtists)
                ? Ok(allArtists.ProjectTo<ArtistDTO>(_mapper.ConfigurationProvider))
                : NotFound();
        }
    }
}