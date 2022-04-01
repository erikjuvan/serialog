namespace serialog
{
    public partial class Form1 : Form
    {
        static bool _serialcomPaused = new bool();
        static bool _serialcomStopped = new bool();
        static SerialCom _serialCom = new SerialCom();
        static string _serialData = new string("");
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
                bool success = int.TryParse(textBox_baud.Text, out baud);

                if (!success)
                    return;

                _serialCom.Open(comboBox_port.GetItemText(comboBox_port.SelectedItem), baud);

                _serialcomPaused = false;
                _serialcomStopped = false;

                comboBox_port.Enabled = false;
                textBox_baud.Enabled = false;

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
            textBox_baud.Enabled = true;
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

            MessageBox.Show(comboBox_port.Items.Count.ToString());
        }

        private static void SerialRead()
        {
            string[] str_arr = { "Hello\n", "bye\n", "Timeout\n", "fsf\n", "turtla\n", "abcaivoasmnoivmsoivmsoavmos\n", "BABBYBY!!!!!!!!\n" };
            int i = 0;
            while (!_serialcomStopped)
            {
                if (!_serialcomPaused)
                {
                    mtx.WaitOne();
                    try
                    {
                        byte[] by = new byte[100];
                        //_serialData += _serialCom.ReadExisting();
                        _serialCom.Read(by, 0, 100);
                    }
                    catch (TimeoutException) 
                    {
                        _serialData += str_arr[i++ % 7];
                    }
                    mtx.ReleaseMutex();

                    Thread.Sleep(100);
                }
            }            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_serialData.Length > 0)
            {
                mtx.WaitOne();
                string str = _serialData;
                _serialData = string.Empty;
                mtx.ReleaseMutex();

                string[] lines = str.Split(
                    new string[] { "\n" },
                    StringSplitOptions.RemoveEmptyEntries
                );

                foreach (string line in lines)
                {
                    if (line == "bye")
                    {
                        /*richTextBox1.DeselectAll();
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.AppendText(line + "\n");*/

                        listView1.Items.Add(line);
                        listView1.Items[listView1.Items.Count - 1].ForeColor = Color.Red;
                    }
                    else 
                    {
                        /*richTextBox1.DeselectAll();
                        richTextBox1.AppendText(line + "\n");*/
                        listView1.Items.Add(line);
                    }

                    if (checkBox1.Checked)
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();
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