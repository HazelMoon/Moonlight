using Moonlight.App.Database.Entities;

namespace Moonlight.App.Models.Abstractions.Services.Extensions;

public interface IServiceFtpActions
{
    public Task<bool> AuthenticateFtpLogin(IServiceProvider provider, User user, int resourceId, HttpRequest request);
}