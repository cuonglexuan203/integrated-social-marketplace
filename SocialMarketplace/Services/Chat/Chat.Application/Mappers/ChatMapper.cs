
using AutoMapper;
using Chat.Application.Mappers.Profiles;

namespace Chat.Application.Mappers
{
    public class ChatMapper
    {
        private static Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                #region adding profiles
                cfg.AddProfile<ChatRoomProfile>();
                cfg.AddProfile<MessageProfile>();
                cfg.AddProfile<UserProfile>();
                #endregion
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => _mapper.Value;
    }
}
