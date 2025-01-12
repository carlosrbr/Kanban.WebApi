namespace Kanban.Application
{
    using System;
    using System.Collections.Generic;
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Kanban.Domain.Interfaces.Service;

    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card Add(Card card)
        {
            card.Id = Guid.NewGuid();
            _cardRepository.Add(card);

            return card;
        }

        public Card Delete(Guid id)
        {

            var existingCard = _cardRepository.FindById(id);

            if (existingCard == null)
            {
                throw new ArgumentException("card não encontrado.");
            }

            _cardRepository.Delete(id);

            return existingCard;
        }

        public Card? FindById(Guid id)
        {
            return _cardRepository.FindById(id);
        }

        public IEnumerable<Card> GetAll()
        {
            return _cardRepository.GetAll();
        }

        public Card Update(Card card)
        {
            var existingCard = _cardRepository.FindById(card.Id);

            if (existingCard == null)
            {
                throw new ArgumentException("card não encontrado.");
            }

            _cardRepository.Update(existingCard);

            return existingCard;
        }
    }
}
