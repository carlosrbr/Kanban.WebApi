namespace Kanban.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Kanban.Domain.Interfaces.Service;

    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IValidator<Card> _cardValidator;

        public CardService(ICardRepository cardRepository, IValidator<Card> cardValidator)
        {
            _cardRepository = cardRepository;
            _cardValidator = cardValidator;
        }

        public Result<Card> Add(Card card)
        {
            card.Id = Guid.NewGuid();
            var validationResult = _cardValidator.Validate(card);

            if (!validationResult.IsValid)
            {
                return new Result<Card>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = card
                };
            }

            try
            {
                _cardRepository.Add(card);
                return new Result<Card> { Success = true, Model = card };
            }
            catch (Exception ex)
            {
                return new Result<Card>
                {
                    Success = false,
                    Errors = new List<string> { ex.Message },
                    Model = card
                };
            }
        }

        public Result<Card> Delete(Guid card)
        {

            var existingCard = _cardRepository.FindById(card);

            if (existingCard == null)
            {
                return new Result<Card>
                {
                    Success = false,
                    Errors = new List<string> { "card não encontrado" }
                };

            }

            _cardRepository.Delete(card);

            return new Result<Card>
            {
                Success = true,
                Model = existingCard!
            };
        }

        public Card FindById(Guid id)
        {
            return _cardRepository.FindById(id);
        }

        public IEnumerable<Card> GetAll()
        {
            return _cardRepository.GetAll();
        }

        public Result<Card> Update(Card card)
        {
            var validationResult = _cardValidator.Validate(card);

            if (!validationResult.IsValid)
            {
                return new Result<Card>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList(),
                    Model = card
                };
            }

            try
            {
                _cardRepository.Update(card);
                return new Result<Card> { Success = true, Model = card };
            }
            catch (Exception ex)
            {
                return new Result<Card>
                {
                    Success = false,
                    Errors = new List<string> { ex.Message },
                    Model = card
                };
            }
        }
    }
}
