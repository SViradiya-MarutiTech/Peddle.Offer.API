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

            services.AddTransient<IMessageBrokerConnection>(service => new MessageBrokerConnection(
                "publisher-connection", service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>(),
                service.GetRequiredService<ILogger<MessageBrokerConnection>>(),
                service.GetRequiredService<ILoggerFactory>()
            ));
        }

        private static void RegisterMessageBrokerPublishers(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBrokerPublisher>(provider =>
            {
                MessageBrokerPublisher messageBrokerPublisher = new MessageBrokerPublisher(
                    provider.GetRequiredService<IMessageBrokerConnection>(),
                    provider.GetRequiredService<ILoggerFactory>(),
                    provider.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>()
                );
                return messageBrokerPublisher;
            });
        }

        private static void RegisterMessageBrokerConsumers(this IServiceCollection services)
        {
            // Register Message Broker Consumers

            services.AddTransient<IMessageBrokerSubscriber<RabbitMQMessage>>(service =>
                new ExchangeSubscriber<RabbitMQMessage>(
                    service.GetRequiredService<IMessageBrokerConnection>().RabbitMQConnection,
                    OfferOperationQueueName,
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.ExchangeName,
                    service.GetRequiredService<ILogger<ExchangeSubscriber<RabbitMQMessage>>>(),
                    service.GetRequiredService<ISerializer<RabbitMQMessage>>()
                ));

            //Add new Consumer class same as below.
            services.TryAddEnumerable(
                new[]
                {
                    ServiceDescriptor.Transient<IMessageBrokerEventConsumer, OfferOperationsServiceCommonConsumer>(),
                    //TODO: Add Other Consumer classes as above
                }
            );
        }


        private static void RegisterCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheServiceConfiguration>(
               configuration.GetSection("CacheServiceConfiguration"));
            services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
        }
    }
}