using TVShowChecker.Core.Interfaces;
using TVShowChecker.Infrastructure;
using TVShowChecker.Infrastructure.DataAccess;
using TVShowChecker.Infrastructure.Services;
using Unity;

namespace TVShowChecker;

internal static class Bootstrapper
{
    public static void AddRegistrations(this IUnityContainer container)
    {
        container.RegisterType<IConfigHandler, ConfigHandler>();
        container.RegisterType<ITVShowService, TVMazeService>();
        container.RegisterType<ILogger, Logger>();
    }
}
