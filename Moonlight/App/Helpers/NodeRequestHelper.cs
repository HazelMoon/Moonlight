using Microsoft.AspNetCore.Mvc;
using Moonlight.App.Database.Entities.Servers;
using Moonlight.App.Exceptions.Api;
using Moonlight.App.Repositories;

namespace Moonlight.App.Helpers;

public class NodeRequestHelper
{
    private readonly Repository<ServerNode> NodeRepository;

    public NodeRequestHelper(Repository<ServerNode> nodeRepository)
    {
        NodeRepository = nodeRepository;
    }

    public async Task<ServerNode> UnpackNode(Controller controller) => await UnpackNode(controller.Request);

    public async Task<bool> Verify(HttpRequest request)
    {
        try
        {
            await UnpackNode(request);
            return true;
        }
        catch (BadRequestException)
        {
            return false;
        }
        catch (ForbiddenException)
        {
            return false;
        }
    }
    
    public Task<ServerNode> UnpackNode(HttpRequest request)
    {
        var requestHeaders = request.Headers;

        if (!requestHeaders.ContainsKey("Authorization"))
            throw new BadRequestException("Authorization header is missing");

        var key = requestHeaders["Authorization"].FirstOrDefault();

        if (key == null)
            throw new BadRequestException("Unable to find key in Authorization header");

        var node = NodeRepository
            .Get()
            .FirstOrDefault(x => x.Token == key);

        if (node == null)
            throw new ForbiddenException("Invalid key provided");

        return Task.FromResult(node);
    }
}