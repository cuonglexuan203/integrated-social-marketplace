namespace Feed.Core.Common.Constants
{
    public static class WeightConstants
    {
        public static class User
        {
            // positive interaction
            public static readonly float TotalReactions = 1.0f;
            public static readonly float TotalComments = 2.0f;
            public static readonly float TotalShares = 2.0f;
            // negative interaction
            public static readonly float NegativeInteractions = 1.5f;
            // Report
            public static readonly float ValidReports = 2f;
            public static readonly float InvalidReports = 3f;
            // high engagement
            public static readonly float HighEngagement = 5f;
            // frequent report
            public static readonly float FrequentReports = 2f;

        }
        public static class Post
        {
            public static readonly float Reaction = 1f;
            public static readonly float Comment = 2f;
            public static readonly float Share = 3f;
        }
    }
}
