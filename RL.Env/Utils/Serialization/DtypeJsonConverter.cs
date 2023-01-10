using System.Text.Json;

namespace RL.Env.Utils.Serialization;

public class DtypeJsonConverter : System.Text.Json.Serialization.JsonConverter<dtype>
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

public class DtypeConverter : Newtonsoft.Json.JsonConverter<dtype>
{
    public override dtype? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, dtype? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;
        if (reader.Value is not string jsonString)
            return null;
        dtype_serializable? dtypeSerializedFormat = Newtonsoft.Json.JsonConvert.DeserializeObject<dtype_serializable>(jsonString);
        return dtypeSerializedFormat == null ? null : new(dtypeSerializedFormat);
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, dtype? value, Newtonsoft.Json.JsonSerializer serializer)
        => writer.WriteValue(Newtonsoft.Json.JsonConvert.SerializeObject(value?.ToSerializable()));
}
