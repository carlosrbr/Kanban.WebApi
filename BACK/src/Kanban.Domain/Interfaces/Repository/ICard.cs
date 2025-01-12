namespace Kanban.Domain.Interfaces.Repository
{
    using Kanban.Domain.Entities;

    public interface ICardRepository : IRepository<Card>
    {
        Card? FindById(Guid id);
    }
}
