using Microsoft.JSInterop;

namespace Moonlight.App.Services.Interop;

public class DropzoneService
{
    private readonly IJSRuntime JsRuntime;

    public DropzoneService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Create(string id, string initialUrl)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.dropzone.create", id, initialUrl);
    }

    public async Task UpdateUrl(string id, string url)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.dropzone.updateUrl", id, url);
    }
}