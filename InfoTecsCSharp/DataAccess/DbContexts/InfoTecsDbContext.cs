using InfoTecsCSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoTecsCSharp.DataAccess.DbContexts
{
    public class InfoTecsDbContext : DbContext
    {
        public InfoTecsDbContext(DbContextOptions<InfoTecsDbContext> options)
            : base(options) {}

        public DbSet<FileDescription> Files { get; set; }
        public DbSet<ResultModel> Results { get; set; }
        public DbSet<TableEntry> TableEntries { get; set; }
    }
}
