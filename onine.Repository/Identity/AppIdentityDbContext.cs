
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using onine.core.Entites.Identity;

namespace onine.Repository.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(
            DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasOne(u => u.Address)
                .WithOne(a => a.AppUser)
                .HasForeignKey<Address>(a => a.AppUserId);
        }
    }
}
