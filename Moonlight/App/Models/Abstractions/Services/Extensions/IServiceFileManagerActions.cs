using Moonlight.App.Database.Entities;
using Moonlight.App.Database.Entities.Store;

namespace Moonlight.App.Models.Abstractions.Services.Extensions;

public interface IServiceFileManagerActions
{
    public Task<bool> ProcessFileUpload(IServiceProvider provider, User user, Service service, string fileName, Stream stream);
}