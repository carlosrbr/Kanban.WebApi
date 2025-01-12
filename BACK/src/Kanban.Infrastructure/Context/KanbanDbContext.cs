namespace Kanban.Infrastructure.Data.Context
{
    using Kanban.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class KanbanDbContext : DbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options)
            : base(options)
        { }

        public DbSet<Card> Cards { get; set; }
    }
}
 