using System;
using System.Reflection;
using Api.MessageBroker.MessageBrokerConsumers;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Peddle.MessageBroker.RabbitMQ.Subscriber;
using Peddle.MessageBroker.Serializer;
using Peddle.MessageBroker.Subscriber;
using Application.Common.Behaviours;
using Application.Interfaces.ExternalServices;
using Application.Interfaces.MessageBroker;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using Infrastructure.ExternalServices;
using Infrastructure.MessageBroker;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Peddle.Foundation.CacheManager.Core;
using Application.Mappings;
using RabbitMQ.Client;
using System.Collections.Generic;
using Peddle.MessageBroker.RabbitMQ.Connection;
using Domain.Dtos.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Domain.Dtos.Shared;

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
        #region QueueNames

        private const string OfferOperationQueueName = "OfferOperationsServiceCommonConsumer";

        #endregion

        public static void AddWebApiServicesExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(typeof(ISerializer<>), typeof(XmlSerializer<>));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());

            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.Load("Application"));
            services.AddMediatR(AppDomain.CurrentDomain.Load("Application"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.RegisterMessageBrokerConnection(configuration);
            services.RegisterMessageBrokerPublishers();
            services.RegisterMessageBrokerConsumers();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient(typeof(IInstantOfferRepository), typeof(OfferRepositoryAsync));
            services.RegisterInfraStructure(configuration);

            services.RegisterCaching(configuration);
            services.RegisterTokenBasedAuthorization(configuration);
        }


        private static void RegisterTokenBasedAuthorization(this IServiceCollection services,IConfiguration configuration)
        {

            services.Configure<JWTTokenConfiguration>(configuration.GetSection("JWTTokenConfiguration"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTTokenConfiguration:Issuer"],
                        ValidAudience = configuration["JWTTokenConfiguration:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTTokenConfiguration:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ErrorResponse<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new ErrorResponse<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });

            services.AddSingleton<IJWTTokenManger, JWTTokenManager>();
        }
        private static void RegisterInfraStructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOfferOperationService, OfferOperationExternalService>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        private static void RegisterMessageBrokerConnection(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MessageBrokerConnectionConfiguration>(
                configuration.GetSection("MessageBrokerConfiguration"));

            services.AddTransient<IConnectionFactory>(
               factory => new ConnectionFactory
               {
                   UserName = factory.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.RabbitMQUserName,
                   Password = factory.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.RabbitMQPassword,
                   HostName = factory.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.RabbitMQHostName,
                   Port = factory.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.RabbitMQPortNumber,
                   DispatchConsumersAsync = true,
                   NetworkRecoveryInterval = TimeSpan.FromSeconds(10), // default is 5 seconds
                   AutomaticRecoveryEnabled = true, // default is true
                   TopologyRecoveryEnabled = true, // default is true
                   HandshakeContinuationTimeout = TimeSpan.FromSeconds(10), // 10 seconds by default
                   ContinuationTimeout = TimeSpan.FromSeconds(20), // 20 seconds by default
                   RequestedConnectionTimeout = 30 * 1000, // default value is 30 * 1000 milliseconds
                   RequestedHeartbeat = 60, // 60 seconds by default.
                   ClientProperties = new Dictionary<string, object>
                       { { "connection-name", "seller-instantoffer-service-subscriber" } }
               });


            services.AddTransient<IRabbitMqConnection>(
                factory => new RabbitMqConnection(
                    factory.GetRequiredService<IConnectionFactory>(),
                    factory.GetRequiredService<ILogger<RabbitMqConnection>>(), 5));


        }

        private static void RegisterMessageBrokerPublishers(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBrokerPublisher>(service =>
            {
                MessageBrokerPublisher messageBrokerPublisher = new MessageBrokerPublisher(
                    service.GetRequiredService<IRabbitMqConnection>(),
                    service.GetRequiredService<ILoggerFactory>(),
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>()
                );
                return messageBrokerPublisher;
            });
        }

        private static void RegisterMessageBrokerConsumers(this IServiceCollection services)
        {

            services.AddTransient<IMessageBrokerSubscriber<RabbitMQMessage>>(service =>
                new ExchangeSubscriber<RabbitMQMessage>(
                     service.GetRequiredService<IRabbitMqConnection>(),
                    OfferOperationQueueName,
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.ExchangeName,
                    service.GetRequiredService<ILogger<ExchangeSubscriber<RabbitMQMessage>>>(),
                    service.GetRequiredService<ISerializer<RabbitMQMessage>>()
                ));

            services.AddSingleton<IMessageBrokerEventConsumer, InstantOfferMessageBrokerConsumer>();
        }


        private static void RegisterCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheServiceConfiguration>(
               configuration.GetSection("CacheServiceConfiguration"));
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
        }
    }
}