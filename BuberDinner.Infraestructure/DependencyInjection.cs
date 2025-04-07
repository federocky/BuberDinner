using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Infraestructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace BuberDinner.Infraestructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, jwtTokenGenerator>();
        return services;
    }
}