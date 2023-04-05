﻿using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using Logging.Net;
using Moonlight.App.Repositories;
using Moonlight.App.Services.DiscordBot.Commands;
using Moonlight.App.Services.DiscordBot.Modules;

namespace Moonlight.App.Services.DiscordBot;

public class DiscordBotService
{
    public static bool MaintenanceMode = false;

    private readonly IServiceScopeFactory ServiceScopeFactory;
    private readonly ConfigService ConfigService;

    private IServiceScope ServiceScope;

    private readonly DiscordSocketClient Client;

    // References
    public ActivityStatusModule ActivityStatusModule { get; private set; }

public DiscordBotService(
        IServiceScopeFactory serviceScopeFactory,
        ConfigService configService)
    {
        ServiceScopeFactory = serviceScopeFactory;
        ConfigService = configService;
        Client = new();

        Task.Run(MainAsync);
    }

    private async Task MainAsync()
    {
        ServiceScope = ServiceScopeFactory.CreateScope();

        var discordConfig = ConfigService
            .GetSection("Moonlight")
            .GetSection("DiscordBot");

        if (!discordConfig.GetValue<bool>("Enable"))
            return;

        Client.Log += Log;
        Client.Ready += OnReady;

        //Commands

        //Module
        ActivityStatusModule = new(Client, ConfigService, ServiceScope);

        await ActivityStatusModule.UpdateActivityStatusList();

        await Client.LoginAsync(TokenType.Bot, discordConfig.GetValue<string>("Token"));
        await Client.StartAsync();

        await Task.Delay(-1);
    }
    private async Task OnReady()
    {
        //await Client.SetGameAsync(name: "https://endelon-hosting.de", type: ActivityType.Watching);
        await Client.SetStatusAsync(UserStatus.Idle);

        Logger.Info($"Invite link: https://discord.com/api/oauth2/authorize?client_id={Client.CurrentUser.Id}&permissions=1099511696391&scope=bot%20applications.commands");
        Logger.Info($"Login as {Client.CurrentUser.Username}#{Client.CurrentUser.DiscriminatorValue}");
        
        Task.Run(ActivityStatusModule.ActivityStatusScheduler);
    }

    private Task Log(LogMessage message)
    {
        Logger.Debug(message.Message);
        return Task.CompletedTask;
    }

    public async Task CreateCommands()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var type = this.GetType();
        var properties = type.GetProperties();
        Logger.Debug("Start Initializing Commands");
        foreach (var property in properties)
        {
            if (property.PropertyType.IsSubclassOf(typeof(BaseModule)))
            {
                try
                {
                    var instance = (BaseModule)property.GetValue(this)!;
                    await instance.RegisterCommands();
                    Logger.Debug("Registered" + instance);
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                }
                catch (Exception ex)
                {
                    Logger.Error($"Module Error {ex.Message}");
                    Logger.Error(ex.InnerException);
                }
            }
        }

        stopwatch.Stop();
        Logger.Info($"Registered all commands. Done in {stopwatch.ElapsedMilliseconds}ms");
    }

}