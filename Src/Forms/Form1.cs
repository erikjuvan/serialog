using System.Diagnostics;

namespace serialog
{
    public partial class Form1 : Form
    {
        public class ProgramInfo
        {
            public static int ListviewBytes = 0;
            public static int PrevListviewBytes = 0;
            public static int SerialDataBytes = 0;
            public static int PrevSerialDataBytes = 0;
        }

        private System.IO.Ports.SerialPort _serialPort = new System.IO.Ports.SerialPort();
        private readonly SerialDataBuffer _serialDataBuffer = new SerialDataBuffer();
        private readonly DataParser _serialRXDataParser = new DataParser();
        private SerialReader _serialReader;

        private readonly DataLog _dataLog = new DataLog();
        private readonly DataLog _dataLogEditable = new DataLog();

        private int _uiUpdatePending = 0; // 0 = none pending, 1 = pending

        public bool DisplayNonPrintableCharsAsHex = false;

        private static bool _serialcomStopped = true;
        private bool serialcomStoppedHandleEvent = new bool();

        private static readonly object _serialDataLock = new object();

        private Stopwatch runTime = new Stopwatch();
        private Stopwatch upTime = new Stopwatch();

        private string _cmdlineHighlightPresetFilename = "";
        private string _cmdlineSerialPresetFilename = "";

        // Forms
        private FormHighlight formHighlight = null;
        private FormSerialSend formSerialSend = null;

        // Pipe server for IPC
        private PipeServer pipeServer;

        public Form1(Dictionary<string, string> options)
        {
            InitializeComponent();

            // Parse command line arguments
            ParseCommandLineArguments(options);

            // Subscribe to highlight changes
            FormHighlight.Highlights.EntriesChanged += HighlightEntries_Changed;

            listView1.DataLog = _dataLogEditable;

            // Attach parser to UI updates
            _serialRXDataParser.LineParsed += bytes =>
            {
                string line = FormatHelpers.BytesToDisplayString(bytes, false, DisplayNonPrintableCharsAsHex);

                AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.SerialRX, line));
            };

            // Set up serial reader
            _serialReader = new SerialReader(_serialPort, _serialDataBuffer, _serialRXDataParser);

            upTime.Start();

            // Start the pipe server with command handling
            pipeServer = new PipeServer("SerialLoggerPipe", HandlePipeCommand);
            pipeServer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create Highlight form and load preset
            formHighlight = new FormHighlight();
            RegisterChild(formHighlight);
            if (_cmdlineHighlightPresetFilename != "")
                formHighlight.LoadPreset(_cmdlineHighlightPresetFilename);

            // Create Serial Send form
            formSerialSend = new FormSerialSend(this, _serialPort, _dataLog);
            RegisterChild(formSerialSend);
            if (_cmdlineSerialPresetFilename != "")
                formSerialSend.LoadPreset(_cmdlineSerialPresetFilename);

            FitListviewToWidth();

