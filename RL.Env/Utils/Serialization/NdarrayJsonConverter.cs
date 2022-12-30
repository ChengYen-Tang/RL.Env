using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Env.Utils.Serialization;
internal class NdarrayJsonConverter : JsonConverter<ndarray>
{
    public override ndarray? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? jsonString = reader.GetString();
        if (jsonString == null)
            return null;
        ndarray_serializable? ns = JsonSerializer.Deserialize<ndarray_serializable>(jsonString, options)!;
        return ns == null ? null : np.FromSerializable(ns);
    }

    public override void Write(Utf8JsonWriter writer, ndarray value, JsonSerializerOptions options)
        => writer.WriteStringValue(JsonSerializer.Serialize(value.ToSerializable(), options));
}
