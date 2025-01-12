namespace Kanban.Infrastructure.Data.Repository
{
    using Kanban.Domain.Entities;
    using Kanban.Domain.Interfaces.Repository;
    using Kanban.Infrastructure.Data.Context;

    public class CardRepository : ICardRepository
    {
        private readonly KanbanDbContext _context;

        public CardRepository(KanbanDbContext context)
        {
            _context = context;
        }

        public Card Add(Card obj)
        {
            _context.Cards.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public void Delete(Guid id)
        {
            var card = _context.Cards.Find(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Card não encontrado.");
            }
        }

        public Card? FindById(Guid id)
        {
            return _context.Cards.Find(id);
        }

        public IEnumerable<Card> GetAll()
        {
            return _context.Cards.ToList();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Card Update(Card obj)
        {
            _context.Cards.Update(obj);
            _context.SaveChanges();
            return obj;
        } 

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
