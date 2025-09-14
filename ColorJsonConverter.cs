using System.Text.Json;
using System.Text.Json.Serialization;

namespace serialog
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return Color.Black;

            // Try named color
            Color c = Color.FromName(value);
            if (c.IsKnownColor || c.IsNamedColor)
                return c;

            // Fallback: ARGB
            if (int.TryParse(value, out int argb))
                return Color.FromArgb(argb);

            return Color.Black;
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            // Prefer named color if possible
            if (value.IsKnownColor || value.IsNamedColor)
                writer.WriteStringValue(value.Name);
            else
                writer.WriteStringValue(value.ToArgb().ToString());
        }
    }
}
