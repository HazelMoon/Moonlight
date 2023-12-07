using Moonlight.App.Actions.Servers.Layouts;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Abstractions.Services;

namespace Moonlight.App.Actions.Servers;

public class ServerServiceDefinition : ServiceDefinition
{
    public override ServiceActions Actions => new ServerActions();
    public override Type ConfigType => typeof(ServerConfig);
    public override Task BuildUserView(ServiceViewContext context)
    {
        context.Layout = ComponentHelper.FromType<ServerUserLayout>();
        
        return Task.CompletedTask;
    }

    public override Task BuildAdminView(ServiceViewContext context)
    {
        context.Layout = ComponentHelper.FromType<ServerAdminLayout>();
        
        return Task.CompletedTask;
    }
}