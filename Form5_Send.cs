using System.Text.RegularExpressions;

namespace serialog
{
    public partial class Form5_Send : Form
    {
        private SerialCom _serialPort;

        internal Form5_Send(SerialCom serialPort)
        {
            InitializeComponent();
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

            string line = listView1.SelectedItems[0].Text;

            // Convert to byte array
            byte[] bytes = line.Split(' ')
                               .Select(b => Convert.ToByte(b, 16))
                               .ToArray();

            _serialPort.Write(bytes, 0, bytes.Length);
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

            textBox1.Clear();
            textBox1.Focus();
        }

        private void textBoxHex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Uri.IsHexDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // ignore invalid input
            }
        }
    }
}
