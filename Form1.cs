using System.Diagnostics;

namespace serialog
{
    public partial class Form1 : Form
    {
        private static bool _serialcomStopped = new bool();
        private bool serialcomStoppedHandleEvent = new bool();
        private static SerialCom _serialCom = new SerialCom();
        private static List<string> _serialDataList = new List<string>();
        private static int _listviewSizeBytes = 0;
        private int _serialDataListCountAddedToTable = 0;
        private static Mutex mtx = new Mutex();
        private Thread serialReadThread = null;

        private Stopwatch runTime = new Stopwatch();
        private Stopwatch upTime = new Stopwatch();

        private Form2_Highlight form2Highlight = null;

        public Form1()
        {
            InitializeComponent();

            comboBox_port.Items.AddRange(GetSortedPorts());

            if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0;

            comboBox_baud.SelectedItem = "1000000";

            _serialcomStopped = true;
            button_stop.Enabled = false;

            Form2_Highlight.InitializeHighlightSettings();

            upTime.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                textBox_find.Select();
            }

            if (e.Shift && e.KeyCode == Keys.F3)
            {
                FindPrevString(textBox_find.Text);
            }
            else if (e.KeyCode == Keys.F3)
            {
                FindNextString(textBox_find.Text);
            }

        }

        private string[] GetSortedPorts()
        {
            var port_names = SerialCom.GetPortNames();
            try
            {
                Array.Sort(port_names, (x, y) => Int32.Parse(x.Substring(3, x.Length - 3)) >
            Int32.Parse(y.Substring(3, y.Length - 3)) ? 1 : -1);
            }
            catch (Exception ex) { }

            return port_names;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void form2Highlight_Closed(object sender, FormClosedEventArgs e)
        {
            form2Highlight.FormClosed -= form2Highlight_Closed;
            form2Highlight = null;
        }

        private void highlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (form2Highlight == null)
            {
                form2Highlight = new Form2_Highlight();
                form2Highlight.FormClosed += form2Highlight_Closed;
                form2Highlight.StartPosition = FormStartPosition.Manual;
                form2Highlight.Left = this.Location.X + this.Width - form2Highlight.Width;
                form2Highlight.Top = this.Location.Y + 100;
                form2Highlight.Show();
            }
            else
            {
                form2Highlight.Focus();
            }
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
                    _serialCom.Open(comboBox_port.Text, baud);
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

                if (addTimestampsToolStripMenuItem.Checked)
                {
                    string dateTimeString = "ACQUISITION STARTED " + DateTime.Now.ToString("dddd dd/MM/yyyy hh:mm:ss");
                    listView1.Items.Add(dateTimeString);
                    _listviewSizeBytes += dateTimeString.Length + 1;
                }

                serialReadThread = new Thread(SerialRead);
                serialReadThread.Start();

                runTime = Stopwatch.StartNew();
                runTime.Start();
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (!_serialcomStopped)
            {
                _serialcomStopped = true;
                serialReadThread.Join();

                _serialCom.Close();

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

        private static void SerialRead()
        {
            while (!_serialcomStopped)
            {
                var line = "";

                try
                {
                    line = _serialCom.ReadLine();
                }
                catch (TimeoutException)
                {
                }

                if (line.Length > 0)
                {
                    mtx.WaitOne();
                    _serialDataList.Add(line);
                    mtx.ReleaseMutex();
                }
            }
        }

        private Color GetForeColor(string line)
        {
            foreach (HighlightEntry highlightEntry in Form2_Highlight.highlightEntries.Items)
            {
                if (line.Contains(highlightEntry.text))
                {
                    return highlightEntry.foreColor;
                }
            }

            return Color.Black;
        }

        private Color GetBackColor(string line)
        {
            foreach (HighlightEntry highlightEntry in Form2_Highlight.highlightEntries.Items)
            {
                if (line.Contains(highlightEntry.text))
                {
                    return highlightEntry.backColor;
                }
            }

            return Color.White;
        }

        private ListViewItem CreateHighlightedListItem(string line)
        {
            ListViewItem item = new ListViewItem(line);

            foreach (HighlightEntry highlightEntry in Form2_Highlight.highlightEntries.Items)
            {
                // text
                string text = highlightEntry.text;

                // ignoreCase
                if (highlightEntry.ignoreCase)
                {
                    line = line.ToLower();
                    text = text.ToLower();
                }

                if (line.Contains(text))
                {
                    // hide
                    if (highlightEntry.hide)
                    {
                        item.ForeColor = Color.Transparent;
                        item.BackColor = Color.Transparent;
                    }
                    else
                    {
                        // foreColor
                        item.ForeColor = highlightEntry.foreColor;
                        // backColor
                        item.BackColor = highlightEntry.backColor;

                        // Font (bold, italic)
                        if (highlightEntry.bold && highlightEntry.italic)
                            item.Font = new Font(listView1.Font, FontStyle.Bold | FontStyle.Italic);
                        else if (highlightEntry.bold)
                            item.Font = new Font(listView1.Font, FontStyle.Bold);
                        else if (highlightEntry.italic)
                            item.Font = new Font(listView1.Font, FontStyle.Italic);
                    }

                    // This break implements that the higher entries have priority since it breaks as soon as it finds first match
                    break;
                }
                else
                {
                    if (toolStripMenuItem_hiderest.Checked)
                    {
                        item.ForeColor = Color.Transparent;
                        item.BackColor = Color.Transparent;
                    }
                }
            }

            return item;
        }

        private void AddEntry()
        {
            mtx.WaitOne();
            if (_serialDataListCountAddedToTable < _serialDataList.Count)
            {
                for (int i = _serialDataListCountAddedToTable; i < _serialDataList.Count; i++)
                {
                    string line = _serialDataList[i];
                    _listviewSizeBytes += line.Length + 1; // 1 for \n

                    listView1.Items.Add(CreateHighlightedListItem(line));
                }
                _serialDataListCountAddedToTable = _serialDataList.Count;

                if (checkBox_follow.Checked)
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
            mtx.ReleaseMutex();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddEntry();

            if (serialcomStoppedHandleEvent)
            {
                serialcomStoppedHandleEvent = false;

                if (addTimestampsToolStripMenuItem.Checked)
                {
                    string dateTimeString = "ACQUISITION STOPPED " + DateTime.Now.ToString("dddd dd/MM/yyyy hh:mm:ss");
                    listView1.Items.Add(dateTimeString);
                    _listviewSizeBytes += dateTimeString.Length + 1;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _serialcomStopped = true;
            if (serialReadThread != null)
                serialReadThread.Join();
            _serialCom.Close();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
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
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    Form3_progressbar form3 = new Form3_progressbar("Saving to file...");
                    form3.StartPosition = FormStartPosition.Manual;
                    form3.Left = this.Location.X + this.Width / 2 - form3.Width / 2;
                    form3.Top = this.Location.Y + this.Height / 2 - form3.Height / 2;
                    form3.Show();
                    form3.ProgressBarSetup(1, 1);
                    Thread.Sleep(200);
                    // Code to write the stream goes here.
                    using (var stream = GenerateStreamFromListOfItems(listView1.Items))
                    {
                        if (stream != null)
                            stream.CopyTo(myStream);
                    }

                    form3.ProgressBarIncrement();
                    Thread.Sleep(300);
                    form3.Close();

                    myStream.Close();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;

                string openFileInfo = "Opening file: " + filePath;
                listView1.Items.Add(openFileInfo);
                _listviewSizeBytes += openFileInfo.Length + 1;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog1.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    Form3_progressbar form3 = new Form3_progressbar("Opening file...");
                    form3.StartPosition = FormStartPosition.Manual;
                    form3.Left = this.Location.X + this.Width / 2 - form3.Width / 2;
                    form3.Top = this.Location.Y + this.Height / 2 - form3.Height / 2;
                    form3.Show();
                    fileContent = reader.ReadToEnd();
                    var listOfLines = fileContent.Split('\n').ToList();
                    form3.ProgressBarSetup(listOfLines.Count, 1);
                    for (int i = 0, size = listOfLines.Count; i < size; i++)
                    {
                        var line = listOfLines[i];
                        _listviewSizeBytes += line.Length + 1;
                        listView1.Items.Add(CreateHighlightedListItem(line));
                        form3.ProgressBarIncrement();
                    }

                    form3.Close();

                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }

                fileStream.Close();

                string endOfFileInfo = "End of file: " + filePath;
                listView1.Items.Add(endOfFileInfo);
                _listviewSizeBytes += endOfFileInfo.Length + 1;
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select lines to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    Form3_progressbar form3 = new Form3_progressbar("Saving to file...");
                    form3.StartPosition = FormStartPosition.Manual;
                    form3.Left = this.Location.X + this.Width / 2 - form3.Width / 2;
                    form3.Top = this.Location.Y + this.Height / 2 - form3.Height / 2;
                    form3.Show();
                    form3.ProgressBarSetup(1, 1);
                    Thread.Sleep(200);
                    // Code to write the stream goes here.
                    using (var stream = GenerateStreamFromListOfItems(listView1.SelectedItems))
                    {
                        if (stream != null)
                            stream.CopyTo(myStream);
                    }

                    form3.ProgressBarIncrement();
                    Thread.Sleep(300);
                    form3.Close();

                    myStream.Close();
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
                if (listView1.Items[i].Text.Contains(text))
                {
                    //listView1.Select();
                    checkBox_follow.Checked = false;
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    break;
                }
            }
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
                if (listView1.Items[i].Text.Contains(text))
                {
                    checkBox_follow.Checked = false;
                    //listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    break;
                }
            }
        }

        private void FindFirstString(string text)
        {
            if (text == "")
                return;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.Contains(text))
                {
                    checkBox_follow.Checked = false;
                    //listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    break;
                }
            }
        }

