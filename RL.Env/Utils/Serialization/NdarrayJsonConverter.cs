using System.Text.Json;

namespace RL.Env.Utils.Serialization;
public class NdarrayJsonConverter : System.Text.Json.Serialization.JsonConverter<ndarray>
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

public class NdarrayConverter : Newtonsoft.Json.JsonConverter<ndarray>
{
    public override ndarray? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, ndarray? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;
        if (reader.Value is not string jsonString)
            return null;
        ndarray_serializable? ns = Newtonsoft.Json.JsonConvert.DeserializeObject<ndarray_serializable>(jsonString);
        return ns == null ? null : np.FromSerializable(ns);
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, ndarray? value, Newtonsoft.Json.JsonSerializer serializer)
        => writer.WriteValue(Newtonsoft.Json.JsonConvert.SerializeObject(value?.ToSerializable()));
}
