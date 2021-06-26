using Git.Data.Models;

namespace Git.Data
{
    using Microsoft.EntityFrameworkCore;

    public class GitDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }

        public DbSet<Commit> Commits { get; init; }

        public DbSet<Repository> Repositories { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Git;Integrated Security=True;");
            }
        }
    }
}