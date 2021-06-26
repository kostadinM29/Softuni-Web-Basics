using BattleCards.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleCards.Data
{

    public class BattleCardsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserCard> UsersCards { get; set; }

        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>()
                .HasKey(uc => new { uc.CardId, uc.UserId });
            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.Card)
                .WithMany(c => c.UserCards)
                .HasForeignKey(uc => uc.CardId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            modelBuilder.Entity<UserCard>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCards)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
