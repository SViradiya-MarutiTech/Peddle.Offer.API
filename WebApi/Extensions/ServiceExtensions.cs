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

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
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

        private static IServiceCollection RegisterMessageBrokerConnection(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MessageBrokerConnectionConfiguration>(
                configuration.GetSection("MessageBrokerConfiguration"));
            services.AddTransient<IMessageBrokerConnection>(service => new MessageBrokerConnection(
                "publisher-connection", service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>(),
                service.GetRequiredService<ILogger<MessageBrokerConnection>>(),
                service.GetRequiredService<ILoggerFactory>()
            ));
            return services;
        }

        private static IServiceCollection RegisterMessageBrokerPublishers(this IServiceCollection services)
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
            return services;
        }

        private static IServiceCollection RegisterMessageBrokerConsumers(this IServiceCollection services)
        {
            // Register Message Broker Consumers
            services.AddTransient(typeof(IMessageBrokerEventConsumer), typeof(MessageBrokerEventConsumer));
            services.AddTransient<IMessageBrokerSubscriber<RabbitMQMessage>>(service =>
                new ExchangeSubscriber<RabbitMQMessage>(
                    service.GetRequiredService<IMessageBrokerConnection>().RabbitMQConnection,
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.QueueName,
                    service.GetRequiredService<IOptions<MessageBrokerConnectionConfiguration>>().Value.ExchangeName,
                    service.GetRequiredService<ILogger<ExchangeSubscriber<RabbitMQMessage>>>(),
                    service.GetRequiredService<ISerializer<RabbitMQMessage>>()
                ));

            return services;
            //Register Event Handlers
        }
    }
}