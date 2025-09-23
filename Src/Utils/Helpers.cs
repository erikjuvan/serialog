using System.Text.RegularExpressions;

namespace serialog
{
    public static class Helpers
    {
        public static string[] GetSortedPorts()
        {
            var portNames = System.IO.Ports.SerialPort.GetPortNames();

            Array.Sort(portNames, (x, y) =>
            {
                int xNum = 0, yNum = 0;

                bool xIsCom = x.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                              int.TryParse(x.Substring(3), out xNum);

                bool yIsCom = y.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                              int.TryParse(y.Substring(3), out yNum);

                if (xIsCom && yIsCom)
                {
                    return xNum.CompareTo(yNum); // sort numerically
                }
                else if (xIsCom)
                {
                    return -1; // COM ports first
                }
                else if (yIsCom)
                {
                    return 1;
                }
                else
                {
                    return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
                }
            });

            return portNames;
        }

        public static bool MatchesPattern(string line, string pattern, bool useRegex)
        {
            if (useRegex)
            {
                try
                {
                    return Regex.IsMatch(line, pattern, RegexOptions.IgnoreCase);
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            return line.Contains(pattern);
        }

        public static Color ParseColorInput(string input, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(input))
                return fallback;

            input = input.Trim();

            // Hex input?
            if (input.StartsWith("#"))
            {
                string hex = input.Substring(1);

                // #RRGGBB
                if (hex.Length == 6 &&
                    int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
                {
                    return Color.FromArgb(
                        (rgb >> 16) & 0xFF,
                        (rgb >> 8) & 0xFF,
                        rgb & 0xFF
                    );
                }

                // #AARRGGBB
                if (hex.Length == 8 &&
                    int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int argb))
                {
                    return Color.FromArgb(
                        (argb >> 24) & 0xFF,
                        (argb >> 16) & 0xFF,
                        (argb >> 8) & 0xFF,
                        argb & 0xFF
                    );
                }
            }

            // Named color
            var named = Color.FromName(input);
            if (!named.IsEmpty)
                return named;

            // Fallback if nothing matched
            return fallback;
        }

        public static void PopulateComboBox(ComboBox comboBox, string extension, bool recursive)
        {
            comboBox.Items.Clear();

            try
            {
                string settingsDir = AppSettings.SettingsFolder;
                if (!Directory.Exists(settingsDir))
                    return;

                var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                var listOfFiles = Directory.EnumerateFiles(settingsDir, "*" + extension, searchOption)
                                           .Select(Path.GetFileNameWithoutExtension);

                foreach (var file in listOfFiles)
                    comboBox.Items.Add(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
