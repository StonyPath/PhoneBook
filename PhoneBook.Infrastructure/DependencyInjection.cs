using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Domain.Repositories;
using PhoneBook.Infrastructure.Persistence;

namespace PhoneBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IPhoneBookRepository, InMemoryPhoneBookRepository>();
        return services;
    }
}
