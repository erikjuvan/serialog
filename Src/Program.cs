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

            // This is quite a hack to avoid an invalid item id after removing them from the FormView match and search comboboxes
            Application.ThreadException += (sender, e) =>
            {
                if (e.Exception is ArgumentOutOfRangeException aoore &&
                    aoore.StackTrace?.Contains("System.Windows.Forms.ComboBox") == true)
                {
                    // Ignore the ComboBox SelectedIndex glitch
                    //MessageBox.Show(e.Exception.ToString(), "Ignoring combobox glitch");
                    return; // swallow
                }

                // For all other exceptions, rethrow or log
                MessageBox.Show(e.Exception.ToString(), "Unhandled UI Exception");
            };

            Application.Run(new Form1());
        }
    }
}