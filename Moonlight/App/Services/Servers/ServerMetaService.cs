using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Enums;
using Moonlight.App.Models.Servers;

namespace Moonlight.App.Services.Servers;

public class ServerMetaService
{
    private readonly Dictionary<int, ServerMeta> StatusCache = new();

    public async Task<ServerMeta> Get(Server server) => await Get(server.Id);

    public async Task<ServerMeta> Get(int id)
    {
        ServerMeta? meta = null;
        
        lock (StatusCache) // Load existing status if exists
        {
            if (StatusCache.ContainsKey(id))
                meta = StatusCache[id];
        }

        if (meta == null)
        {
            meta = new()
            {
                State = ServerState.Offline,
                LastChangeTimestamp = DateTime.UtcNow
            };

            await Set(id, meta);
        }

        return meta;
    }

    public Task Set(int id, ServerMeta meta)
    {
        Logger.Debug($"Added {id} to meta cache");
        
        lock (StatusCache)
            StatusCache[id] = meta;
        
        return Task.CompletedTask;
    }
}