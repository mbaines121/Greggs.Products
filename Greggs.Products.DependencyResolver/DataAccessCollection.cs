using Greggs.Products.DataAccess;
using Greggs.Products.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.DependencyResolver;

/// <summary>
/// The purpose of the dependency resolver service collection is to decouple the solution DI configuration from the client application.
/// If a new client app is now added to the solution, then the configuration does not need to be duplicated.
/// </summary>
public static class DataAccessCollection
{
    public static IServiceCollection AddDataAccessCollection(this IServiceCollection services)
    {
        services.AddTransient<IDataAccess<Product>, ProductAccess>();
        services.AddTransient<IFinanceAccess, FinanceAccess>();

        return services;
    }
}
