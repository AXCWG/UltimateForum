using System.Text.Json;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UltimateForum.Razor;

public class AppConfiguration : IAppConfiguration
{
    private readonly IConfiguration _configuration;
    public AppConfiguration(IConfiguration config)
    {
        _configuration = config; 
    }

    public async Task SetValueAsync(string key, string value)
    {
        var obj = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync(_configuration["AppConfigDir"] ??  "appsettings.json"), new JsonSerializerOptions()
        {
            AllowTrailingCommas = true
        });
        if (obj is null)
        {
            throw new FileNotFoundException("appsettings.json");
        }
        obj[key] = value; 
        await File.WriteAllTextAsync(_configuration["AppConfigDir"] ??  "appsettings.json", JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy =JsonNamingPolicy.CamelCase, WriteIndented = true
        }));

        while (_configuration[key] != value)
        {
            await Task.Delay(1);
        }
    }
    public string? this [string key] => _configuration[key];
    
}

public interface IAppConfiguration
{
    Task SetValueAsync(string key, string value);
}