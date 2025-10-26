using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace VDA5050.NET.Internal.Messages;

public static class MessagesConverter
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        // Match camelCase JSON to PascalCase C# properties
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },

        // Allow missing members (optional, for backward compatibility)
        MissingMemberHandling = MissingMemberHandling.Ignore,

        // Ignore null values (optional)
        NullValueHandling = NullValueHandling.Ignore,

        // Add enum handling
        Converters =
        {
            new StringEnumConverter()
        }
    };
    
    public static string ToJson(this Header obj) => JsonConvert.SerializeObject(obj, Settings);

    public static T? FromJson<T>(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
        catch (JsonException)
        {
            // JSON malformed or incompatible with target type
            return default;
        }
        catch (Exception)
        {
            // Any other unexpected issue
            return default;
        }
    }
}