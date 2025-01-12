namespace webapi_kanban.Controllers
{
    using Kanban.Application.Interfaces;
    using Kanban.Application.ViewModels;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardAppService _cardAppService;

        public CardsController(ICardAppService cardAppService)
        {
            _cardAppService = cardAppService;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            return Ok(_cardAppService.GetAll());
        }


        [HttpPost]
        public IActionResult CreateCard([FromBody] CardViewModel card)
        {

            var result = _cardAppService.Add(card);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetCards), new { id = result.Model.Id }, result.Model); ;
            }

            return BadRequest(result.Errors);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateCard(Guid id, [FromBody] CardViewModel card)
        {
            var existingCard = _cardAppService.FindById(id);


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

            Result<CardViewModel> result = _cardAppService.Update(existingCard);

            if (result.Success)
            {
                return Ok(result.Model); ;
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCard(Guid id)
        {
            _cardAppService.Delete(id);

            return Ok(_cardAppService.GetAll()); ;
        }
    }
}
