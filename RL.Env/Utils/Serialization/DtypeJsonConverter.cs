using System.Text.Json;
using System.Text.Json.Serialization;

namespace RL.Env.Utils.Serialization;

public class DtypeJsonConverter : JsonConverter<dtype>
{
    public override dtype? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? jsonString = reader.GetString();
        if (jsonString == null)
            return null;
        dtype_serializable? dtypeSerializedFormat = JsonSerializer.Deserialize<dtype_serializable>(jsonString, options);
        return dtypeSerializedFormat == null ? null : new(dtypeSerializedFormat);
    }

    public override void Write(Utf8JsonWriter writer, dtype value, JsonSerializerOptions options)
        => writer.WriteStringValue(JsonSerializer.Serialize(value.ToSerializable(), options));
}
