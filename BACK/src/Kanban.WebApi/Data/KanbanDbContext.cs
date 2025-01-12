using Microsoft.EntityFrameworkCore;
using webapi_kanban.dto;


namespace KanbanWebApi.Data
{
    public class KanbanDbContext : DbContext
    {
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options) { }
        public DbSet<CardDto> Cards { get; set; }
    }
}
