using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Models.Entities;

namespace PartyInvitationManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum to be stored as string
            modelBuilder.Entity<Invitation>()
                .Property(e => e.Status)
                .HasConversion<string>();

            // Configure the relationship
            modelBuilder.Entity<Party>()
                .HasMany(p => p.Invitations)
                .WithOne(i => i.Party)
                .HasForeignKey(i => i.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
