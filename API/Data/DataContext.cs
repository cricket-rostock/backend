using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        private readonly DbContextOptions _options;

        public DataContext(DbContextOptions options)
            : base(options) => this._options = options;

        public DbSet<AppUser> Users { get; set; }
    }
}
