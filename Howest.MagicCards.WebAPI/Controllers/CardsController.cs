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
        public ActionResult<PagedResponse<IEnumerable<CardDTO>>> GetCards([FromQuery] PaginationFilter paginationFilter)
        {
            return Ok(new PagedResponse<IEnumerable<CardDTO>>(
                _cardRepo.GetAllCards()
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                .ToList(),
             paginationFilter.PageNumber,
             paginationFilter.PageSize)
          );
        }

        [HttpGet("{id:int}")]
        public ActionResult<CardDetailDTO> GetCard(int id)
        {
            var card = _cardRepo.GetCardbyId(id);
            if (card == null)
            {
                return NotFound(); 
            }

            var cardDetailDTO = _mapper.Map<CardDetailDTO>(card); 

            return Ok(cardDetailDTO);
        }


    }
}
