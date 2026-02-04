using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using AXHelper.Extensions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UltimateForum.Razor;

public class AppConfiguration : IAppConfiguration, IAppConfigurationProperties
{
  
    public bool AllowUserCreateBoard
    {
        get => _configuration[nameof(AllowUserCreateBoard)].ParseBool();
        set => SetValue(nameof(AllowUserCreateBoard), value.ToString());
    }

    public bool AllowAnonymousPost
    {
        get => _configuration[nameof(AllowAnonymousPost)].ParseBool();
        set => SetValue(nameof(AllowAnonymousPost), value.ToString());
    }

    public bool AllowAnonymousTopic
    {
        get => _configuration[nameof(AllowAnonymousTopic)].ParseBool();
        set => SetValue(nameof(AllowAnonymousTopic), value.ToString());
    }

    public bool AllowAnonymousBoard
    {
        get => _configuration[nameof(AllowAnonymousBoard)].ParseBool();
        set => SetValue(nameof(AllowAnonymousBoard), value.ToString());
    }

    public bool ShowCreateBoardWhenRequirementNotMet
    {
        get => _configuration[nameof(ShowCreateBoardWhenRequirementNotMet)].ParseBool();
        set => SetValue(nameof(ShowCreateBoardWhenRequirementNotMet), value.ToString());
    }

    public bool ShowCreateTopicWhenRequirementNotMet
    {
        get => _configuration[nameof(ShowCreateTopicWhenRequirementNotMet)].ParseBool();
        set => SetValue(nameof(ShowCreateTopicWhenRequirementNotMet), value.ToString());
    }

    public bool ShowCreatePostWhenRequirementNotMet
    {
        get => _configuration[nameof(ShowCreatePostWhenRequirementNotMet)].ParseBool();
        set => SetValue(nameof(ShowCreatePostWhenRequirementNotMet), value.ToString());
    }

    public IEnumerable<KeyValuePair<string, (string LocalizedString, bool value)>> AsKvEnumerable()
    {
        var e = typeof(IAppConfigurationProperties)
            .GetProperties()
            .Select(propertyInfo => new KeyValuePair<string, (string LocalizedString, bool value)>(propertyInfo.Name,
                (propertyInfo.Name switch
                {
                    nameof(AllowUserCreateBoard) => "允许用户创建板块",
                    nameof(AllowAnonymousPost) => "允许匿名回复",
                    nameof(AllowAnonymousTopic) => "允许匿名话题",
                    nameof(AllowAnonymousBoard) => "允许匿名版块",
                    nameof(ShowCreateBoardWhenRequirementNotMet) => "条件不允许仍旧显示“创建板块”按钮",
                    nameof(ShowCreateTopicWhenRequirementNotMet) => "条件不允许仍旧显示“创建话题”按钮",
                    nameof(ShowCreatePostWhenRequirementNotMet) => "条件不允许仍旧显示“创建回复”按钮",
                    _ => throw new ArgumentOutOfRangeException()
                }, (bool)propertyInfo.GetValue(this)!)));
           

        return e;
    }
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
    public void SetValue(string key, string value)
    {
        var obj = JsonSerializer.Deserialize<JsonNode>( File.ReadAllText(_configuration["AppConfigDir"] ??  "appsettings.json"), new JsonSerializerOptions()
        {
            AllowTrailingCommas = true
        });
        if (obj is null)
        {
            throw new FileNotFoundException("appsettings.json");
        }
        obj[key] = value; 
         File.WriteAllText(_configuration["AppConfigDir"] ??  "appsettings.json", JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy =JsonNamingPolicy.CamelCase, WriteIndented = true
        }));

        while (_configuration[key] != value)
        {
             Task.Delay(1).GetAwaiter().GetResult();
        }
    }
    public string? this [string key] => _configuration[key];

    public IEnumerable<KeyValuePair<string,string?>> AsEnumerable()
    {
        return _configuration.AsEnumerable();
    }
}

public interface IAppConfiguration
{
    Task SetValueAsync(string key, string value);
}

public interface IAppConfigurationProperties
{
    bool AllowUserCreateBoard
    {
        get;
        set;
    }

    bool AllowAnonymousPost
    {
        get;
        set;
    }

    bool AllowAnonymousTopic
    {
        get;
        set;
    }

    bool AllowAnonymousBoard
    {
        get;
        set;
    }

    bool ShowCreateBoardWhenRequirementNotMet
    {
        get;
        set;
    }

    bool ShowCreateTopicWhenRequirementNotMet
    {
        get;
        set;
    }

    bool ShowCreatePostWhenRequirementNotMet
    {
        get ;
        set ;
    }
}