using Backend.Identity.Configurations;
using Backend.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Identity.Persistence
{
    public class ShopIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public ShopIdentityContext(DbContextOptions<ShopIdentityContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(builder);
        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
