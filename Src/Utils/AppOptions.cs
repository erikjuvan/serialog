public class AppOptions
{
    public string Port { get; set; } = null;
    public int Baud { get; set; } = 921600;
    public bool AutoConnect { get; set; } = false;
    public string HighlightPresetFile { get; set; } = null;
    public string SerialPresetFile { get; set; } = null;
}
