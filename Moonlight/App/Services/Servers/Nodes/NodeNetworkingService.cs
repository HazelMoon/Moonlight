using System.Net.WebSockets;
using System.Reflection;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Extensions;
using Moonlight.App.Helpers;
using Moonlight.App.Packets.Servers.Server;
using WsPackets.Server;
using WsPackets.Shared;

namespace Moonlight.App.Services.Servers.Nodes;

public class NodeNetworkingService
{
    private readonly string AssemblyPrefix = "Moonlight.App.Packets.Servers";
    private readonly Dictionary<int, WspServer> NodeWsServers = new();
    private readonly IServiceProvider ServiceProvider;

    public NodeNetworkingService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public async Task HandleIncomingWebsocket(WebSocket webSocket, ServerNode node) // This method will accept and wait websocket connections
    {
        WspServer? wspServer = null;

        lock (NodeWsServers)
        {
            if (NodeWsServers.ContainsKey(node.Id))
                wspServer = NodeWsServers[node.Id];
        }

        if (wspServer == null)
            wspServer = await AddWsServer(node);
        
        var connection = await wspServer.AddConnection(webSocket);
        connection.OnLogError = message => Logger.Warn($"WspServer ({node.Name}): {message}");
        await connection.WaitForClose();
    }

    private async Task HandlePacket(ServerNode node, object data)
    {
        Logger.Debug($"Received {data.GetType().Name} from {node.Name}");
        
        try
        {
            if (data is ServerStateUpdate serverStateUpdate)
            {
                var serverService = ServiceProvider.GetRequiredService<ServerService>();

                // Modify current status and save the changes
                var meta = await serverService.Meta.Get(serverStateUpdate.Id);
                meta.State = serverStateUpdate.State;
                meta.LastChangeTimestamp = DateTime.UtcNow;

                // Notify that changes have been made
                await meta.OnStateChanged.Invoke();
            }

            if (data is ServerConsoleMessage serverConsoleMessage)
            {
                Logger.Debug(serverConsoleMessage.Message);
                
                var serverService = ServiceProvider.GetRequiredService<ServerService>();

                // Load meta
                var meta = await serverService.Meta.Get(serverConsoleMessage.Id);

                lock (meta.ConsoleMessages)
                    meta.ConsoleMessages.Add(serverConsoleMessage.Message);

                await meta.OnConsoleMessage.Invoke(serverConsoleMessage.Message);
            }
        }
        catch (Exception e)
        {
            Logger.Warn($"An unknown error occured while handling a '{data.GetType()}' packet from node '{node.Name}'");
            Logger.Warn(e);
        }
    }

    private Task<WspServer> AddWsServer(ServerNode node)
    {
        // Create ws server
        var wsServer = new WspServer(
            new AssemblyTypeResolver(
                Assembly.GetExecutingAssembly(), AssemblyPrefix));
        
        // Build packet handler with node context
        wsServer.OnPacket += async (_, packet) => await HandlePacket(node, packet);
        
        // Save ws server with node id
        lock (NodeWsServers)
            NodeWsServers.Add(node.Id, wsServer);
        
        return Task.FromResult(wsServer);
    }

    public async Task SendWsPacket(int node, object data)
    {
        WspServer? wspServer = null;

        lock (NodeWsServers)
        {
            if (NodeWsServers.ContainsKey(node))
                wspServer = NodeWsServers[node];
        }

        if (wspServer == null)
            throw new IOException("Unable to send packet to a node without ws connection");

        await wspServer.Send(data);
    }

    public async Task SendWsPacket(ServerNode node, object data) => await SendWsPacket(node.Id, data);
}