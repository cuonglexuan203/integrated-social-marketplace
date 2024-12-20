﻿
using Feed.Core.Entities;
using Feed.Core.Interfaces;
using Feed.Core.Specs;
using Feed.Core.ValueObjects;

namespace Feed.Core.Repositories
{
    public interface IPostRepository: ISoftDeletable<Post>
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Pagination<Post>> GetPostsAsync(PostSpecParams postParams, CancellationToken token = default);
        Task<Post> GetPostAsync(string id, CancellationToken token = default);
        Task<Comment> AddCommentToPostAsync(Comment comment);
        //Task<IEnumerable<Post>> GetPostByUserId(string userId);
        Task<Post> CreatePostAsync(Post post, CancellationToken cancellationToken = default);
        Task<bool> UpdatePostAsync(Post post, CancellationToken cancellationToken = default);
        //Task<bool> DeletePost(string id);
        Task<bool> IsPostExistsAsync(string id);
        Task<Reaction> AddReacionToPostAsync(string postId, Reaction reaction, CancellationToken cancellationToken = default);
        Task<bool> RemoveReactionFromPostAsync(string postId, string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Reaction>> GetAllReactionsByPostId(string postId, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string postId, CancellationToken token = default);
        Task<IEnumerable<Post>> GetAllUserPostsAsync(string userId, CancellationToken cancellationToken = default);
        Task<Pagination<Post>> GetPostsReactedByUserIdAsync(string userId, ReactionSpecParams reactionParams, CancellationToken token = default);
        Task<Post> GetPostByCommentId(string commentId, CancellationToken token = default);
        Task<long> CountPostsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetTopPostIdsByScoreAsync(int take);
        Task<long> CountPostAsync();
        Task<IEnumerable<Post>> SearchPostsByUserIdAsync(string userId, string keyword, CancellationToken cancellationToken = default);
    }
}
