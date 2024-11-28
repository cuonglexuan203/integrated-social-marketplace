using Chat.Application.Dtos;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Queries
{
    public class SearchMessagesQuery: IRequest<IList<MessageDto>>
    {
        public string RoomId { get; set; }
        public string Keyword { get; set; }
    }

    public class SearchMessagesHandler : IRequestHandler<SearchMessagesQuery, IList<MessageDto>>
    {
        private readonly ILogger<SearchMessagesQuery> _logger;
        private readonly IChatService _chatService;

        public SearchMessagesHandler(ILogger<SearchMessagesQuery> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService;
        }
        public async Task<IList<MessageDto>> Handle(SearchMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _chatService.SearchMessagesAsync(request.RoomId, request.Keyword, cancellationToken);
            var messageDtos = ChatMapper.Mapper.Map<List<MessageDto>>(messages);

            return messageDtos;
        }
    }
}
