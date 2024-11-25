using AutoMapper;
using Identity.Infrastructure.Mappers.Profiles;

namespace Identity.Infrastructure.Mappers
{
    public class IdentityMapper
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<UserProfile>();
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => _mapper.Value;
    }
}
