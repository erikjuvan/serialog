using System.Text.RegularExpressions;

namespace serialog
{
    public partial class Form4_Send : Form
    {
        private Form1 _parentForm;
        private SerialCom _serialPort;

        internal Form4_Send(Form1 parent, SerialCom serialPort)
        {
            InitializeComponent();

            _parentForm = parent;
            _serialPort = serialPort;
        }

        private void Form5_Send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void button1_add_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(input))
                return;

            // Remove all non-hex characters (except valid hex digits)
            string cleaned = new string(input.Where(c => Uri.IsHexDigit(c)).ToArray());

            if (cleaned.Length == 0)
            {
                MessageBox.Show("Input must contain at least one hex digit.");
                return;
            }

            // Split into byte pairs, pad single dangling digit with '0' in front
            var bytes = new List<string>();
            for (int i = 0; i < cleaned.Length; i += 2)
            {
                string byteStr;
                if (i + 1 < cleaned.Length)
                    byteStr = cleaned.Substring(i, 2);
                else
                    byteStr = "0" + cleaned[i]; // pad dangling digit

                bytes.Add(byteStr.ToUpper());
            }

            string formatted = string.Join(" ", bytes);

            // Add to ListView only if not empty
            if (!string.IsNullOrWhiteSpace(formatted))
            {
                listView1.Items.Add(formatted);
            }

            textBox1.Focus();
        }

        private bool CanSendData()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a line to send.");
                return false;
            }

            if (_serialPort == null)
            {
                MessageBox.Show("Serial port is null!");
                return false;
            }

            if (!_serialPort.IsOpen())
            {
                MessageBox.Show("Serial port is closed!");
                return false;
            }

            return true;
        }

        private void SendSelectedLine()
        {
            foreach (ListViewItem selectedItem in listView1.SelectedItems)
            {
                string line = selectedItem.Text;

                // Convert to byte array
                byte[] bytes = line.Split(' ')
                                   .Select(b => Convert.ToByte(b, 16))
                                   .ToArray();

                _serialPort.Write(bytes, 0, bytes.Length);

                // Add to parent form's listview as hex string (space separated)
                string hexString = string.Join(" ", bytes.Select(b => b.ToString("X2")));
                _parentForm.listView1.Items.Add(hexString);
            }
        }
        private void button2_send_Click(object sender, EventArgs e)
        {
            if (CanSendData())
            {
                SendSelectedLine();
            }
        }

        private void button_send_every_Click(object sender, EventArgs e)
        {
            if (!CanSendData())
            {
                return;
            }

            timer_send_every_ms.Enabled = !timer_send_every_ms.Enabled;

            if (timer_send_every_ms.Enabled)
            {
                if (int.TryParse(textBox_send_every.Text, out int interval))
                {
                    timer_send_every_ms.Interval = interval;
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for interval (ms).");
                }

                button_send_every.BackColor = Color.Green;
            }
            else
            {
                button_send_every.BackColor = Button.DefaultBackColor;
            }
        }

        private void timer_send_every_ms_Tick(object sender, EventArgs e)
        {
            SendSelectedLine();
        }

        private void textBoxHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Uri.IsHexDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // ignore invalid input
            }
        }

        private void textBox_send_every_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // ignore the key press
            }
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
                // Remove all selected items
                while (listView1.SelectedItems.Count > 0)
                {
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                }
            }
        }

        private void Populate_comboBox_file()
        {
            comboBox_file.Items.Clear();

            try
            {
                var listOfFiles = Directory.EnumerateFiles(".settings", "*.send", SearchOption.AllDirectories)
                                           .Select(Path.GetFileNameWithoutExtension);

                foreach (var file in listOfFiles)
                {
                    comboBox_file.Items.Add(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_file_DropDown(object sender, EventArgs e)
        {
            Populate_comboBox_file();
        }

        private async void button_file_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_file.Text))
                return;

            string filename = comboBox_file.Text;
            string fullpath = Path.Combine(".settings", filename + ".send");
            bool write = true;

            Directory.CreateDirectory(".settings");

            if (File.Exists(fullpath))
            {
                var ret = MessageBox.Show($"File '{filename}' already exists, overwrite it?", "Overwrite?",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ret == DialogResult.No)
                    write = false;
            }

            if (write)
            {
                var lines = listView1.Items.Cast<ListViewItem>().Select(i => i.Text).ToList();
                await File.WriteAllLinesAsync(fullpath, lines);
            }
        }

        private void button_file_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_file.Text))
                return;

            string filename = Path.Combine(".settings", comboBox_file.Text + ".send");

            if (!File.Exists(filename))
            {
                MessageBox.Show($"File '{comboBox_file.Text}' doesn't exist!", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            listView1.BeginUpdate();

            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                listView1.Items.Add(line);
            }

            listView1.EndUpdate();
        }

        private void button_file_delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_file.Text))
                return;

            string fullpath = Path.Combine(".settings", comboBox_file.Text + ".send");

            try
            {
                if (File.Exists(fullpath))
                    File.Delete(fullpath);

                comboBox_file.Text = "";
                Populate_comboBox_file();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete '{comboBox_file.Text}'!\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
