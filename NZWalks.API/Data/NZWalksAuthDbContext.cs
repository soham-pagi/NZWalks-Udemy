using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> authDbContextOptions) : base(authDbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "dc541d0c-31e7-4430-9705-5cd9636ef4bb";
            var writerRoleId = "e992cbbb-7dfa-4f05-a5af-c4b67fec4a06";

            var roles = new List<IdentityRole>() 
            {
                new()
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new()
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
