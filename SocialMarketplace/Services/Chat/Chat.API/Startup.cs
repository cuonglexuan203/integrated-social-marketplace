using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Chat.Infrastructure.Configurations;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Chat.Infrastructure.Persistence.DbContext;
using Chat.Core.Repositories;
using Chat.Infrastructure.Persistence.Repositories;
using Chat.API.Hubs;

namespace Chat.API
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
            #region binding configuration
            services.Configure<DatabaseSettings>(_configuration.GetSection("DatabaseSettings"));
            services.Configure<JwtSettings>(_configuration.GetSection("Jwt"));
            var dbSettings = _configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>();
            #endregion
            services.AddProblemDetails();
            services.AddControllers();
            services.AddApiVersioning();
            services.AddHealthChecks()
                .AddMongoDb(dbSettings.ConnectionString, "Chat MongoDB Health Check"
                , HealthStatus.Degraded);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat.API", Version = "v1" }); });
            services.AddHttpContextAccessor();
            services.AddSignalR();

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/chatHub"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            #region services registration
            services.AddScoped<IChatContext, ChatContext>();
            services.AddScoped<IChatRepository, ChatRepository>();

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat.API v1"));
            }
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("sm-web-policy");
            app.UseAuthentication();
            //app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
