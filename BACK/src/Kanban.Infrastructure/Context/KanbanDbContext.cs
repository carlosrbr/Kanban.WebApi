using Kanban.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Infrastructure.Data.Context
{
    public class KanbanDbContext : DbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options)
            : base(options)
        { }

        public DbSet<Card> Cards { get; set; }
    }
}
