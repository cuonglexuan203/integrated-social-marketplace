using Identity.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Identity.Infrastructure.Persistence.DbContext
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);
             modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
         }
        */


    }
}
