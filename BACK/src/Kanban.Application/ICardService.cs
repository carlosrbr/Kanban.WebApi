namespace Kanban.Domain.Interfaces.Service
{
    using System.Numerics;
    using Kanban.Application;
    using Kanban.Domain.Entities;

    public interface ICardService
    {
        Result<Card> Add(Card card);

        Card FindById(Guid id);

        Result<Card> Update(Card card);

        Result<Card> Delete(Guid card);

        IEnumerable<Card> GetAll();
    }
}
