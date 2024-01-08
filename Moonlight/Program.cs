using BlazorTable;
using Moonlight.App.Actions.Dummy;
using Moonlight.App.Actions.Servers;
using Moonlight.App.Database;
using Moonlight.App.Database.Enums;
using Moonlight.App.Extensions;
using Moonlight.App.Helpers;
using Moonlight.App.Helpers.LogMigrator;
using Moonlight.App.Repositories;
using Moonlight.App.Services;
using Moonlight.App.Services.Background;
using Moonlight.App.Services.Community;
using Moonlight.App.Services.Interop;
using Moonlight.App.Services.Servers;
using Moonlight.App.Services.Servers.Nodes;
using Moonlight.App.Services.ServiceManage;
using Moonlight.App.Services.Store;
using Moonlight.App.Services.Ticketing;
using Moonlight.App.Services.Sys;
using Moonlight.App.Services.Users;
using Moonlight.App.Services.Utils;
using Serilog;

var configService = new ConfigService();
var moonlightService = new MoonlightService(configService);

Directory.CreateDirectory(PathBuilder.Dir("storage"));
Directory.CreateDirectory(PathBuilder.Dir("storage", "logs"));

var logConfig = new LoggerConfiguration();

logConfig = logConfig.Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate:
        "{Timestamp:HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}");

Log.Logger = logConfig.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    var uploadLimit = ByteSizeValue.FromMegaBytes(configService.Get().WebServer.HttpUploadLimit).Bytes;
    options.Limits.MaxRequestBodySize = uploadLimit;
});

// Init plugin system
var pluginService = new PluginService();
builder.Services.AddSingleton(pluginService);

await pluginService.Load(builder);
await pluginService.RunPreInit();

builder.Services.AddDbContext<DataContext>();

// Helpers
builder.Services.AddScoped<NodeRequestHelper>();
builder.Services.AddScoped<SessionThreadLock>();

// Repositories
builder.Services.AddScoped(typeof(Repository<>));

// Services / Utils
builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<ConnectionService>();
builder.Services.AddSingleton<SharedFileAccessService>();

// Services / Interop
builder.Services.AddScoped<CookieService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<ModalService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<FileDownloadService>();
builder.Services.AddScoped<ClipboardService>();
builder.Services.AddScoped<DropzoneService>();

// Services / Store
builder.Services.AddScoped<StoreService>();
builder.Services.AddScoped<StoreAdminService>();
builder.Services.AddScoped<StoreOrderService>();
builder.Services.AddScoped<StoreGiftService>();
builder.Services.AddSingleton<StorePaymentService>();
builder.Services.AddScoped<TransactionService>();

// Services / Community
builder.Services.AddScoped<PostService>();

// Services / Users
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserAuthService>();
builder.Services.AddScoped<UserDetailsService>();
builder.Services.AddScoped<UserDeleteService>();

// Services / Background
builder.Services.AddSingleton<AutoMailSendService>();

// Services / ServiceManage
builder.Services.AddScoped<ServiceService>();
builder.Services.AddSingleton<ServiceAdminService>();
builder.Services.AddSingleton<ServiceDefinitionService>();
builder.Services.AddSingleton<ServiceManageService>();

// Services / Ticketing
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TicketChatService>();
builder.Services.AddScoped<TicketCreateService>();

// Services / Servers / Nodes
builder.Services.AddSingleton<NodeService>();
builder.Services.AddSingleton<NodeBootService>();
builder.Services.AddSingleton<NodeNetworkingService>();

// Services / Servers
builder.Services.AddSingleton<ServerService>();
builder.Services.AddSingleton<ServerMetaService>();
builder.Services.AddSingleton<ServerPowerService>();
builder.Services.AddSingleton<ServerConsoleService>();

// Services
builder.Services.AddScoped<IdentityService>();
builder.Services.AddSingleton(configService);
builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<BucketService>();
builder.Services.AddSingleton<MailService>();
builder.Services.AddSingleton(moonlightService);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddBlazorTable();

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new LogMigrateProvider());

var config =
    new ConfigurationBuilder().AddJsonString(
        "{\"LogLevel\":{\"Default\":\"Information\",\"Microsoft.AspNetCore\":\"Warning\"}}");
builder.Logging.AddConfiguration(config.Build());

var app = builder.Build();
moonlightService.Application = app;

app.UseWebSockets();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

// Auto start background services
app.Services.GetRequiredService<AutoMailSendService>();

var serviceService = app.Services.GetRequiredService<ServiceDefinitionService>();

//serviceService.Register<DummyServiceDefinition>(ServiceType.Server);
serviceService.Register<ServerServiceDefinition>(ServiceType.Server);

await pluginService.RunPostInit(app);

Task.Run(async () =>
{
    await Task.Delay(TimeSpan.FromSeconds(3));

    var nodeService = app.Services.GetRequiredService<NodeService>();
    await nodeService.Boot.BootAll();
});

app.Run();