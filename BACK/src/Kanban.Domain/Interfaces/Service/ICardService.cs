namespace Kanban.Domain.Interfaces.Service
{
    using Kanban.Domain.Entities;

    public interface ICardService
    {
        Card Add(Card card);

        Card? FindById(Guid id);

        Card Update(Card card);

        Card Delete(Guid card);

        IEnumerable<Card> GetAll();
    }
}
