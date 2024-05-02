using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<CardDTO>> GetCards()
        {
            return Ok(_cardRepo.GetAllCards().
                ProjectTo<CardDTO>(_mapper.ConfigurationProvider));
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
