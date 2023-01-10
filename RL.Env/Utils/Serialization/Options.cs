using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Env.Utils.Serialization;

public static class Options
{
    public readonly static JsonSerializerOptions SystemTextJsonSerializerOptions = new()
    {
        IncludeFields = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    public readonly static JsonSerializerSettings NewtonsoftSerializerOptions = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Converters = newtonsoftJsonConverters ??= new Newtonsoft.Json.JsonConverter[] { new DtypeConverter(), new NdarrayConverter(), new RandomConverter() }
    };

    private static Newtonsoft.Json.JsonConverter[] newtonsoftJsonConverters = null!;
}
