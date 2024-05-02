using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepo;
        public BooksController()
        {
            _cardRepo = new SQLCardRepository();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Card>> GetCards()
        {
            return Ok(_cardRepo.GetAllCards());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Card> GetCard(int id)
        {
            return (_cardRepo.GetCardbyId(id) is Card foundCard)
                    ? Ok(foundCard)
                    : NotFound($"No card is found with id {id}");
        }

        [HttpGet("search")]
       
    }
}
