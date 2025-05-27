using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SmartWebScraper.Domain.Contracts;
using SmartWebScraper.Application.Features.Commands.SaveSearchResult;

namespace SmartWebScraper.Application;
public static class ApplicationServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<SaveSearchResultCommand>());// Register MediatR
        services.AddValidatorsFromAssemblyContaining<SaveSearchResultCommandValidator>(); // Register FluentValidation validators
        services.AddHttpClient(); 
        return services;
    }
}
