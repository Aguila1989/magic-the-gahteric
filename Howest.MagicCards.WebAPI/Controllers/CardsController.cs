using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Wrappers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        private readonly IMapper _mapper;
        public CardsController(IMapper mapper, ICardRepository repo)
        {
            _cardRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<PagedResponse<IEnumerable<CardDTO>>> GetCards([FromQuery] CardFilter filter)
        {
            return Ok(new PagedResponse<IEnumerable<CardDTO>>(
                _cardRepo.GetAllCards()
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                .ToList(),
             filter.PageNumber,
             filter.PageSize)
          );
        }

        [HttpGet("{id:int}")]
        public ActionResult<CardDetailDTO> GetCard(int id)
        {
            return (_cardRepo.GetCardbyId(id) is Card card)
                ? Ok(_mapper.Map<CardDetailDTO>(card))
                : NotFound(new Response<String>() { Message = "No card was found" });
        }


    }
}