        private void FindLastString(string text)
        {
            if (text == "")
                return;

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                if (listView1.Items[i].Text.Contains(text))
                {
                    checkBox_follow.Checked = false;
                    //listView1.Select();
                    listView1.SelectedItems.Clear();
                    listView1.Items[i].Selected = true;
                    listView1.Items[i].EnsureVisible();
                    break;
                }
            }
        }

        private void FindAllString(string text)
        {
            if (text == "")
                return;

            bool foundText = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text.Contains(text))
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

        private void clearAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            _listviewSizeBytes = 0;
        }

        private void ReloadListViewItems()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i] = CreateHighlightedListItem(listView1.Items[i].Text);
                //listView1.Items.Insert(item.Index + 1, CreateHighlightedListItem(item.Text));
                //listView1.Items.Remove(item);
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadListViewItems();
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

            // So that saved file size will be the same as the one in the label subtract one byte (last newline)
            double size = _listviewSizeBytes > 0 ? _listviewSizeBytes - 1 : 0;
            string sizeStr;
            if (size > 1024.0 * 1024.0)
            {
                size /= 1024.0 * 1024.0;
                sizeStr = size.ToString("0.00") + " MB";
            }
            else if (size > 1024.0)
            {
                size /= 1024.0;
                sizeStr = size.ToString("0.00") + " KB";
            }
            else
            {
                sizeStr = Convert.ToInt32(size).ToString() + " B";
            }

            label_processinfo.Text = "Alive: " + ups +
                " Running: " + runs + " Data: " + sizeStr;
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
                    ReloadListViewItems();
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
    }
}