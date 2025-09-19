using System.Diagnostics;

namespace serialog
{
    public partial class Form1 : Form
    {
        private System.IO.Ports.SerialPort _serialPort = new System.IO.Ports.SerialPort();
        private readonly SerialDataBuffer _serialDataBuffer = new SerialDataBuffer();
        private readonly DataParser _serialRXDataParser = new DataParser();
        private SerialReader _serialReader;

        private readonly DataLog _dataLog = new DataLog();
        private readonly DataLog _dataLogEditable = new DataLog();

        private int _uiUpdatePending = 0; // 0 = none pending, 1 = pending

        private static bool _serialcomStopped = true;
        private bool serialcomStoppedHandleEvent = new bool();
        private static int _listviewSizeBytes = 0;
        private static int _prevListviewSizeBytes = 0;
        private static int _serialDataListSizeBytes = 0;
        private static int _prevSerialDataListSizeBytes = 0;
        private static readonly object _serialDataLock = new object();

        private Stopwatch runTime = new Stopwatch();
        private Stopwatch upTime = new Stopwatch();

        private Form2_Highlight form2Highlight = null;
        private Form4_Send form4Send = null;
        
        public Form1(Dictionary<string, string> options)
        {
            InitializeComponent();

            comboBox_port.Items.AddRange(Helpers.GetSortedPorts());

            // Parse command line arguments
            ParseCommandLineArguments(options);

            // Subscribe to highlight changes
            Form2_Highlight.highlightEntries.EntriesChanged += HighlightEntries_Changed;

            listView1.DataLog = _dataLogEditable;

            // Let listview see the Highlights 
            listView1.HighlightItems = Form2_Highlight.highlightEntries.Items;

            // Attach parser to UI updates
            _serialRXDataParser.LineParsed += bytes =>
            {
                string line = FormatHelpers.BytesToDisplayString(bytes, hideNonPrintableCharsToolStripMenuItem.Checked);

                AddLogEntry(new DataEntry(line, DateTime.Now, DataEntrySource.SerialRX));
            };

            // Set up serial reader
            _serialReader = new SerialReader(_serialPort, _serialDataBuffer, _serialRXDataParser);

            upTime.Start();
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
                    FindPrevString(textBox_find.Text);
                }
                else
                {
                    FindNextString(textBox_find.Text);
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
        }

        public void AddLogEntry(DataEntry entry)
        {
            // Add to master log (assumed thread-safe)
            _dataLog.Add(entry);

            // Add to editable log connected to listview (thread-safe)
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
                    AddLogEntry(new DataEntry("ACQUISITION STOPPED", DateTime.Now, DataEntrySource.User));
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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


            string serialSizeStr = FormatHelpers.NumberToBKBMB(_serialDataListSizeBytes);

            double serialBytesPerSec = (double)(_serialDataListSizeBytes - _prevSerialDataListSizeBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            _prevSerialDataListSizeBytes = _serialDataListSizeBytes;
            string serialSpeedStr = FormatHelpers.NumberToBKBMB(serialBytesPerSec, "/s");
            double avgSerialBytesPerSec = _serialDataListSizeBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
            string avgSerialSpeedStr = FormatHelpers.NumberToBKBMB(avgSerialBytesPerSec, "/s");

            // So that saved file size will be the same as the one in the label subtract one byte (last newline)                       
            double listSize = _listviewSizeBytes > 0 ? _listviewSizeBytes - 1 : 0;
            string listSizeStr = FormatHelpers.NumberToBKBMB(listSize);

            double listBytesPerSec = (double)(_listviewSizeBytes - _prevListviewSizeBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            _prevListviewSizeBytes = _listviewSizeBytes;
            string listSpeedStr = FormatHelpers.NumberToBKBMB(listBytesPerSec, "/s");
            double avgListBytesPerSec = _listviewSizeBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
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

        private void comboBox_port_DropDown(object sender, EventArgs e)
        {
            comboBox_port.Items.Clear();
            comboBox_port.Items.AddRange(Helpers.GetSortedPorts());
            if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0;
        }

        private void button_run_Click(object sender, EventArgs e)
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
                    AddLogEntry(new DataEntry("ACQUISITION STARTED", DateTime.Now, DataEntrySource.User));
                }

                runTime.Start();
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
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
                string text = textBox_find.Text;

                if (e.Shift)
                {
                    FindPrevString(text);
                }
                else
                {
                    FindNextString(text);
                }
            }
        }

