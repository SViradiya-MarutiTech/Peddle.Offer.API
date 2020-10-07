using System;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Api.MessageBrokerConsumers;
using Api.EventHandlers;

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
        #region QueueNames
        private const string OFFER_OPERATIONS_SERVICE_COMMON_CONSUMER = "OfferOperationsServiceCommonConsumer";

        #endregion
        public static void AddWebApiServicesExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(typeof(ISerializer<>), typeof(XmlSerializer<>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
            services.AddTransient(typeof(IOfferRepository), typeof(OfferRepositoryAsync));

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
                    OFFER_OPERATIONS_SERVICE_COMMON_CONSUMER,
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.ExchangeName,
                    service.GetRequiredService<ILogger<ExchangeSubscriber<RabbitMQMessage>>>(),
                    service.GetRequiredService<ISerializer<RabbitMQMessage>>()
                ));

            //Add new Consumer class same as below.
            services.AddTransient<IMessageBrokerEventConsumer>(serviceProvider => new OfferOperationsServiceCommonConsumer(
                    serviceProvider.GetRequiredService<IMediator>(),
                    serviceProvider.GetRequiredService<IMessageBrokerSubscriber<RabbitMQMessage>>(),
                    serviceProvider.GetRequiredService<ILogger<OfferOperationsServiceCommonConsumer>>(),
                    serviceProvider.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value
                ));
            //One consumer event handler to start 
            services.AddTransient<IMessageBrokerEventHandler>(service => new MessageBrokerEventHandler(
                new IMessageBrokerEventConsumer[] {
                    service.GetRequiredService<IMessageBrokerEventConsumer>()
                }));


        }
    }
}