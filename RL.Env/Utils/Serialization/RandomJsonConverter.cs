using System.Text.Json;

namespace RL.Env.Utils.Serialization;

public class RandomJsonConverter : System.Text.Json.Serialization.JsonConverter<np.random>
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

public class RandomConverter : Newtonsoft.Json.JsonConverter<np.random>
{
    public override np.random? ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, np.random? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;
        if (reader.Value is not string jsonString)
            return null;
        np.random_serializable? rs = Newtonsoft.Json.JsonConvert.DeserializeObject<np.random_serializable>(jsonString);
        if (rs == null)
            return null;

        np.random rnd = new();
        rnd.FromSerialization(rs);
        return rnd;
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, np.random? value, Newtonsoft.Json.JsonSerializer serializer)
        => writer.WriteValue(Newtonsoft.Json.JsonConvert.SerializeObject(value?.ToSerialization()));
}
