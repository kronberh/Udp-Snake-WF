using System.Text.Json.Serialization;
using System.Text.Json;

namespace ns_Converters
{
    internal class ColorConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement root = doc.RootElement;
            int r = root.GetProperty("R").GetInt32();
            int g = root.GetProperty("G").GetInt32();
            int b = root.GetProperty("B").GetInt32();
            return Color.FromArgb(r, g, b);
        }
        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("R", value.R);
            writer.WriteNumber("G", value.G);
            writer.WriteNumber("B", value.B);
            writer.WriteEndObject();
        }
    }
}
