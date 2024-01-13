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
        context.Layout = typeof(ServerUserLayout);

        context.AddPage<UserPages.Overview>("Overview", "/", "bx bx-sm bxs-dashboard");
        context.AddPage<UserPages.Console>("Console", "/console", "bx bx-sm bxs-terminal");
        context.AddPage<UserPages.Files>("Files", "/files", "bx bx-sm bx-folder");
        context.AddPage<UserPages.Reset>("Reset", "/reset", "bx bx-sm bx-revision");
        
        return Task.CompletedTask;
    }

    public override Task BuildAdminView(ServiceViewContext context)
    {
        context.Layout = typeof(ServerAdminLayout);
        
        return Task.CompletedTask;
    }
}