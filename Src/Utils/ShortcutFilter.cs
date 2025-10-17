namespace serialog
{
    public class ShortcutFilter : IMessageFilter
    {
        private readonly Form1 _mainForm;

        public ShortcutFilter(Form1 mainForm)
        {
            _mainForm = mainForm;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_KEYDOWN = 0x0100;

            if (m.Msg == WM_KEYDOWN)
            {
                Keys key = (Keys)m.WParam;
                bool ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;

                // Ctrl+H → Highlight window
                if (ctrl && key == Keys.D1)
                {
                    _mainForm.ToggleToolWindow(1);
                    return true;
                }

                // Ctrl+1 → Serial Send windows
                else if (ctrl && key == Keys.D2)
                {
                    _mainForm.ToggleToolWindow(2);
                    return true;
                }

                // Ctrl+2 → View windows
                else if (ctrl && key == Keys.D3)
                {
                    _mainForm.ToggleToolWindow(3);
                    return true;
                }
                
                // All possible windows
                else if (key == Keys.Oem3)
                {
                    _mainForm.ToggleAllToolWindows();
                }
            }

            return false;
        }
    }
}