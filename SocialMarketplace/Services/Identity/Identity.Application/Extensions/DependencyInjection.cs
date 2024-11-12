using Identity.Application.Commands.User.Create;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Identity.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
            });

            return services;
        }
    }
}
