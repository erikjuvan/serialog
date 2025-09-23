using System.Text.Json;

namespace serialog
{
    public static class AppConfigurationManager
    {
        public static class Default
        {
            public static string Folder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".settings");
            public static string HighlightPresetExt { get; } = ".hl";
            public static string SerialPresetExt { get; } = ".ser";
            public static string ConfigFilename { get; } = "config.json";
        }

        private static string _folder = Default.Folder;

        public static string Folder {
            get => _folder;
            set {
                if (string.IsNullOrWhiteSpace(value)) return;
                _folder = value;
                Directory.CreateDirectory(_folder);
                FolderChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static string ConfigFile => Path.Combine(Folder, Default.ConfigFilename);

        public static event EventHandler FolderChanged;

        // Load settings from JSON file
        public static AppOptions LoadFromFile()
        {
            if (!File.Exists(ConfigFile)) return new AppOptions();

            try
            {
                var json = File.ReadAllText(ConfigFile);
                return JsonSerializer.Deserialize<AppOptions>(json) ?? new AppOptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load settings from {ConfigFile}:\n{ex.Message}",
                    "Settings Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return new AppOptions();
            }
        }

        // Save settings
        public static void SaveToFile(AppOptions options)
        {
            Directory.CreateDirectory(Folder);
            var json = JsonSerializer.Serialize(options, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFile, json);
        }

        // Parse command line arguments
        private static AppOptions ParseCommandLine(string[] args)
        {
            var options = new AppOptions();

            foreach (var arg in args)
            {
                if (!arg.StartsWith("--")) continue;

                var parts = arg.Substring(2).Split('=', 2);
                string key = parts[0].ToLowerInvariant();
                string value = parts.Length == 2 ? parts[1] : "true";

                switch (key)
                {
                    case "port": options.Port = value; break;
                    case "baud": if (int.TryParse(value, out int rate)) options.Baud = rate; break;
                    case "autoconnect": options.AutoConnect = value.Equals("true", StringComparison.OrdinalIgnoreCase); break;
                    case "load-highlight-preset": options.HighlightPresetFile = value; break;
                    case "load-serial-preset": options.SerialPresetFile = value; break;
                }
            }

            return options;
        }

        // Load options (from CLI + configuration file)
        public static AppOptions Load(string[] args)
        {
            var fileOptions = LoadFromFile();
            var cliOptions = ParseCommandLine(args);
            return Merge(fileOptions, cliOptions);
        }

        private static AppOptions Merge(AppOptions baseOptions, AppOptions overrideOptions)
        {
            return new AppOptions
            {
                Port = overrideOptions.Port ?? baseOptions.Port,
                Baud = overrideOptions.Baud != 0 ? overrideOptions.Baud : baseOptions.Baud,
                AutoConnect = overrideOptions.AutoConnect || baseOptions.AutoConnect,
                HighlightPresetFile = overrideOptions.HighlightPresetFile ?? baseOptions.HighlightPresetFile,
                SerialPresetFile = overrideOptions.SerialPresetFile ?? baseOptions.SerialPresetFile
            };
        }
    }

}
