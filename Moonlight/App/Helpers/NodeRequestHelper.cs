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

    public Task<ServerNode> UnpackNode(Controller controller)
    {
        var requestHeaders = controller.Request.Headers;

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