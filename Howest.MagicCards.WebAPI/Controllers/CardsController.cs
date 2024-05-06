using Asp.Versioning;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
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
        public CardsController(IMapper mapper, ICardRepository repo)
        {
            _cardRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<PagedResponse<IEnumerable<CardDTO>>> GetCards([FromQuery] CardFilter filter)
        {
            IQueryable<Card> cards = _cardRepo.GetAllCards();
            if (cards == null)
            {
                return BadRequest(new Response<CardDTO>()
                {
                    Succeeded = false,
                    Errors = ["400"],
                    Message = "No cards where found."
                });
            }
            return Ok(new PagedResponse<IEnumerable<CardDTO>>(
                cards
                .ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                .Sort(filter.OrderByNameAsc)
                .ToPagedList(filter.PageNumber, filter.PageSize)
                .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                .ToList(),
             filter.PageNumber,
             filter.PageSize)
            {
                TotalRecords = cards.ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                                    .Count(),
                TotalPages = (int)Math.Ceiling(cards.ToFilteredList(filter.SetCode, filter.Type, filter.Name, filter.Text, filter.Artist, filter.RarityCode)
                                    .Count()/ (double)filter.PageSize)
            }
          );
        }

        [HttpGet("{id:int}")]
        public ActionResult<CardDetailDTO> GetCard(int id)
        {
            return (_cardRepo.GetCardbyId(id) is Card card)
                ? Ok(_mapper.Map<CardDetailDTO>(card))
                : NotFound(new Response<CardDetailDTO>()
                {
                    Succeeded = false,
                    Errors = new string[] { "404" },
                    Message = $"No card found with id {id}"
                }
                    );
        }


    }
}
