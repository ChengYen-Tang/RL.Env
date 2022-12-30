using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Env.Utils.Serialization;

public static class Options
{
    public readonly static JsonSerializerOptions SerializerOptions = new()
    {
        IncludeFields = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    };
}