            RefreshComPortComboBox();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            FitListviewToWidth();
        }

        private void FitListviewToWidth()
        {
            // Optionally subtract a bit for the form borders and padding
            int availableWidth = listView1.Width - 10 - SystemInformation.VerticalScrollBarWidth;
            // For single-column ListView, fill entire width
            listView1.Columns[0].Width = availableWidth;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                listView1.Focus();
            }

            if (e.Control && e.KeyCode == Keys.F)
            {
                textBox_find.Select();
            }

            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    listView1.FindPrev(textBox_find.Text);
                }
                else
                {
                    listView1.FindNext(textBox_find.Text);
                }
            }
        }

        private void ParseCommandLineArguments(Dictionary<string, string> options)
        {
            // Apply user arguments
            if (options.TryGetValue("port", out var port))
            {
                string portString = port.ToString();

                // If the item exists in the comboBox, select it
                int index = comboBox_port.Items.IndexOf(portString);
                if (index >= 0)
                {
                    comboBox_port.SelectedIndex = index;
                }
                else
                {
                    // If it's not in the list, just set the text
                    comboBox_port.Text = portString;
                }
            }
            else if (comboBox_port.Items.Count > 0)
            {
                 comboBox_port.SelectedIndex = 0;
            }

            if (options.TryGetValue("baud", out var baudStr) &&
                int.TryParse(baudStr, out var rate))
            {
                string rateString = rate.ToString();

                // If the item exists in the comboBox, select it
                int index = comboBox_baud.Items.IndexOf(rateString);
                if (index >= 0)
                {
                    comboBox_baud.SelectedIndex = index;
                }
                else
                {
                    // If it's not in the list, just set the text
                    comboBox_baud.Text = rateString;
                }
            }
            else
            {
                comboBox_baud.SelectedItem = "921600";
            }

            if (options.TryGetValue("autoconnect", out var ac))
            {
                bool autoConnect = ac.Equals("true", StringComparison.OrdinalIgnoreCase);

                if (autoConnect)
                {
                    button_run_Click(this, EventArgs.Empty);
                }
            }

            if (options.TryGetValue("load-highlight-preset", out var highlightPresetFilename))
            {
                _cmdlineHighlightPresetFilename = highlightPresetFilename.ToString();
            }

            if (options.TryGetValue("load-serial-preset", out var serialPresetFilename))
            {
                _cmdlineSerialPresetFilename = serialPresetFilename.ToString();
            }
        }

        private void HandlePipeCommand(string command)
        {
            // Ensure cross-thread safety for UI
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => HandlePipeCommand(command)));
                return;
            }

            if (command == "disconnect")
            {
                try
                {
                    DisconnectSerial();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during disconnect: {ex.Message}");
                }
            }
            else if (command == "connect")
            {
                try
                {
                    ConnectSerial();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during connect: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"Unknown pipe command: {command}");
            }
        }

        public void AddLogEntry(DataEntry entry)
        {
            _dataLog.Add(entry);
            _dataLogEditable.Add(entry);

            // Try to schedule one UI update if none is queued yet
            if (Interlocked.Exchange(ref _uiUpdatePending, 1) == 0)
            {
                // Queue a single UI update. When that runs, it will clear the pending flag.
                BeginInvoke(new Action(ProcessPendingUiUpdate));
            }
        }

        private void ProcessPendingUiUpdate()
        {
            try
            {
                // Optional: avoid visible flicker while updating
                listView1.BeginUpdate();
                try
                {
                    // Grow virtual size to match the log size
                    listView1.VirtualListSize = _dataLogEditable.Count;

                    // If follow is ON and user is already at bottom (or near it), scroll to end
                    if (checkBox_follow.Checked)
                    {
                        listView1.Follow();
                    }

                    // Redraw visible region (Invalidate is usually fine)
                    listView1.Invalidate();
                }
                finally
                {
                    listView1.EndUpdate();
                }
            }
            finally
            {
                // Allow scheduling the next batch of updates
                Interlocked.Exchange(ref _uiUpdatePending, 0);
            }
        }

        private void HighlightEntries_Changed(object? sender, EventArgs e)
        {
            listView1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialcomStoppedHandleEvent)
            {
                serialcomStoppedHandleEvent = false;

                if (addStartStopTimestampToolStripMenuItem.Checked)
                {
                    AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.User, "ACQUISITION STOPPED"));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pipeServer?.Stop();

            // Serial comm
            _serialcomStopped = true;
            _serialPort.Close();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && e.Modifiers == Keys.None)
            {
                checkBox_follow.Checked = !checkBox_follow.Checked;
                e.Handled = true;
            }
        }

        private void listView1_Scrolled(object sender, EventArgs e)
        {
            checkBox_follow.Checked = false;
        }

        void RefreshComPortComboBox()
        {
            var ports = Helpers.SerialPort.GetPorts(); // get fresh list
            int previousIndex = comboBox_port.SelectedIndex;

            comboBox_port.Items.Clear();

            foreach (var port in ports)
                comboBox_port.Items.Add(port.Description);

            // Restore previous selection if still valid
            if (previousIndex >= 0 && previousIndex < comboBox_port.Items.Count)
                comboBox_port.SelectedIndex = previousIndex;
            else if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0; // default to first item

            // Set collapsed text to COM name of selected port
            if (comboBox_port.SelectedIndex >= 0)
                comboBox_port.Text = ports[comboBox_port.SelectedIndex].PortName;

            // Resize dropdown
            AdjustComboBoxDropDownWidth(comboBox_port);
        }

        private void comboBox_port_DropDown(object sender, EventArgs e)
        {
            RefreshComPortComboBox();
        }

        private void comboBox_port_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var ports = Helpers.SerialPort.GetPorts();
            int index = comboBox_port.SelectedIndex;
            if (index >= 0 && index < ports.Length)
            {
                string comName = ports[index].PortName;

                // Delay assignment to avoid ComboBox overwriting
                comboBox_port.BeginInvoke((Action)(() =>
                {
                    comboBox_port.Text = comName;
                    comboBox_port.SelectAll(); // optional: highlights all text in collapsed ComboBox
                }));
            }
        }

        // Helper to resize dropdown width
        private void AdjustComboBoxDropDownWidth(ComboBox combo)
        {
            int maxWidth = 0;
            using (var g = combo.CreateGraphics())
            {
                foreach (var item in combo.Items)
                {
                    string text = item.ToString();
                    int width = TextRenderer.MeasureText(text, combo.Font).Width;
                    if (width > maxWidth)
                        maxWidth = width;
                }
            }
            combo.DropDownWidth = maxWidth + 20;
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            ConnectSerial();
        }

        private void ConnectSerial()
        {
            if (_serialcomStopped)
            {
                int baud = 0;
                bool success = int.TryParse(comboBox_baud.Text, out baud);

                if (!success)
                    return;

                try
                {
                    _serialPort.BaudRate = baud;
                    _serialPort.PortName = comboBox_port.Text;
                    _serialPort.Parity = System.IO.Ports.Parity.None;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = System.IO.Ports.StopBits.One;
                    _serialPort.Handshake = System.IO.Ports.Handshake.None;
                    _serialPort.ReadTimeout = 100;
                    _serialPort.WriteTimeout = 100;
                    _serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                _serialcomStopped = false;
                button_stop.Enabled = true;
                button_run.Enabled = false;

                comboBox_port.Enabled = false;
                comboBox_baud.Enabled = false;

                if (addStartStopTimestampToolStripMenuItem.Checked)
                {
                    AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.User, "ACQUISITION STARTED"));
                }

                runTime.Start();
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            DisconnectSerial();
        }

        private void DisconnectSerial()
        {
            if (!_serialcomStopped)
            {
                _serialcomStopped = true;

                _serialPort.Close();

                button_stop.Enabled = false;
                button_run.Enabled = true;
                comboBox_port.Enabled = true;
                comboBox_baud.Enabled = true;

                serialcomStoppedHandleEvent = true;

                runTime.Stop();
            }
        }

        private void textBox_find_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool found = e.Shift
                    ? listView1.FindPrev(textBox_find.Text)
                    : listView1.FindNext(textBox_find.Text);

                if (!found)
                    MessageBox.Show("No match");
            }
        }

        private void button_findnext_Click(object sender, EventArgs e)
        {
            bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
            bool found = ctrl
                ? listView1.FindLast(textBox_find.Text)
                : listView1.FindNext(textBox_find.Text);

            if (!found)
                MessageBox.Show("No match");
        }

        private void button_findprev_Click(object sender, EventArgs e)
        {
            bool ctrl = (ModifierKeys & Keys.Control) == Keys.Control;
            bool found = ctrl
                ? listView1.FindFirst(textBox_find.Text)
                : listView1.FindPrev(textBox_find.Text);

            if (!found)
                MessageBox.Show("No match");
        }

        private void button_findall_Click(object sender, EventArgs e)
        {
            bool found = listView1.FindAll(textBox_find.Text);
            if (!found)
                MessageBox.Show("No match");
        }

        private void button_prev_highlight_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                listView1.FindFirstHighlighted();
            }
            else
            {
                listView1.FindPrevHighlighted();
            }
        }

        private void button_next_highlight_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                listView1.FindLastHighlighted();
            }
            else
            {
                listView1.FindNextHighlighted();
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog1.FileName;
            AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.User, "Opening file: " + filePath));

            string[] lines = await System.IO.File.ReadAllLinesAsync(filePath);

            var batch = new List<ListViewItem>();
            int batchSize = 100; // adjust for performance vs. responsiveness

            await ProgressForm.Helper.RunWithProgressAsync(
                this,
                "Opening file...",
                lines.Length,
                async (i) =>
                {
                    DataEntryParser.TryParse(lines[i], out var entry);
                    AddLogEntry(entry);

                    await Task.Yield(); // keep UI responsive
                },
                onUIThread: true
            );

            AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.User, "End of file: " + filePath));

            listView1.RefreshEntries();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                string filename = saveFileDialog1.FileName;

                // Convert DataLog snapshot into readable lines
                var lines = _dataLog.GetSnapshot().Select(entry => entry.ToString());

                File.WriteAllLines(filename, lines);
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select lines to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string filename = saveFileDialog1.FileName;

            // Get lines from the backing data based on selected indices
            var lines = listView1.SelectedIndices
                .Cast<int>()
                .Select(i => _dataLog.GetSnapshot()[i].ToString())  // or ToDecoratedString() / ToRawString()
                .ToArray();

            File.WriteAllLines(filename, lines);
        }

        private void saveSerialAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                string filename = saveFileDialog1.FileName;

                // Save raw bytes
                File.WriteAllBytes(filename, _serialDataBuffer.GetSnapshot());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            try
            {
                for (int i = 0; i < listView1.VirtualListSize; i++)
                {
                    listView1.SelectedIndices.Add(i);
                }
            }
            finally
            {
                listView1.EndUpdate();
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear shared data safely
            lock (_serialDataLock)
            {
                listView1.ClearView();
            }

            runTime = new Stopwatch();
            if (!_serialcomStopped)
                runTime.Start();
        }

        private static DialogResult ShowInputDialogBox(ref string input, string prompt, string title = "Title", int width = 300, int height = 120)
        {
            //This function creates the custom input dialog box by individually creating the different window elements and adding them to the dialog box

            //Specify the size of the window using the parameters passed
            Size size = new Size(width, height);
            //Create a new form using a System.Windows Form
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            //Set the window title using the parameter passed
            inputBox.Text = title;

            //Create a new label to hold the prompt
            Label label = new Label();
            label.Text = prompt;
            label.Location = new Point(5, 5);
            label.Width = size.Width - 10;
            inputBox.Controls.Add(label);

            //Create a textbox to accept the user's input
            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, label.Location.Y + 25);
            textBox.Text = input;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            inputBox.Controls.Add(textBox);

            //Create an OK Button 
            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(71, 38);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, textBox.Bottom + 10);
            inputBox.Controls.Add(okButton);

            //Create a Cancel Button
            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(71, 38);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(size.Width - 80, textBox.Bottom + 10);
            inputBox.Controls.Add(cancelButton);

            //Set the input box's buttons to the created OK and Cancel Buttons respectively so the window appropriately behaves with the button clicks
            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            //Show the window dialog box 
            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;

            //After input has been submitted, return the input value
            return result;
        }

        private void addCustomRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = listView1.SelectedIndices;
            int addRowAtIndex = listView1.Items.Count;

            if (selected.Count > 0)
                addRowAtIndex = selected[selected.Count - 1] + 1;

            string input = "";
            var result = ShowInputDialogBox(ref input, "Enter text: ", "Custom row entry");

            if (result == DialogResult.OK)
            {
                AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.User, input));
            }
        }

        private void hideUnhighlightedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideUnhighlightedToolStripMenuItem.Checked = !hideUnhighlightedToolStripMenuItem.Checked;
            hideUnhighlightedContextMenuItem.Checked = hideUnhighlightedToolStripMenuItem.Checked;

            listView1.HideNonMatchingLines = hideUnhighlightedToolStripMenuItem.Checked;

            listView1.Invalidate();
        }

        private void disableHighlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableHighlightsToolStripMenuItem.Checked = !disableHighlightsToolStripMenuItem.Checked;
            disableHighlightsContextMenuItem.Checked = disableHighlightsToolStripMenuItem.Checked;

            listView1.HighlightsDisabled = disableHighlightsToolStripMenuItem.Checked;

            listView1.Invalidate();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;
            fontDialog1.ShowApply = true;
            fontDialog1.ShowEffects = true;
            fontDialog1.ShowHelp = true;

            fontDialog1.MinSize = 6;
            fontDialog1.MaxSize = 20;

            fontDialog1.Font = listView1.Font;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                listView1.SetFont(fontDialog1.Font);
            }
        }

        private void settingsFolderChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = AppSettings.SettingsFolder // start in current folder
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Store the absolute path directly
                AppSettings.SettingsFolder = dialog.SelectedPath;
                Directory.CreateDirectory(AppSettings.SettingsFolder); // ensure it exists

                MessageBox.Show(
                    $"Settings directory set to '{AppSettings.SettingsFolder}'",
                    "Directory",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void settingsFolderResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppSettings.SettingsFolder = AppSettings.DefaultSettingsFolder;

            MessageBox.Show(
                $"Settings directory reset to '{AppSettings.SettingsFolder}'",
                "Directory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void RegisterChild(Form child)
        {
            child.Owner = this;
            child.ShowInTaskbar = false;
            child.StartPosition = FormStartPosition.Manual;
            child.Left = this.Location.X + this.Width / 2 - child.Width / 2;
            child.Top = this.Location.Y + this.Height / 2 - child.Height / 2;

            child.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    child.Hide();

                    // schedule activation after Hide completes
                    this.BeginInvoke((Action)(() => ActivateOwnerSafely()));
                }
            };

            child.VisibleChanged += (s, e) =>
            {
                if (!child.Visible)
                {
                    // schedule activation after Hide completes
                    this.BeginInvoke((Action)(() => ActivateOwnerSafely()));
                }
            };

            // Optional: keep/ensure Escape hides the child (you already do this in the child)
            child.KeyPreview = true;
            child.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) { child.Hide(); e.Handled = true; } };
        }

        private void ActivateOwnerSafely()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            // restore if minimized
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // try usual managed activation
            this.Activate();
        }

        private void formHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formHighlight.Show();   // unhide if hidden
            formHighlight.Focus();
        }

        private void toolsAddViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new FormView
            {
                Owner = this,
                ShowInTaskbar = false,
            };

            child.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    child.Close();
                    e.Handled = true;
                }
            };

            child.Show();
        }

        private void toolsSerialSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSerialSend.Show();   // unhide if hidden
            formSerialSend.Focus();
        }

        private void checkBoxRegex_CheckedChanged(object sender, EventArgs e)
        {
            ListViewVirtExtensions.UseRegex = checkBoxRegex.Checked;
        }

        private void nonPrintableCharsAsHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayNonPrintableCharsAsHex = nonPrintableCharsAsHexToolStripMenuItem.Checked;
        }

        private void timer_updatesysinfo_Tick(object sender, EventArgs e)
        {
            var up = upTime.Elapsed;
            string ups = "";
            if (up.Hours > 0) ups += up.Hours.ToString() + ":";
            if (up.Minutes > 0) ups += up.Minutes.ToString("00") + ":";
            if (up.Seconds > 0) ups += up.Seconds.ToString("00");

            var run = runTime.Elapsed;
            string runs = "";
            if (run.Hours > 0) runs += run.Hours.ToString() + ":";
            if (run.Minutes > 0) runs += run.Minutes.ToString("00") + ":";
            if (run.Seconds > 0) runs += run.Seconds.ToString("00");

            ProgramInfo.SerialDataBytes = _serialDataBuffer.Count;
            string serialSizeStr = FormatHelpers.NumberToBKBMB(ProgramInfo.SerialDataBytes);

            double serialBytesPerSec = (double)(ProgramInfo.SerialDataBytes - ProgramInfo.PrevSerialDataBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            ProgramInfo.PrevSerialDataBytes = ProgramInfo.SerialDataBytes;
            string serialSpeedStr = FormatHelpers.NumberToBKBMB(serialBytesPerSec, "/s");
            double avgSerialBytesPerSec = ProgramInfo.SerialDataBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
            string avgSerialSpeedStr = FormatHelpers.NumberToBKBMB(avgSerialBytesPerSec, "/s");

            //ProgramInfo.ListviewBytes = _dataLog.Size;
            // So that saved file size will be the same as the one in the label subtract one byte (last newline)                       
            double listSize = ProgramInfo.ListviewBytes > 0 ? ProgramInfo.ListviewBytes - 1 : 0;
            string listSizeStr = FormatHelpers.NumberToBKBMB(listSize);

            double listBytesPerSec = (double)(ProgramInfo.ListviewBytes - ProgramInfo.PrevListviewBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            ProgramInfo.PrevListviewBytes = ProgramInfo.ListviewBytes;
            string listSpeedStr = FormatHelpers.NumberToBKBMB(listBytesPerSec, "/s");
            double avgListBytesPerSec = ProgramInfo.ListviewBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
            string avgListSpeedStr = FormatHelpers.NumberToBKBMB(avgListBytesPerSec, "/s");

            int bytesToRead = 0;
            if (_serialPort.IsOpen)
                bytesToRead = _serialPort.BytesToRead;

            string availableBytesStr = FormatHelpers.NumberToBKBMB(bytesToRead);

            this.Text = "Serialog |" +
                "   Serial: " + serialSizeStr + " @ " + serialSpeedStr + " (avg. " + avgSerialSpeedStr + ")" +
                "   List: " + listSizeStr + " @ " + listSpeedStr + " (avg. " + avgListSpeedStr + ")" +
                "   Available: " + availableBytesStr +
                "   Alive: " + ups +
                "   Running: " + runs;
        }
    }
}
