namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SharedTripDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }

        public SharedTripDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(ut => new { ut.TripId, ut.UserId });
            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.Trip)
                .WithMany(c => c.UserTrips)
                .HasForeignKey(ut => ut.TripId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            modelBuilder.Entity<UserTrip>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTrips)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
