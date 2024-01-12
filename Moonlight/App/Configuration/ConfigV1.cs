﻿using System.ComponentModel;
using Moonlight.App.Helpers;
using Newtonsoft.Json;

namespace Moonlight.App.Configuration;

public class ConfigV1
{
    [JsonProperty("AppUrl")]
    [Description("The url with which moonlight is accessible from the internet. It must not end with a /")]
    public string AppUrl { get; set; } = "http://your-moonlight-instance-without-slash.owo";
    
    [JsonProperty("Security")] public SecurityData Security { get; set; } = new();
    [JsonProperty("Database")] public DatabaseData Database { get; set; } = new();
    [JsonProperty("MailServer")] public MailServerData MailServer { get; set; } = new();
    [JsonProperty("Store")] public StoreData Store { get; set; } = new();
    [JsonProperty("WebServer")] public WebServerData WebServer { get; set; } = new();
    [JsonProperty("FileManager")] public FileManagerData FileManager { get; set; } = new();
    
    public class FileManagerData
    {
        [JsonProperty("MaxFileOpenSize")]
        [Description(
            "This specifies the maximum file size a user will be able to open in the file editor in kilobytes")]
        public int MaxFileOpenSize { get; set; } = 1024 * 5; // 5 MB

        [JsonProperty("OperationTimeout")]
        [Description("This specifies the general timeout for file manager operations. This can but has not to be used by file accesses")]
        public int OperationTimeout { get; set; } = 5;
    }
    
    public class WebServerData
    {
        [JsonProperty("HttpUploadLimit")]
        [Description("This sets the kestrel upload limit in megabytes. Changing this will need an restart")]
        public int HttpUploadLimit { get; set; } = 100 * 1024;
    }

    public class StoreData
    {
        [JsonProperty("Currency")]
        [Description("A string value representing the currency which will be shown to a user")]
        public string Currency { get; set; } = "€";
    }
    
    public class SecurityData
    {
        [JsonProperty("Token")]
        [Description("The security token helio will use to encrypt various things like tokens")]
        public string Token { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

        [JsonProperty("EnableEmailVerify")]
        [Description("This will users force to verify their email address if they havent already")]
        public bool EnableEmailVerify { get; set; } = false;

        [JsonProperty("EnableReverseProxyMode")]
        [Description("Enable this option if you are using a reverse proxy to access moonlight. This will configure some parts of moonlight to act correctly like the ip detection")]
        public bool EnableReverseProxyMode { get; set; } = false;
    }

    public class DatabaseData
    {
        [JsonProperty("UseSqlite")]
        public bool UseSqlite { get; set; } = false;
        
        [JsonProperty("SqlitePath")]
        public string SqlitePath { get; set; } = PathBuilder.File("storage", "data.sqlite");
        
        [JsonProperty("Host")]
        public string Host { get; set; } = "your.db.host";
        
        [JsonProperty("Port")]
        public int Port { get; set; } = 3306;
        
        [JsonProperty("Username")]
        public string Username { get; set; } = "moonlight_user";
        
        [JsonProperty("Password")]
        public string Password { get; set; } = "s3cr3t";
        
        [JsonProperty("Database")]
        public string Database { get; set; } = "moonlight_db";
    }
    
    public class MailServerData
    {
        [JsonProperty("Host")] public string Host { get; set; } = "your.email.host";

        [JsonProperty("Port")] public int Port { get; set; } = 465;

        [JsonProperty("Email")] public string Email { get; set; } = "noreply@your.email.host";

        [JsonProperty("Password")] public string Password { get; set; } = "s3cr3t";

        [JsonProperty("UseSsl")] public bool UseSsl { get; set; } = true;

        [JsonProperty("SenderName")]
        [Description("This will be shown as the system emails sender name in apps like gmail")]
        public string SenderName { get; set; } = "Moonlight System";
    }
}