namespace serialog
{
    public partial class Form1 : Form
    {
        private static bool _serialcomPaused = new bool();
        private static bool _serialcomStopped = new bool();
        private static SerialCom _serialCom = new SerialCom();
        private static List<string> _serialDataList = new List<string>();
        private int _serialDataListCountAddedToTable = 0;
        private static Mutex mtx = new Mutex();
        private Thread serialReadThread = null;

        private Form2_Highlight form2Highlight = null;

        private string[] GetSortedPorts()
        {
            var port_names = SerialCom.GetPortNames();
            try
            {
                Array.Sort(port_names, (x, y) => Int32.Parse(x.Substring(3, x.Length - 3)) >
            Int32.Parse(y.Substring(3, y.Length - 3)) ? 1 : -1);
            }            
            catch (Exception ex) {}

            return port_names;
        }

        public Form1()
        {
            InitializeComponent();

            comboBox_port.Items.AddRange(GetSortedPorts());

            if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0;

            comboBox_baud.SelectedItem = "1000000";

            _serialcomStopped = true;
            _serialcomPaused = true;

            Form2_Highlight.InitializeHighlightSettings();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                textBox_find.Select();
            }

            if (textBox_find.Focused)
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

            if (e.Shift && e.KeyCode == Keys.F3)
            {
                FindPrevString(textBox_find.Text);
            }
            else if (e.KeyCode == Keys.F3)
            {
                FindNextString(textBox_find.Text);
            }
            
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
                form2Highlight.Left = Form1.MousePosition.X;
                form2Highlight.Top = Form1.MousePosition.Y;
                form2Highlight.Show();
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

                _serialCom.Open(comboBox_port.GetItemText(comboBox_port.SelectedItem), baud);

                _serialcomPaused = false;
                _serialcomStopped = false;

                comboBox_port.Enabled = false;
                comboBox_baud.Enabled = false;

                serialReadThread = new Thread(SerialRead);
                serialReadThread.Start();
            }
            else if (_serialcomPaused)
            {
                _serialcomPaused = false;
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            _serialcomPaused = true;
        }
        private void button_stop_Click(object sender, EventArgs e)
        {
            _serialcomStopped = true;
            serialReadThread.Join();

            _serialCom.Close();

            comboBox_port.Enabled = true;
            comboBox_baud.Enabled = true;
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
                if (!_serialcomPaused)
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

                    //Thread.Sleep(100);
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
                        item.Text = "";                        
                    }
                    else
                    {
                        // foreColor
                        item.ForeColor = highlightEntry.foreColor;
                        // backColor
                        item.BackColor = highlightEntry.backColor;

                        // Font (bold, italic)
                        if (highlightEntry.bold && highlightEntry.italic)
                            item.Font = new Font("Courier New", 10, FontStyle.Bold | FontStyle.Italic);
                        else if (highlightEntry.bold)
                            item.Font = new Font("Courier New", 10, FontStyle.Bold);
                        else if (highlightEntry.italic)
                            item.Font = new Font("Courier New", 10, FontStyle.Italic);
                    }

                    // This break implements that the higher entries have priority since it breaks as soon as it finds first match
                    break;
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

                    listView1.Items.Add(CreateHighlightedListItem(line));
                }
                _serialDataListCountAddedToTable = _serialDataList.Count;

                if (checkBox1.Checked)
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
            mtx.ReleaseMutex();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddEntry();
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
                Clipboard.SetText(text);
            }
            else if (e.Control && e.KeyCode == Keys.W)
            {
                listView1.Items.Clear();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }                
            }
        }

        private void listView1_Scrolled(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }

        public static Stream GenerateStreamFromListOfItems(ListView.ListViewItemCollection items)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (ListViewItem item in items)
            {
                writer.Write(item.Text + '\n');
            }
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream GenerateStreamFromListOfItems(ListView.SelectedListViewItemCollection items)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (ListViewItem item in items)
            {
                writer.Write(item.Text + '\n');
            }
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
                    // Code to write the stream goes here.
                    using (var stream = GenerateStreamFromListOfItems(listView1.Items))
                    {
                        stream.CopyTo(myStream);
                    }

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

                //Read the contents of the file into a stream
                var fileStream = openFileDialog1.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                    var list_of_lines = fileContent.Split('\n').ToList();
                    foreach (var line in list_of_lines)
                    {
                        listView1.Items.Add(CreateHighlightedListItem(line));
                    }

                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }

                fileStream.Close();
            }
        }

        private void saveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select lines to save.");
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
                    // Code to write the stream goes here.
                    using (var stream = GenerateStreamFromListOfItems(listView1.SelectedItems))
                    {
                        stream.CopyTo(myStream);
                    }

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

            for (int i = listView1.Items.Count-1; i >= 0; i--)
            {
                if (listView1.Items[i].Text.Contains(text))
                {
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
                listView1.Items[listView1.SelectedIndices[listView1.SelectedIndices.Count-1]].EnsureVisible();
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

        private void toolStripMenuItem_hiderest_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem_hiderest.Checked)
            {
                // Hide non matching entries
            }
            else
            {
                // Show non matching entries
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
            listView1.Focus();
        }

        private void clearAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}