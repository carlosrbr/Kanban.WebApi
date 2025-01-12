namespace webapi_kanban.Controllers
{
    using Kanban.Application;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public IActionResult GetCards()
        {

            return Ok(_cardService.GetAll());
        }


        [HttpPost]
        public IActionResult CreateCard([FromBody] Card card)
        {

            Result<Card> result = _cardService.Add(card);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetCards), new { id = result.Model.Id }, result.Model); ;
            }

            return BadRequest(result.Errors);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateCard(Guid id, [FromBody] Card card)
        {
            var existingCard = _cardService.FindById(id);


            if (existingCard == null)
            {
                return NotFound();
            }

            if (existingCard.Id != card.Id)
            {
                return BadRequest();
            }

            existingCard.Titulo = card.Titulo;
            existingCard.Conteudo = card.Conteudo;
            existingCard.Lista = card.Lista;

            Result<Card> result = _cardService.Update(existingCard);

            if (result.Success)
            {
                return Ok(result.Model); ;
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCard(Guid id)
        {
            _cardService.Delete(id);

            return Ok(_cardService.GetAll()); ;
        }
    }
}
