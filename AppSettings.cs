namespace serialog
{
    public static class AppSettings
    {
        public static string DefaultSettingsFolder { get; } = Path.Combine(Application.StartupPath, ".settings");
        private static string _settingsFolder = DefaultSettingsFolder;

        public static string SettingsFolder {
            get => _settingsFolder;
            set {
                _settingsFolder = value;
                Directory.CreateDirectory(_settingsFolder);
                SettingsFolderChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler SettingsFolderChanged;
    }
}
