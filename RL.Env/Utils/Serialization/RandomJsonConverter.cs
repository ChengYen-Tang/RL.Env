using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Env.Utils.Serialization;

public class RandomJsonConverter : JsonConverter<np.random>
{
    public override np.random? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? jsonString = reader.GetString();
        if (jsonString == null)
            return null;
        np.random_serializable? rs = JsonSerializer.Deserialize<np.random_serializable>(jsonString, options)!;
        if (rs == null)
            return null;

        np.random rnd = new();
        rnd.FromSerialization(rs);
        return rnd;
    }

    public override void Write(Utf8JsonWriter writer, np.random value, JsonSerializerOptions options)
        => writer.WriteStringValue(JsonSerializer.Serialize(value.ToSerialization(), options));
}
