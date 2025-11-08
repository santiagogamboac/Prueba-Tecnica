using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "df8d34db-8932-45f9-83b9-1f81e96fd1ee",
                    Name = "ADMINISTRADOR",
                    NormalizedName = "ADMINISTRADOR"
                }
            );
        }
    }
}
