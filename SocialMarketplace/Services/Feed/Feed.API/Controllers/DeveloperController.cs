using Feed.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Feed.API.Controllers
{
    public class DeveloperController: ApiController
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public DeveloperController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new
            {
                Posts = await _postRepository.GetAllPostsAsync(),
                Comments = await _commentRepository.GetAllCommentsAsync()
            });
        }
    }
}
