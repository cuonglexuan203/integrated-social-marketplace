
namespace Feed.Infrastructure.Configurations
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PostsCollection { get; set; }
        public string CommentsCollection { get; set; }
        public string SavedPostsCollection { get; set; }
        public string ReportsCollection { get; set; }
        public string UserReactionsCollection { get; set; }
        public string UserSharesCollection { get; set; }
        public string UserCommentsCollection { get; set; }
    }
}