        private void FindLastString(string text)
        {
            if (text == "")
                return;

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    checkBox_follow.Checked = false;
                    listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    return;
                }
            }
            MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FindNextString(string text)
        {
            if (text == "")
                return;

            var selected = listView1.SelectedIndices;
            int searchFromIndex = 0;

            if (selected.Count > 0)
                searchFromIndex = selected[selected.Count - 1] + 1;

            for (int i = searchFromIndex; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    listView1.Select();
                    checkBox_follow.Checked = false;
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    return;
                }
            }
            MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_findnext_Click(object sender, EventArgs e)
        {
            string text = textBox_find.Text;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                FindLastString(text);
            }
            else
            {
                FindNextString(text);
            }
        }

        private void FindFirstString(string text)
        {
            if (text == "")
                return;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    checkBox_follow.Checked = false;
                    listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    return;
                }
            }
            MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FindPrevString(string text)
        {
            if (text == "")
                return;

            var selected = listView1.SelectedIndices;
            int searchFromIndex = listView1.Items.Count - 1;

            if (selected.Count > 0)
                searchFromIndex = selected[0] - 1;

            for (int i = searchFromIndex; i >= 0; i--)
            {
                if (listView1.Items[i].Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    checkBox_follow.Checked = false;
                    listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    return;
                }
            }
            MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_findprev_Click(object sender, EventArgs e)
        {
            string text = textBox_find.Text;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                FindFirstString(text);
            }
            else
            {
                FindPrevString(text);
            }
        }

        private void FindAllString(string text)
        {
            if (text == "")
                return;

            bool foundText = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    if (!foundText)
                    {
                        foundText = true;
                        listView1.Select();
                        listView1.SelectedItems.Clear();
                    }
                    listView1.Items[i].Selected = true;
                }
            }

            if (foundText)
            {
                checkBox_follow.Checked = false;
                listView1.Items[listView1.SelectedIndices[listView1.SelectedIndices.Count - 1]].EnsureVisible();
            }
            else
            {
                MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_findall_Click(object sender, EventArgs e)
        {
            string text = textBox_find.Text;

            FindAllString(text);
        }

        private void FindHighlighted(int startIdx, bool reverse = false)
        {
            if (reverse)
            {
                for (int i = startIdx; i >= 0; --i)
                {
                    var itm = listView1.Items[i];
                    if (itm.ForeColor.Name != "WindowText" || itm.BackColor.Name != "Window")
                    {
                        checkBox_follow.Checked = false;
                        listView1.Select();
                        listView1.SelectedItems.Clear();
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                        return;
                    }
                }
            }
            else
            {
                for (int i = startIdx; i < listView1.Items.Count; ++i)
                {
                    var itm = listView1.Items[i];
                    if (itm.ForeColor.Name != "WindowText" || itm.BackColor.Name != "Window")
                    {
                        checkBox_follow.Checked = false;
                        listView1.Select();
                        listView1.SelectedItems.Clear();
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                        return;
                    }
                }
            }

            MessageBox.Show("No match", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FindFirstHighlightedEntry()
        {
            FindHighlighted(0);
        }

        private void FindPrevHighlightedEntry()
        {
            var selected = listView1.SelectedIndices;
            int searchFromIndex = listView1.Items.Count - 1;

            if (selected.Count > 0)
                searchFromIndex = selected[0] - 1;

            FindHighlighted(searchFromIndex, true);
        }

        private void button_prev_highlight_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                FindFirstHighlightedEntry();
            }
            else
            {
                FindPrevHighlightedEntry();
            }
        }

        private void FindLastHighlightedEntry()
        {
            FindHighlighted(listView1.Items.Count - 1, true);
        }

        private void FindNextHighlightedEntry()
        {
            var selected = listView1.SelectedIndices;
            int searchFromIndex = 0;

            if (selected.Count > 0)
                searchFromIndex = selected[selected.Count - 1] + 1;

            FindHighlighted(searchFromIndex);
        }

        private void button_next_highlight_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                FindLastHighlightedEntry();
            }
            else
            {
                FindNextHighlightedEntry();
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog1.FileName;
            AddLogEntry(new DataEntry("Opening file: " + filePath, DateTime.Now, DataEntrySource.User));

            string[] lines = await System.IO.File.ReadAllLinesAsync(filePath);

            var batch = new List<ListViewItem>();
            int batchSize = 100; // adjust for performance vs. responsiveness

            await ProgressForm.Helper.RunWithProgressAsync(
                this,
                "Opening file...",
                lines.Length,
                async (i) =>
                {
                    AddLogEntry(new DataEntry(lines[i]));

                    await Task.Yield(); // keep UI responsive
                },
                onUIThread: true
            );

            AddLogEntry(new DataEntry("End of file: " + filePath, DateTime.Now, DataEntrySource.User));

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
                File.WriteAllBytes(filename, _serialDataBuffer.GetSnapshot().ToArray());
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
                // Add item
                // TODO
            }
        }

        private void hideUnhighlightedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideUnhighlightedToolStripMenuItem.Checked = !hideUnhighlightedToolStripMenuItem.Checked;
            hideUnhighlightedContextMenuItem.Checked = hideUnhighlightedToolStripMenuItem.Checked;

            listView1.HideNonMatchingLines = hideUnhighlightedToolStripMenuItem.Checked;
        }

        private void disableHighlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableHighlightsToolStripMenuItem.Checked = !disableHighlightsToolStripMenuItem.Checked;
            disableHighlightsContextMenuItem.Checked = disableHighlightsToolStripMenuItem.Checked;

            listView1.HighlightsDisabled = disableHighlightsToolStripMenuItem.Checked;
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
            }
        }

        private void settingsFolderResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppSettings.SettingsFolder = AppSettings.DefaultSettingsFolder;
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
            if (form2Highlight == null || form2Highlight.IsDisposed)
            {
                form2Highlight = new Form2_Highlight();
                RegisterChild(form2Highlight);
                form2Highlight.Show();
            }
            else
            {
                form2Highlight.Show();   // unhide if hidden
                form2Highlight.Focus();
            }
        }

        private void toolsAddViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new FormView
            {
                Owner = this,
                ShowInTaskbar = false,
            };
            child.Show();
        }

        private void formSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form4Send == null || form4Send.IsDisposed)
            {
                form4Send = new Form4_Send(this, _serialPort, _dataLog);
                RegisterChild(form4Send);
                form4Send.Show();
            }
            else
            {
                form4Send.Show();   // unhide if hidden
                form4Send.Focus();
            }
        }
    }
}
