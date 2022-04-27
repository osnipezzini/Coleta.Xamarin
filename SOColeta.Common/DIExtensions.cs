using Microsoft.Extensions.DependencyInjection;

using SOColeta.Common.Services;

namespace SOColeta.Common;

public static class DIExtensions
{
    public static void ConfigureCommon(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUserConnectionManager, UserConnectionManager>();
    }
}
