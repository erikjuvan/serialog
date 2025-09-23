namespace serialog
{
    public static class SettingsManager
    {
        public static class Default
        {
            // Default settings folder
            public static string SettingsFolder { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".settings");
            public static string HighlightPresetExt { get; } = ".hl";
            public static string SerialPresetExt { get; } = ".ser";

        }

        private static string _settingsFolder = Default.SettingsFolder;

        public static string SettingsFolder {
            get => _settingsFolder;
            set {
                if (string.IsNullOrWhiteSpace(value)) return;

                _settingsFolder = value;
                Directory.CreateDirectory(_settingsFolder);
                SettingsFolderChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static string SettingsFile => Path.Combine(SettingsFolder, "config.json");

        public static event EventHandler SettingsFolderChanged;

        public static CommandLineOptions LoadSettings(string[] args)
        {
            var fileOptions = LoadFromFile();
            var cliOptions = CommandLineOptions.Parse(args);
            return CommandLineOptions.Merge(fileOptions, cliOptions);
        }

        public static CommandLineOptions LoadFromFile()
        {
            if (!File.Exists(SettingsFile))
                return new CommandLineOptions();

            try
            {
                var json = File.ReadAllText(SettingsFile);
                return System.Text.Json.JsonSerializer.Deserialize<CommandLineOptions>(json) ?? new CommandLineOptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load settings from {SettingsFile}:\n{ex.Message}",
                    "Settings Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return new CommandLineOptions();
            }
        }

        public static void SaveToFile(CommandLineOptions options)
        {
            Directory.CreateDirectory(SettingsFolder);
            var json = System.Text.Json.JsonSerializer.Serialize(options, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFile, json);
        }
    }
}
