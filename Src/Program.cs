namespace serialog
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var options = ParseArguments(args);

            Application.Run(new Form1(options));
        }

        private static Dictionary<string, string> ParseArguments(string[] args)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    var parts = arg.Substring(2).Split('=', 2);
                    if (parts.Length == 2)
                    {
                        result[parts[0]] = parts[1];
                    }
                    else
                    {
                        // Handle flag without value (e.g. --autoconnect)
                        result[parts[0]] = "true";
                    }
                }
            }

            return result;
        }
    }
}