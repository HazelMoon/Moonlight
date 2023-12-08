using System.Net.WebSockets;
using System.Reflection;
using Moonlight.App.Helpers;
using Moonlight.App.Packets.Servers.Server;
using WsPackets.Server;
using WsPackets.Shared;

namespace Moonlight.App.Services.Servers.Nodes;

public class NodeNetworkingService
{
    private readonly WspServer WspServer;
    private readonly IServiceProvider ServiceProvider;

    public NodeNetworkingService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        var assemblyPrefix = "Moonlight.App.Packets.Servers";
        WspServer = new(new AssemblyTypeResolver(Assembly.GetExecutingAssembly(), assemblyPrefix));
        
        WspServer.OnPacket += HandlePacket;
    }

    public async Task HandleIncomingWebsocket(WebSocket webSocket) // This method will accept and wait websocket connections
    {
        var connection = await WspServer.AddConnection(webSocket);
        connection.OnLogError = s => Logger.Warn(s);
        await connection.WaitForClose();
    }

    private async void HandlePacket(object? _, object data)
    {
        Logger.Debug($"Incoming packet: {data}");
        
        if (data is ServerStateUpdate serverStateUpdate)
        {
            Logger.Info($"Server Status Update received: {serverStateUpdate.Id} is now {serverStateUpdate.State}");
        }
    }
}