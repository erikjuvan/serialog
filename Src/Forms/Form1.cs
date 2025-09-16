using System.Diagnostics;
using System.Text;

namespace serialog
{
    public partial class Form1 : Form
    {
        private System.IO.Ports.SerialPort _serial = new System.IO.Ports.SerialPort();

        private readonly SerialDataBuffer _serialDataBuffer = new SerialDataBuffer();
        private readonly DataLog _dataLog = new DataLog();
        private readonly LogView _logView = new LogView();

        private static bool _serialcomStopped = new bool();
        private bool serialcomStoppedHandleEvent = new bool();
        private static List<string> _serialDataList = new List<string>(1000000);
        private static int _listviewSizeBytes = 0;
        private static int _prevListviewSizeBytes = 0;
        private static int _serialDataListSizeBytes = 0;
        private static int _prevSerialDataListSizeBytes = 0;
        private int _serialDataListCountAddedToTable = 0;
        private static readonly object _serialDataLock = new object();

        private Stopwatch runTime = new Stopwatch();
        private Stopwatch upTime = new Stopwatch();

        private Form2_Highlight form2Highlight = null;
        private Form3_Highlights form3Highlights = null;
        private Form4_Send form4Send = null;
        
        public Form1(Dictionary<string, string> options)
        {
            InitializeComponent();

            // Hook filtered view to ListView
            _logView.EntryAdded += entry =>
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(new Action(() => AddToListView(entry)));
                }
                else
                {
                    AddToListView(entry);
                }
            };

            comboBox_port.Items.AddRange(GetSortedPorts());

            _serialcomStopped = true;
            button_stop.Enabled = false;

            // Parse command line arguments
            ParseCommandLineArguments(options);

            // Subscribe to highlight changes
            Form2_Highlight.highlightEntries.EntriesChanged += HighlightEntries_Changed;

            upTime.Start();
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

        private void AddToListView(DataEntry entry)
        {
            var item = new ListViewItem(entry.ToString());
            if (entry.IsSent)
                item.ForeColor = Color.Blue; // TX in blue
            listView1.Items.Add(item);
            listView1.Items[listView1.Items.Count - 1].EnsureVisible();
        }

        private void HighlightEntries_Changed(object? sender, EventArgs e)
        {
            // Redraw your list view or refresh the virtual items
            //listView1.Invalidate(); // For future virtual listview
            //ReloadAllListViewItems(); // For current testing
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

        private string[] GetSortedPorts()
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    _serial.BaudRate = baud;
                    _serial.PortName = comboBox_port.Text;
                    _serial.Parity = System.IO.Ports.Parity.None;
                    _serial.DataBits = 8;
                    _serial.StopBits = System.IO.Ports.StopBits.One;
                    _serial.Handshake = System.IO.Ports.Handshake.None;
                    _serial.ReadTimeout = 100;
                    _serial.WriteTimeout = 100;
                    _serial.DataReceived += Serial_DataReceived;
                    _serial.Open();
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
                    var entry = new DataEntry(DateTime.Now, false, "ACQUISITION STARTED");
                    _logView.Add(entry);
                }

