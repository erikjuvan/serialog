namespace serialog
{
    public partial class Form1 : Form
    {
        static bool _serialcomPaused = new bool();
        static bool _serialcomStopped = new bool();
        static SerialCom _serialCom = new SerialCom();
        static List<string> _serialDataList = new List<string>();
        int _serialDataListCountAddedToTable = 0;
        private static Mutex mtx = new Mutex();

        Thread serialReadThread = null;        

        public Form1()
        {
            InitializeComponent();

            var port_names = SerialCom.GetPortNames();
            foreach (var item in port_names)
            {
                comboBox_port.Items.Add(item);
            }
            if (comboBox_port.Items.Count > 0)
                comboBox_port.SelectedIndex = 0;

            comboBox_baud.SelectedItem = "1000000";

            _serialcomStopped = true;
            _serialcomPaused = true;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void highlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2_Highlighting form2_Highlighting = new Form2_Highlighting();
            form2_Highlighting.ShowDialog();
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
            var port_names = SerialCom.GetPortNames();
            foreach (var item in port_names)
            {
                comboBox_port.Items.Add(item);
            }
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
            if (line.Contains("RX"))
            {
                return Color.Red;
            }
            else if (line.Contains("TX"))
            {
                return Color.Violet;
            }
            else
            {
                return Color.Black;
            }            
        }

        private Color GetBackColor(string line)
        {
            if (line.Contains("DOC"))
            {
                return Color.AliceBlue;
            }
            else
            {
                return Color.White;
            }            
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

            if (e.Control && e.KeyCode == Keys.W)
            {
                listView1.Items.Clear();
            }
        }

        private void listView1_Scrolled(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
        }
        
    }
}