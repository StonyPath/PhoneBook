using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Application.Common.Behaviors;
using System.Reflection;

namespace PhoneBook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
