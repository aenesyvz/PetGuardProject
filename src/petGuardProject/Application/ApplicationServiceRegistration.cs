using Application.Services.AuthenticatorService;
using Application.Services.AuthService;
using Application.Services.BackerService;
using Application.Services.CityService;
using Application.Services.DistrictService;
using Application.Services.OperationClaimsService;
using Application.Services.PetOwnersService;
using Application.Services.PetService;
using Application.Services.UserOperationClaimsService;
using Application.Services.UsersService;
using Application.Services.UtilitiesService;
using Core.Application.Dtos;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using Core.Application.Pipelines.Validation;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.ElasticSearch;
using Core.ElasticSearch.Models;
using Core.Mailing;
using Core.Mailing.MailKitImplementations;
using Core.Security;
using Core.Security.JWT;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NArchitecture.Core.Localization.Resource.Yaml.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
         this IServiceCollection services,
         MailSettings mailSettings,
         FileLogConfiguration fileLogConfiguration,
         ElasticSearchConfig elasticSearchConfig,
         TokenOptions tokenOptions
     )
    {

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
        });

        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMailService, MailKitMailService>(_ => new MailKitMailService(mailSettings));
        services.AddSingleton<ILogger, SerilogFileLogger>(_ => new SerilogFileLogger(fileLogConfiguration));
        services.AddSingleton<IElasticSearch, ElasticSearchManager>(_ => new ElasticSearchManager(elasticSearchConfig));

        services.AddScoped<IAuthService, AuthManager>();
        services.AddScoped<IAuthenticatorService, AuthenticatorManager>();
        services.AddScoped<IOperationClaimService, OperationClaimManager>();
        services.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ICityService, CityManager>();
        services.AddScoped<IDistrictService, DistrictManager>();
        services.AddScoped<IBackerService, BackerManager>();
        services.AddScoped<IPetService, PetManager>();
        services.AddScoped<IPetOwnerService,PetOwnerManager>();


        services.AddYamlResourceLocalization();

        services.AddSecurityServices<Guid, int, Guid>(tokenOptions);

        return services;
    }


    public static IServiceCollection AddSubClassesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
    )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (Type? item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
        return services;
    }
}
