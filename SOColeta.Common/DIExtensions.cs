using Microsoft.Extensions.DependencyInjection;

using SOColeta.Common.Services;

namespace SOColeta.Common;

public static class DIExtensions
{
    public static IServiceCollection ConfigureCommon(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUserConnectionManager, UserConnectionManager>();
        return serviceCollection;
    }
}
