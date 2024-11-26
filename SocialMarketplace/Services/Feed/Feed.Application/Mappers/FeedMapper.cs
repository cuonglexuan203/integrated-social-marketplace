using AutoMapper;
using Feed.Application.Mappers.Profiles;

namespace Feed.Application.Mappers
{
    public class FeedMapper
    {
        private static readonly Lazy<IMapper> _lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<PostMappingProfile>();
                cfg.AddProfile<CommentMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<SavedPostMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => _lazy.Value;
    }
}
