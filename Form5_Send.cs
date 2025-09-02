using System.Text.RegularExpressions;

namespace serialog
{
    public partial class Form5_Send : Form
    {
        private Form1 _parentForm;
        private SerialCom _serialPort;

        internal Form5_Send(Form1 parent, SerialCom serialPort)
        {
            InitializeComponent();

            _parentForm = parent;
            _serialPort = serialPort;
        }

        private void button2_send_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a line to send.");
                return;
            }

            if (_serialPort == null)
            {
                MessageBox.Show("Serial port is null!");
                return;
            }

            if (!_serialPort.IsOpen())
            {
                MessageBox.Show("Serial port is closed!");
                return;
            }

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

        private void textBoxHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Uri.IsHexDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // ignore invalid input
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

        private void comboBox_file_DropDown(object sender, EventArgs e)
        {
            comboBox_file.Items.Clear();

            // list of files without the path, just file name
            try
            {
                var listOfFiles = Directory.EnumerateFiles(".settings", "*", SearchOption.AllDirectories).Select(Path.GetFileName);
                var listOfSendFiles= listOfFiles.Where(text => text.Contains(".send"));
                foreach (var sendfile in listOfSendFiles)
                {
                    int to = sendfile.IndexOf(".");

                    var result = sendfile.Substring(0, to);

                    comboBox_file.Items.Add(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_file_save_Click(object sender, EventArgs e)
        {
            if (comboBox_file.Text.Length == 0)
                return;

            string filename = comboBox_file.Text;
            string fullpath = ".settings/" + filename + ".send";
            bool write = true;

            System.IO.Directory.CreateDirectory(".settings");

            if (System.IO.File.Exists(fullpath))
            {
                var ret = MessageBox.Show("File '" + comboBox_file.Text + "' already exists, overwrite it?", "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ret == DialogResult.No)
                    write = false;
            }

            if (write)
            {
                List<string> items = new List<string>();

                foreach (ListViewItem item in listView1.Items)
                {
                    items.Add(item.Text);
                }

                await File.WriteAllLinesAsync(fullpath, items);
            }
        }

        private void button_file_load_Click(object sender, EventArgs e)
        {
            if (comboBox_file.Text.Length == 0)
                return;

            string filename = ".settings/" + comboBox_file.Text + ".send";

            if (!System.IO.File.Exists(filename))
            {
                MessageBox.Show("File '" + comboBox_file.Text + "' doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse file
            var lines = File.ReadAllLines(filename);

            foreach (var line in lines)
            {
                var items = line.Split(",");

                listView1.Items.Add(line);
            }
        }

        private void button_file_delete_Click(object sender, EventArgs e)
        {
            string filename = comboBox_file.Text;
            string fullpath = ".settings/" + filename + ".send";

            try
            {
                File.Delete(fullpath);
                comboBox_file.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete '" + comboBox_file.Text + "'!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form5_Send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }
    }
}
