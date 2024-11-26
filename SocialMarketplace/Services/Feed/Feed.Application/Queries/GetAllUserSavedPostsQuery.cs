
using Feed.Application.DTOs;
using MediatR;

namespace Feed.Application.Queries
{
    public class GetAllUserSavedPostsQuery: IRequest<IList<SavedPostDto>>
    {
        public string UserId { get; set; }

        public GetAllUserSavedPostsQuery(string userId)
        {
            UserId = userId;
        }
    }
}
