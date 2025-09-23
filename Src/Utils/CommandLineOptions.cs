using System.Text.Json;

namespace serialog
{
    public class CommandLineOptions
    {
        // Defaults
        public string Port { get; set; } = null;
        public int Baud { get; set; } = 921600;
        public bool AutoConnect { get; set; } = false;
        public string HighlightPresetFile { get; set; } = null;
        public string SerialPresetFile { get; set; } = null;

        public static CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions();

            foreach (var arg in args)
            {
                if (!arg.StartsWith("--")) continue;

                var parts = arg.Substring(2).Split('=', 2);
                string key = parts[0].ToLowerInvariant();
                string value = parts.Length == 2 ? parts[1] : "true"; // flags without value

                switch (key)
                {
                    case "port":
                        options.Port = value;
                        break;

                    case "baud":
                        if (int.TryParse(value, out int rate))
                            options.Baud = rate;
                        break;

                    case "autoconnect":
                        options.AutoConnect = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;

                    case "load-highlight-preset":
                        options.HighlightPresetFile = value;
                        break;

                    case "load-serial-preset":
                        options.SerialPresetFile = value;
                        break;

                    default:
                        Console.WriteLine($"Unknown option: {key}");
                        break;
                }
            }

            return options;
        }

        public static CommandLineOptions Merge(CommandLineOptions baseOptions, CommandLineOptions overrideOptions)
        {
            // CLI options override file options
            var result = new CommandLineOptions
            {
                Port = overrideOptions.Port ?? baseOptions.Port,
                Baud = overrideOptions.Baud != 0 ? overrideOptions.Baud : baseOptions.Baud,
                AutoConnect = overrideOptions.AutoConnect || baseOptions.AutoConnect,
                HighlightPresetFile = overrideOptions.HighlightPresetFile ?? baseOptions.HighlightPresetFile,
                SerialPresetFile = overrideOptions.SerialPresetFile ?? baseOptions.SerialPresetFile
            };
            return result;
        }

        public static CommandLineOptions FromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
                return new CommandLineOptions();

            var json = File.ReadAllText(filePath);
            try
            {
                return JsonSerializer.Deserialize<CommandLineOptions>(json) ?? new CommandLineOptions();
            }
            catch
            {
                // Could log error or ignore silently
                return new CommandLineOptions();
            }
        }
    }
}
