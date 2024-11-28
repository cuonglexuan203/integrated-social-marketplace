using Chat.Application.Dtos;
using Chat.Application.Interfaces.HttpClients;
using Chat.Application.Interfaces.Services;
using Chat.Application.Mappers;
using Chat.Core.Entities;
using Chat.Core.Enums;
using Chat.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Chat.Application.Commands
{
    public class SaveMessageCommand: IRequest<MessageDto>
    {
        public string RoomId { get; set; }
        public string SenderId { get; set; }
        public string? ContentText { get; set; }
        public IFormFile[]? Media { get; set; }
        public string[]? AttachedPostIds { get; set; }
    }

    public class SaveMessageHandler : IRequestHandler<SaveMessageCommand, MessageDto>
    {
        private readonly ILogger<SaveMessageHandler> _logger;
        private readonly IChatService _chatService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IFeedService _feedService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SaveMessageHandler(ILogger<SaveMessageHandler> logger, IChatService chatService, ICloudinaryService cloudinaryService,
            IFeedService feedService)
        {
            _logger = logger;
            _chatService = chatService;
            _cloudinaryService = cloudinaryService;
            _feedService = feedService;
        }
        public async Task<MessageDto> Handle(SaveMessageCommand request, CancellationToken cancellationToken)
        {

            var message = new Message
            {
                RoomId = request.RoomId,
                SenderId = request.SenderId,
                ContentText = request.ContentText,
                Status = MessageStatus.Sent,
            };

            if (request.Media != null)
            {
                var mediaDtos = await _cloudinaryService.UploadMultipleFilesAsync(request.Media);
                var mediaList = ChatMapper.Mapper.Map<List<Media>>(mediaDtos);
                message.Media = mediaList;
            }

            if(request.AttachedPostIds != null && request.AttachedPostIds.Any())
            {
                foreach (var postId in request.AttachedPostIds)
                {
                    try
                    {
                        var postDto = await _feedService.GetPostAsync(postId, cancellationToken);
                        var postReference = ChatMapper.Mapper.Map<PostReference>(postDto);
                        message.AttachedPosts.Add(postReference);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in getting the attached post {postId}: {errorMsg}", postId, ex.Message);
                    }
                }
            }

            var savedMessage = await _chatService.SaveMessageAsync(message, cancellationToken);
            var savedMessageDto = ChatMapper.Mapper.Map<MessageDto>(savedMessage);

            return savedMessageDto;
            
        }
    }
}
