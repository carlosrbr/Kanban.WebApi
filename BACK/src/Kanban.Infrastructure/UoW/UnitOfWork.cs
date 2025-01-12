using Kanban.Infrastructure.Data.Context;
using Kanban.Infrastructure.Data.Interfaces;

namespace EP.CrudModalDDD.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KanbanDbContext _context;
        private bool _disposed;

        public UnitOfWork(KanbanDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}