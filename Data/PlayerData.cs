using System.Text.Json;
using ColorConverter = ns_Converters.ColorConverter;

namespace ns_Data
{
    internal record PlayerData
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public static JsonSerializerOptions SerializerOptions { get; } = new() { Converters = { new ColorConverter() } };
        public PlayerData(string Name, Color Color)
        {
            this.Name = Name;
            this.Color = Color;
        }
    }
}