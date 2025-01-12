namespace webapi_kanban.Controllers
{
    using KanbanWebApi.Data;
    using Microsoft.AspNetCore.Mvc;
    using webapi_kanban.dto;

    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly KanbanDbContext _context;

        public CardsController(KanbanDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            return Ok(_context.Cards);
        }


        [HttpPost]
        public IActionResult CreateCard([FromBody] CardDto card)
        {
            if (string.IsNullOrEmpty(card.Titulo) 
                || string.IsNullOrEmpty(card.Conteudo) 
                || string.IsNullOrEmpty(card.Lista))
            {
                return BadRequest();
            }
                

            card.Id = Guid.NewGuid();
            _context.Cards.Add(card);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCards), new { id = card.Id }, card);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCard(Guid id, [FromBody] CardDto card)
        {
            var existingCard = _context.Cards.Find(id);
            if (existingCard == null)
            {
                return NotFound();
            }

            existingCard.Titulo = card.Titulo;
            existingCard.Conteudo = card.Conteudo;
            existingCard.Lista = card.Lista;

            _context.SaveChanges();
            return Ok(existingCard);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCard(Guid id)
        {
            var card = _context.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            _context.SaveChanges();

            return Ok(_context.Cards);
        }
    }
}
