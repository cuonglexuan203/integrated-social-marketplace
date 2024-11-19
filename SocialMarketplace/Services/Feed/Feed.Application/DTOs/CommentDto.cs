﻿
using Feed.Core.Entities;
using Feed.Core.ValueObjects;

namespace Feed.Application.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public CompactUser User { get; set; }
        public List<Media> Media { get; set; }
        public string CommentText { get; set; }
        public int LikesCount { get; set; }
        public List<Reaction> Reactions { get; set; }
        public string? ParentCommentID { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}