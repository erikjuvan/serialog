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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void highlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2_Highlight form2_Highlight = new Form2_Highlight();
            form2_Highlight.ShowDialog();
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
            foreach (HighlightEntry highlightEntry in Form2_Highlight.highlightSettings.highlightEntries)
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
            foreach (HighlightEntry highlightEntry in Form2_Highlight.highlightSettings.highlightEntries)
            {
                if (line.Contains(highlightEntry.text))
                {
                    return highlightEntry.backColor;
                }
            }

            return Color.White;
        }

        private void AddEntry()
        {
            mtx.WaitOne();
            if (_serialDataListCountAddedToTable < _serialDataList.Count)
            {
                for (int i = _serialDataListCountAddedToTable; i < _serialDataList.Count; i++)
                {
                    var entry = _serialDataList[i];
                    listView1.Items.Add(entry);
                    listView1.Items[listView1.Items.Count - 1].ForeColor = GetForeColor(entry);
                    listView1.Items[listView1.Items.Count - 1].BackColor = GetBackColor(entry);
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

        public static Stream GenerateStreamFromListOfStrings(List<string> s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (string item in s)
                writer.Write(item + "\n");
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
                    using (var stream = GenerateStreamFromListOfStrings(_serialDataList))
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
                        listView1.Items.Add(line);
                        listView1.Items[listView1.Items.Count - 1].ForeColor = GetForeColor(line);
                        listView1.Items[listView1.Items.Count - 1].BackColor = GetBackColor(line);
                    }

                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                }

                fileStream.Close();
            }
        }

        public static Stream GenerateStreamFromListOfItems(ListView.SelectedListViewItemCollection selectedListViewItemCollection)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (ListViewItem item in selectedListViewItemCollection)
            {
                writer.Write(item.Text + '\n');
            }                
            writer.Flush();
            stream.Position = 0;
            return stream;
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
    }
}