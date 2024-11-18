using Feed.Application.Handlers;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Core.Repositories;
using Feed.Infrastructure.Configurations;
using Feed.Infrastructure.Persistence.DbContext;
using Feed.Infrastructure.Persistence.Repositories;
using Feed.Infrastructure.Services;
using Feed.Infrastructure.Services.HttpClients;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Feed.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();
            services.AddHealthChecks()
                .AddMongoDb(_configuration["DatabaseSettings:ConnectionString"], "Feed MongoDB Health Check"
                , HealthStatus.Degraded);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feed.API", Version = "v1" }); });
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPostHandler).Assembly));

            #region jwt config
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                    };
                });
            #endregion

            #region service registration
            services.AddScoped<IFeedContext, FeedContext>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.Configure<CloudinarySettings>(_configuration.GetSection("CloudinarySettings"));
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService, IdentityService>();
            #endregion

            #region CORS registration
            services.AddCors(options =>
            {
                options.AddPolicy("sm-web-policy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feed.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("sm-web-policy");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

    }
}
