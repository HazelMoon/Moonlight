﻿using JWT.Algorithms;
using JWT.Builder;
using Moonlight.App.Helpers;
using Moonlight.App.Models.Enums;
using Newtonsoft.Json;

namespace Moonlight.App.Services.Utils;

public class JwtService
{
    private readonly ConfigService ConfigService;
    private readonly TimeSpan DefaultDuration = TimeSpan.FromDays(365 * 10);

    public JwtService(ConfigService configService)
    {
        ConfigService = configService;
    }

    public Task<string> Create(Action<Dictionary<string, string>> data, JwtType type, TimeSpan? validDuration = null)
    {
        var builder = new JwtBuilder()
            .WithSecret(ConfigService.Get().Security.Token)
            .IssuedAt(DateTime.UtcNow)
            .AddHeader("Type", type.ToString())
            .ExpirationTime(DateTime.UtcNow.Add(validDuration ?? DefaultDuration))
            .WithAlgorithm(new HMACSHA512Algorithm());

        var dataDic = new Dictionary<string, string>();
        data.Invoke(dataDic);

        foreach (var entry in dataDic)
            builder = builder.AddClaim(entry.Key, entry.Value);

        var jwt = builder.Encode();
        
        return Task.FromResult(jwt);
    }

    public Task<bool> Validate(string token, params JwtType[] allowedTypes)
    {
        try
        {
            var headerJson = new JwtBuilder()
                .WithSecret(ConfigService.Get().Security.Token)
                .WithAlgorithm(new HMACSHA512Algorithm())
                .MustVerifySignature()
                .DecodeHeader(token);

            if (headerJson == null)
                return Task.FromResult(false);
            
            // Jwt type validation
            if(allowedTypes.Length == 0)
                return Task.FromResult(true);
            
            var headerData = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerJson);
            
            if(headerData == null) // => Invalid header
                return Task.FromResult(false);
            
            if(!headerData.ContainsKey("Type")) // => Invalid header, Type is missing
                return Task.FromResult(false);

            foreach (var name in Enum.GetNames(typeof(JwtType)))
            {
                if(headerData["Type"] == name) // => Correct type found
                    return Task.FromResult(true);
            }
            
            // None found? Invalid type!
            return Task.FromResult(false);
        }
        catch (Exception e)
        {
            Logger.Warn(e.Message);
            return Task.FromResult(false);
        }
    }

    public Task<Dictionary<string, string>> Decode(string token)
    {
        try
        {
            var json = new JwtBuilder()
                .WithSecret(ConfigService.Get().Security.Token)
                .WithAlgorithm(new HMACSHA512Algorithm())
                .MustVerifySignature()
                .Decode(token);

            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            
            return Task.FromResult(data)!;
        }
        catch (Exception)
        {
            return Task.FromResult<Dictionary<string, string>>(null!);
        }
    }
}