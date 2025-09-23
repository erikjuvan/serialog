using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Text;

namespace serialog
{
    public static class Helpers
    {
        public class SerialPort
        {
            private const int DIGCF_PRESENT = 0x02;
            private const int SPDRP_FRIENDLYNAME = 0x0C;

            [StructLayout(LayoutKind.Sequential)]
            private struct SP_DEVINFO_DATA
            {
                public int cbSize;
                public Guid ClassGuid;
                public int DevInst;
                public IntPtr Reserved;
            }

            [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SetupDiGetClassDevs(
                ref Guid ClassGuid,
                IntPtr Enumerator,
                IntPtr hwndParent,
                uint Flags);

            [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
            private static extern bool SetupDiEnumDeviceInfo(
                IntPtr DeviceInfoSet,
                int MemberIndex,
                ref SP_DEVINFO_DATA DeviceInfoData);

            [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
            private static extern bool SetupDiGetDeviceRegistryProperty(
                IntPtr DeviceInfoSet,
                ref SP_DEVINFO_DATA DeviceInfoData,
                uint Property,
                out uint PropertyRegDataType,
                byte[] PropertyBuffer,
                uint PropertyBufferSize,
                out uint RequiredSize);

            [DllImport("setupapi.dll")]
            private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

            // GUID for "Ports" device class
            private static Guid GUID_DEVCLASS_PORTS = new Guid("4d36e978-e325-11ce-bfc1-08002be10318");

            public class SerialPortItem
            {
                public string PortName { get; set; }    // COM3
                public string Description { get; set; } // USB Serial Device (COM3)

                public override string ToString() => PortName;
            }

            public static SerialPortItem[] GetPorts()
            {
                List<SerialPortItem> ports = new List<SerialPortItem>();

                // First get COM port names
                string[] portNames = System.IO.Ports.SerialPort.GetPortNames();

                // Now get friendly names from SetupAPI
                IntPtr hDevInfo = SetupDiGetClassDevs(ref GUID_DEVCLASS_PORTS, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT);
                if (hDevInfo == IntPtr.Zero)
                    return Array.Empty<SerialPortItem>();

                SP_DEVINFO_DATA devInfo = new SP_DEVINFO_DATA();
                devInfo.cbSize = Marshal.SizeOf(devInfo);

                Dictionary<string, string> descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, ref devInfo); i++)
                {
                    byte[] buffer = new byte[256];
                    if (SetupDiGetDeviceRegistryProperty(hDevInfo, ref devInfo, SPDRP_FRIENDLYNAME,
                            out _, buffer, (uint)buffer.Length, out _))
                    {
                        string desc = Encoding.Unicode.GetString(buffer).TrimEnd('\0');

                        // Extract COM name from description if it contains "(COMx)"
                        int start = desc.LastIndexOf("(COM", StringComparison.OrdinalIgnoreCase);
                        int end = desc.LastIndexOf(")");
                        if (start >= 0 && end > start)
                        {
                            string comName = desc.Substring(start + 1, end - start - 1); // e.g., "COM3"
                            descriptions[comName] = desc;
                        }
                    }
                }

                SetupDiDestroyDeviceInfoList(hDevInfo);

                // Create items for each port
                foreach (var port in portNames)
                {
                    ports.Add(new SerialPortItem
                    {
                        PortName = port,
                        Description = descriptions.ContainsKey(port) ? descriptions[port] : port
                    });
                }

                // Sort numerically by COM number
                ports.Sort((a, b) =>
                {
                    int aNum = 0, bNum = 0;
                    bool aIsCom = a.PortName.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                                  int.TryParse(a.PortName.Substring(3), out aNum);
                    bool bIsCom = b.PortName.StartsWith("COM", StringComparison.OrdinalIgnoreCase) &&
                                  int.TryParse(b.PortName.Substring(3), out bNum);

                    if (aIsCom && bIsCom) return aNum.CompareTo(bNum);
                    if (aIsCom) return -1;
                    if (bIsCom) return 1;
                    return string.Compare(a.PortName, b.PortName, StringComparison.OrdinalIgnoreCase);
                });

                return ports.ToArray();
            }
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
