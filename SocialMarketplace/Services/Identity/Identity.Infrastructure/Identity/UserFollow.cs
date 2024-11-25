
namespace Identity.Infrastructure.Identity
{
    public class UserFollow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        public string FollowedId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;
        //public bool IsActive { get; set; }

        // Navigation properties
        public virtual ApplicationUser Follower { get; set; }
        public virtual ApplicationUser Followed { get; set; }
    }
}
