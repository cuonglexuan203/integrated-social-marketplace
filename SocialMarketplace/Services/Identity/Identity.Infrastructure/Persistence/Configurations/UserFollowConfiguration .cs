using Identity.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.HasKey(uf => uf.Id);
            builder.Property(uf => uf.Id)
                    .ValueGeneratedOnAdd();

            // Configure relationships
            builder.HasOne(uf => uf.Follower)
                   .WithMany(u => u.Following)
                   .HasForeignKey(uf => uf.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uf => uf.Followed)
                   .WithMany(u => u.Followers)
                   .HasForeignKey(uf => uf.FollowedId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Create a unique index to prevent duplicate follows
            builder.HasIndex(uf => new { uf.FollowerId, uf.FollowedId })
                   .IsUnique();
        }
    }
}
