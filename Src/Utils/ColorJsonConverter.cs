using System.Text.Json;
using System.Text.Json.Serialization;

namespace serialog
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            return Helpers.ParseColorInput(value ?? string.Empty, Color.Black);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            if (value.IsKnownColor || value.IsNamedColor)
            {
                writer.WriteStringValue(value.Name);
            }
            else
            {
                // Always write as #AARRGGBB
                writer.WriteStringValue($"#{value.ToArgb():X8}");
            }
        }
    }
}