                runTime.Start();
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (!_serialcomStopped)
            {
                _serialcomStopped = true;

                _serial.Close();

                button_stop.Enabled = false;
                button_run.Enabled = true;
                comboBox_port.Enabled = true;
                comboBox_baud.Enabled = true;

                serialcomStoppedHandleEvent = true;

                runTime.Stop();
            }
        }

        private void comboBox_port_DropDown(object sender, EventArgs e)
        {
            comboBox_port.Items.Clear();
            comboBox_port.Items.AddRange(GetSortedPorts());
            if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0;
        }

        private List<byte> _serialLineBuffer = new List<byte>();

        private void Serial_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int count = _serial.BytesToRead;
            var buffer = new byte[count];
            _serial.Read(buffer, 0, count);

            _serialDataBuffer.Append(buffer, count);

            foreach (var b in buffer)
            {
                if (b == (byte)'\n')
                {
                    // If previous char was \r, drop it (classic CRLF)
                    if (_serialLineBuffer.Count > 0 && _serialLineBuffer[^1] == (byte)'\r')
                    {
                        _serialLineBuffer.RemoveAt(_serialLineBuffer.Count - 1);
                    }

                    string display = BytesToDisplayString(_serialLineBuffer);
                    var entry = new DataEntry(DateTime.Now, false, display);

                    _dataLog.Add(entry);
                    _logView.Add(entry);

                    _serialLineBuffer.Clear();
                }
                else
                {
                    _serialLineBuffer.Add(b);
                }
            }
        }

        // Convert bytes to readable string
        private string BytesToDisplayString(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                if (b >= 32 && b <= 126)
                    sb.Append((char)b);
                else
                    sb.Append($"{{0x{b:X2}}}");
            }
            return sb.ToString();
        }

        private ListViewItem CreateHighlightedListItem(string line)
        {
            var item = new ListViewItem(line);

            if (disableHighlightsToolStripMenuItem.Checked)
                return item;

            foreach (HighlightEntry entry in Form2_Highlight.highlightEntries.Items)
            {
                if (!entry.Enabled) continue;

                string haystack = entry.IgnoreCase ? line.ToLower() : line;
                string pattern = entry.IgnoreCase ? entry.Text.ToLower() : entry.Text;

                bool foundMatch = MatchesPattern(haystack, pattern);

                if (foundMatch)
                {
                    if (entry.Remove)
                        return null;

                    if (entry.Hide)
                    {
                        item.ForeColor = Color.Transparent;
                        item.BackColor = Color.Transparent;
                        return item;
                    }

                    item.ForeColor = entry.ForeColor;
                    item.BackColor = entry.BackColor;

                    // Font styles
                    FontStyle style = FontStyle.Regular;
                    if (entry.Bold) style |= FontStyle.Bold;
                    if (entry.Italic) style |= FontStyle.Italic;
                    if (style != FontStyle.Regular)
                        item.Font = new Font(listView1.Font, style);

                    return item; // first match wins
                }
            }

            // No match found
            if (toolStripMenuItem_hiderest.Checked)
            {
                if (alsoRemoveToolStripMenuItem.Checked)
                    return null;

                item.ForeColor = Color.Transparent;
                item.BackColor = Color.Transparent;
            }

            return item;
        }

        private bool MatchesPattern(string line, string pattern)
        {
            if (pattern.Contains("&"))
            {
                var tokens = pattern.Split('&');
                return tokens.All(token => line.Contains(token));
            }
            else if (pattern.Contains("|"))
            {
                var tokens = pattern.Split('|');
                return tokens.Any(token => line.Contains(token));
            }
            else
            {
                return line.Contains(pattern);
            }
        }

        private void AddEntry()
        {
            List<string> newLines;

            // Only lock to read new lines
            lock (_serialDataLock)
            {
                if (_serialDataListCountAddedToTable >= _serialDataList.Count)
                    return;

                newLines = _serialDataList.Skip(_serialDataListCountAddedToTable).ToList();

                _serialDataListCountAddedToTable = _serialDataList.Count;
            }

            // Add items to ListView outside lock
            foreach (string line in newLines)
            {
                ListViewItem item = CreateHighlightedListItem(line);
                if (item != null)
                {
                    listView1.Items.Add(item);
                    _listviewSizeBytes += line.Length + 1; // 1 for \n
                }
            }

            // Scroll to last item if follow is enabled
            if (checkBox_follow.Checked && listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddEntry();

            if (serialcomStoppedHandleEvent)
            {
                serialcomStoppedHandleEvent = false;

                if (addStartStopTimestampToolStripMenuItem.Checked)
                {
                    var entry = new DataEntry(DateTime.Now, false, "ACQUISITION STOPPED ");
                    _logView.Add(entry);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _serialcomStopped = true;
            _serial.Close();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true; // optional, prevents further processing
                ListView.SelectedListViewItemCollection selectedItems = listView1.SelectedItems;
                String text = "";
                foreach (ListViewItem item in selectedItems)
                {
                    text += item.Text + "\n";
                }
                if (text.Length > 0)
                    Clipboard.SetText(text);
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                e.Handled = true; // optional, prevents further processing
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true; // optional, prevents further processing
                if (listView1.SelectedItems.Count == 0)
                    return;

                if (MessageBox.Show("Delete selected text? You can't get it back!", "Careful...",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int idx = 0;
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        idx = item.Index;
                        _listviewSizeBytes -= item.Text.Length + 1;
                        listView1.Items.Remove(item);
                    }
                    //listView1.Items[idx].Selected = true; // no need to select item

                    try
                    {
                        listView1.Items[idx].Focused = true;
                    }
                    catch (Exception ex)
                    { }
                }
            }
            else if (e.KeyCode == Keys.F && e.Modifiers == Keys.None)
            {
                checkBox_follow.Checked = !checkBox_follow.Checked;
                e.Handled = true; // optional, prevents further processing
            }
        }

        private void listView1_Scrolled(object sender, EventArgs e)
        {
            checkBox_follow.Checked = false;
        }

        public static Stream GenerateStreamFromListOfItems(ListView.ListViewItemCollection items)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            if (items.Count == 0)
                return null;

            // for instead of foreach so that we can control last item and not add "\n" at the end so that we 
            // do not introduce an extra item in list
            for (int i = 0; i < items.Count - 1; i++)
            {
                writer.Write(items[i].Text + '\n');
            }
            writer.Write(items[items.Count - 1].Text);

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream GenerateStreamFromListOfItems(ListView.SelectedListViewItemCollection items)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            if (items.Count == 0)
                return null;

            // for instead of foreach so that we can control last item and not add "\n" at the end so that we 
            // do not introduce an extra item in list
            for (int i = 0; i < items.Count - 1; i++)
            {
                writer.Write(items[i].Text + '\n');
            }
            writer.Write(items[items.Count - 1].Text);

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream GenerateStreamFromSerialData()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            if (_serialDataList.Count == 0)
                return null;

            // for instead of foreach so that we can control last item and not add "\n" at the end so that we 
            // do not introduce an extra item in list
            for (int i = 0; i < _serialDataList.Count - 1; i++)
            {
                writer.Write(_serialDataList[i].ToString() + '\n');
            }
            writer.Write(_serialDataList[_serialDataList.Count - 1].ToString());

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog1.FileName;
            listView1.Items.Add("Opening file: " + filePath);
            _listviewSizeBytes += filePath.Length + 1;

            string[] lines = await System.IO.File.ReadAllLinesAsync(filePath);

            var batch = new List<ListViewItem>();
            int batchSize = 100; // adjust for performance vs. responsiveness

            await ProgressForm.Helper.RunWithProgressAsync(
                this,
                "Opening file...",
                lines.Length,
                async (i) =>
                {
                    var item = CreateHighlightedListItem(lines[i]);
                    if (item != null)
                        batch.Add(item);

                    // Flush batch periodically
                    if (batch.Count >= batchSize || i == lines.Length - 1)
                    {
                        listView1.BeginUpdate();
                        listView1.Items.AddRange(batch.ToArray());
                        listView1.EndUpdate();
                        batch.Clear();
                    }

                    await Task.Yield(); // keep UI responsive
                },
                onUIThread: true
            );

            if (listView1.Items.Count > 0)
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();

            listView1.Items.Add("End of file: " + filePath);
            _listviewSizeBytes += filePath.Length + 1;
        }

        private async void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                using (Stream myStream = saveFileDialog1.OpenFile())
                using (var sourceStream = GenerateStreamFromListOfItems(listView1.Items))
                {
                    if (sourceStream == null) return;

                    const int bufferSize = 8192;
                    long totalLength = sourceStream.Length;
                    int totalSteps = (int)Math.Ceiling((double)totalLength / bufferSize);

                    await ProgressForm.Helper.RunWithProgressAsync(
                        this,
                        "Saving to file...",
                        totalSteps,
                        async (step) =>
                        {
                            byte[] buffer = new byte[bufferSize];
                            int bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                                await myStream.WriteAsync(buffer, 0, bytesRead);
                        },
                        onUIThread: false
                    );
                }
            }
        }

        private async void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select lines to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                using (Stream myStream = saveFileDialog1.OpenFile())
                using (var sourceStream = GenerateStreamFromListOfItems(listView1.SelectedItems))
                {
                    if (sourceStream == null) return;

                    const int bufferSize = 8192;
                    long totalLength = sourceStream.Length;
                    int totalSteps = (int)Math.Ceiling((double)totalLength / bufferSize);

                    await ProgressForm.Helper.RunWithProgressAsync(
                        this,
                        "Saving selected items...",
                        totalSteps,
                        async (step) =>
                        {
                            byte[] buffer = new byte[bufferSize];
                            int bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                                await myStream.WriteAsync(buffer, 0, bytesRead);
                        },
                        onUIThread: false
                    );
                }
            }
        }

        private async void saveSerialAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

                using (Stream myStream = saveFileDialog1.OpenFile())
                {
                    var serialStream = GenerateStreamFromSerialData();
                    if (serialStream == null) return;

                    const int bufferSize = 8192;
                    long totalLength = serialStream.Length;
                    int totalSteps = (int)Math.Ceiling((double)totalLength / bufferSize);

                    await ProgressForm.Helper.RunWithProgressAsync(
                        this,
                        "Saving Serial data...",
                        totalSteps,
                        async (step) =>
                        {
                            byte[] buffer = new byte[bufferSize];
                            int bytesRead = await serialStream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead > 0)
                                await myStream.WriteAsync(buffer, 0, bytesRead);
                        },
                        onUIThread: false
                    );
                }
            }
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

        private void button_findall_Click(object sender, EventArgs e)
        {
            string text = textBox_find.Text;

            FindAllString(text);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear shared data safely
            lock (_serialDataLock)
            {
                listView1.Items.Clear();
                _logView.Clear();
            }

            runTime = new Stopwatch();
            if (!_serialcomStopped)
                runTime.Start();
        }

        private async void ReloadAllListViewItems()
        {
            if (listView1.Items.Count == 0) return;

            await ProgressForm.Helper.RunWithProgressAsync(
                this,
                "Reloading all items...",
                listView1.Items.Count,
                async (i) =>
                {
                    var item = CreateHighlightedListItem(listView1.Items[i].Text);
                    if (item == null)
                    {
                        listView1.Items[i].Remove();
                    }
                    else
                    {
                        listView1.Items[i] = item;
                    }
                    await Task.Yield(); // keeps async flow smooth
                },
                onUIThread: true
            );
        }

        private async void ReloadSelectedListViewItems(ListView.SelectedListViewItemCollection selectedItems)
        {
            if (selectedItems.Count == 0) return;

            var itemsToReload = selectedItems.Cast<ListViewItem>().ToList();

            await ProgressForm.Helper.RunWithProgressAsync(
                this,
                "Reloading selected items...",
                itemsToReload.Count,
                async (i) =>
                {
                    var oldItem = itemsToReload[i];
                    var newItem = CreateHighlightedListItem(oldItem.Text);

                    if (newItem == null)
                    {
                        oldItem.Remove();
                    }
                    else
                    {
                        listView1.Items[oldItem.Index] = newItem;
                    }
                    await Task.Yield();
                },
                onUIThread: true
            );
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                ReloadAllListViewItems();
            else
                ReloadSelectedListViewItems(listView1.SelectedItems);
        }

        string NumberToBKBMB(double num, string suffix = "")
        {
            string str;

            if (num > 1024.0 * 1024.0)
            {
                num /= 1024.0 * 1024.0;
                str = num.ToString("0.00") + " MB" + suffix;
            }
            else if (num > 1024.0)
            {
                num /= 1024.0;
                str = num.ToString("0.00") + " KB" + suffix;
            }
            else
            {
                str = Convert.ToInt32(num).ToString() + " B" + suffix;
            }

            return str;
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


            string serialSizeStr = NumberToBKBMB(_serialDataListSizeBytes);

            double serialBytesPerSec = (double)(_serialDataListSizeBytes - _prevSerialDataListSizeBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            _prevSerialDataListSizeBytes = _serialDataListSizeBytes;
            string serialSpeedStr = NumberToBKBMB(serialBytesPerSec, "/s");
            double avgSerialBytesPerSec = _serialDataListSizeBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
            string avgSerialSpeedStr = NumberToBKBMB(avgSerialBytesPerSec, "/s");

            // So that saved file size will be the same as the one in the label subtract one byte (last newline)                       
            double listSize = _listviewSizeBytes > 0 ? _listviewSizeBytes - 1 : 0;
            string listSizeStr = NumberToBKBMB(listSize);

            double listBytesPerSec = (double)(_listviewSizeBytes - _prevListviewSizeBytes) / ((double)timer_updatesysinfo.Interval / 1000.0);
            _prevListviewSizeBytes = _listviewSizeBytes;
            string listSpeedStr = NumberToBKBMB(listBytesPerSec, "/s");
            double avgListBytesPerSec = _listviewSizeBytes / (run.TotalSeconds > 0 ? run.TotalSeconds : 1);
            string avgListSpeedStr = NumberToBKBMB(avgListBytesPerSec, "/s");

            int bytesToRead = 0;
            if (_serial.IsOpen)
                bytesToRead = _serial.BytesToRead;

            string availableBytesStr = NumberToBKBMB(bytesToRead);

            this.Text = "Serialog |" +
                "   Serial: " + serialSizeStr + " @ " + serialSpeedStr + " (avg. " + avgSerialSpeedStr + ")" +
                "   List: " + listSizeStr + " @ " + listSpeedStr + " (avg. " + avgListSpeedStr + ")" +
                "   Available: " + availableBytesStr +
                "   Alive: " + ups +
                "   Running: " + runs;
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
                listView1.Font.Dispose();
                listView1.Font = fontDialog1.Font;

                if (MessageBox.Show("Reload highlight settings?", "Reload?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ReloadAllListViewItems();
                }
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

        private void toolStripMenuItem_hiderest_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_hiderest.Checked = !toolStripMenuItem_hiderest.Checked;
            hideHighlightedToolStripMenuItem.Checked = toolStripMenuItem_hiderest.Checked;
            if (!toolStripMenuItem_hiderest.Checked)
            {
                alsoRemoveToolStripMenuItem.Checked = false;
                alsoRemoveToolStripMenuItem1.Checked = false;
            }

        }

        private void alsoRemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alsoRemoveToolStripMenuItem.Checked = !alsoRemoveToolStripMenuItem.Checked;
            alsoRemoveToolStripMenuItem1.Checked = alsoRemoveToolStripMenuItem.Checked;
            if (alsoRemoveToolStripMenuItem.Checked)
            {
                toolStripMenuItem_hiderest.Checked = true;
                hideHighlightedToolStripMenuItem.Checked = true;
            }
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            selectAllToolStripMenuItem_Click(sender, e);
        }

        private void clearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clearAllToolStripMenuItem_Click(sender, e);
        }

        private void reloadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            reloadToolStripMenuItem_Click(sender, e);
        }

        private void highlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            highlightsToolStripMenuItem_Click(sender, e);
        }

        private void hideHighlightedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMenuItem_hiderest_Click(sender, e);
        }

        private void alsoRemoveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            alsoRemoveToolStripMenuItem_Click(sender, e);
        }

        // This one is the menustrip click function
        private void disableHighlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            disableHighlightsToolStripMenuItem.Checked = !disableHighlightsToolStripMenuItem.Checked;
            disableHighlightsToolStripMenuItem1.Checked = disableHighlightsToolStripMenuItem.Checked;
            if (!disableHighlightsToolStripMenuItem.Checked)
            {
                disableHighlightsToolStripMenuItem.Checked = false;
                disableHighlightsToolStripMenuItem1.Checked = false;
            }
        }

        // This one is the context click function
        private void disableHighlightsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            disableHighlightsToolStripMenuItem_Click(sender, e);
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
                ListViewItem item = new ListViewItem(input);
                var ret = listView1.Items.Insert(addRowAtIndex, item);

                // Ensure listview is selected
                listView1.Select();

                // Select added item
                listView1.SelectedIndices.Clear();
                ret.Selected = true;
                ret.EnsureVisible();
            }
        }

        private void addCustomRowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addCustomRowToolStripMenuItem_Click(sender, e);
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

        private void highlightsToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void highlightsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (form3Highlights == null || form3Highlights.IsDisposed)
            {
                form3Highlights = new Form3_Highlights();
                RegisterChild(form3Highlights);
                form3Highlights.Show();
            }
            else
            {
                form3Highlights.Show();   // unhide if hidden
                form3Highlights.Focus();
            }
        }

        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form4Send == null || form4Send.IsDisposed)
            {
                form4Send = new Form4_Send(this, _serial, _dataLog, _logView);
                RegisterChild(form4Send);
                form4Send.Show();
            }
            else
            {
                form4Send.Show();   // unhide if hidden
                form4Send.Focus();
            }
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppSettings.SettingsFolder = AppSettings.DefaultSettingsFolder;
        }
    }
}
