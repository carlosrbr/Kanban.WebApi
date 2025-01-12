namespace Kanban.Domain.Interfaces.Repository
{
    using System;
    using System.Collections.Generic;

    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Add(TEntity obj);
        IEnumerable<TEntity> GetAll();
        TEntity Update(TEntity obj);
        void Delete(Guid id);
        int SaveChanges();
    }
}
