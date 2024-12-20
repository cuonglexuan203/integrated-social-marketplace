﻿using Feed.API.Middlewares;
using Feed.Application.Extensions;
using Feed.Application.Interfaces;
using Feed.Application.Interfaces.HttpClients;
using Feed.Application.Interfaces.Services;
using Feed.Application.Services;
using Feed.Core.Repositories;
using Feed.Infrastructure.Configurations;
using Feed.Infrastructure.Extensions;
using Feed.Infrastructure.Persistence.DbContext;
using Feed.Infrastructure.Persistence.Repositories;
using Feed.Infrastructure.Services;
using Feed.Infrastructure.Services.HttpClients;
using Feed.Infrastructure.Validation;
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
            #region binding configuration
            services.Configure<DatabaseSettings>(_configuration.GetSection(nameof(DatabaseSettings)));
            services.Configure<CloudinarySettings>(_configuration.GetSection(nameof(CloudinarySettings)));
            var dbSettings = _configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            #endregion
            #region add exception handlers
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            #endregion

            services.AddProblemDetails();
            services.AddControllers();
            services.AddApiVersioning();

            services.AddHealthChecks()
                .AddMongoDb(dbSettings.ConnectionString, "Feed MongoDB Health Check"
                , HealthStatus.Degraded);
            services.AddInfrastructure();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feed.API", Version = "v1" }); 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token below. Example: 'your-token'",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddApplicationServices();
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
            services.AddScoped<ISavedPostRepository, SavedPostRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUserReactionRepository, UserReactionRepository>();
            services.AddScoped<IUserCommentsRepository, UserCommentRepository>();
            services.AddScoped<IUserShareRepository, UserShareRepository>();
            services.AddScoped<IPostScoringService, PostScoringService>();
            services.AddScoped<IUserCredibilityUpdateService, UserCredibilityUpdateService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IPostMappingService, PostMappingService>();
            services.AddScoped<IPostRankingService, PostRankingService>();
            services.AddScoped<IReportMappingService, ReportMappingService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpClient<IRecommendationService, RecommendationService>();
            services.AddScoped<IMongoIdValidator, MongoIdValidator>();
            #endregion
            #region CORS registration
            services.AddCors(options =>
            {
                options.AddPolicy("sm-web-policy", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
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
            app.UseExceptionHandler();
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
