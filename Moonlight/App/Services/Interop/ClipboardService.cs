using Microsoft.JSInterop;

namespace Moonlight.App.Services.Interop;

public class ClipboardService
{
    private readonly IJSRuntime JsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Copy(string content)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.clipboard.copy", content);
    }
}