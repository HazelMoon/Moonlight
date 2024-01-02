﻿using Microsoft.JSInterop;

namespace Moonlight.App.Services.Interop;

public class ToastService
{
    private readonly IJSRuntime JsRuntime;

    public ToastService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Success(string title, string message, int timeout = 5000)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.toasts.success", title, message, timeout);
    }
    
    public async Task Info(string title, string message, int timeout = 5000)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.toasts.info", title, message, timeout);
    }
    
    public async Task Danger(string title, string message, int timeout = 5000)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.toasts.danger", title, message, timeout);
    }
    
    public async Task Warning(string title, string message, int timeout = 5000)
    {
        await JsRuntime.InvokeVoidAsync("moonlight.toasts.warning", title, message, timeout);
    }
    
    // Overloads
    
    public async Task Success(string message, int timeout = 5000)
    {
        await Success("", message, timeout);
    }
    
    public async Task Info(string message, int timeout = 5000)
    {
        await Info("", message, timeout);
    }
    
    public async Task Danger(string message, int timeout = 5000)
    {
        await Danger("", message, timeout);
    }
    
    public async Task Warning(string message, int timeout = 5000)
    {
        await Warning("", message, timeout);
    }
    
    // Progress (exceptions are ignored because we dont want to crash a process just because the ui is not updating
    public async Task CreateProgress(string id, string text)
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("moonlight.toasts.progress.create", id, text);
        }
        catch (Exception) { /* ignored */ }
    }

    public async Task ModifyProgress(string id, string text)
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("moonlight.toasts.progress.modify", id, text);
        }
        catch (Exception) { /* ignored */ }
    }

    public async Task RemoveProgress(string id)
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("moonlight.toasts.progress.remove", id);
        }
        catch (Exception) { /* ignored */ }
    }
}