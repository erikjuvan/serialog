namespace serialog
{
    public partial class FormSerialSend : Form
    {
        private Form1 _parentForm;
        private System.IO.Ports.SerialPort _serial;
        private DataLog _dataLog;
        private string _sendFileExtension = ".send";

        public class DataRowModel
        {
            public string Description { get; set; }
            public string HexData { get; set; }
        }

        internal FormSerialSend(Form1 parent, System.IO.Ports.SerialPort serial, DataLog dataLog)
        {
            InitializeComponent();

            _parentForm = parent;
            _serial = serial;
            _dataLog = dataLog;

            // Populate file combobox
            Populate_comboBox_file_onFolderChange();

            // Subscribe to folder change
            AppSettings.SettingsFolderChanged += (_, __) => Populate_comboBox_file_onFolderChange();
        }

        private bool CanSendData()
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a line to send.");
                return false;
            }

            if (_serial == null)
            {
                MessageBox.Show("Serial port is null!");
                return false;
            }

            if (!_serial.IsOpen)
            {
                MessageBox.Show("Serial port is closed!");
                return false;
            }

            return true;
        }

        private void SendSelectedLine()
        {

            // Get distinct rows from selected cells
            var rowsToSend = dataGridView1.SelectedCells
                                .Cast<DataGridViewCell>()
                                .Select(c => c.OwningRow)
                                .Where(r => !r.IsNewRow)
                                .Distinct()
                                .ToList();

            foreach (var row in rowsToSend)
            {
                // Assume the second column (index 1) contains the hex string
                string line = Convert.ToString(row.Cells[1].Value)?.Replace(" ", "") ?? "";
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    // Convert to byte array
                    byte[] bytes = Enumerable.Range(0, line.Length / 2)
                                             .Select(i => Convert.ToByte(line.Substring(i * 2, 2), 16))
                                             .ToArray();

                    _serial.Write(bytes, 0, bytes.Length);

                    // Add to log and view
                    string sendString = FormatHelpers.BytesToDisplayString(bytes, false, _parentForm.DisplayNonPrintableCharsAsHex);
                    _parentForm.AddLogEntry(new DataEntry(DateTime.Now, DataEntrySource.SerialTX, sendString));
                }
                catch (FormatException)
                {
                    MessageBox.Show($"Invalid hex in row {row.Index + 1}");
                }
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

        private void textBox_send_every_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // ignore the key press
            }
        }

        private void Populate_comboBox_file()
        {
            Helpers.PopulateComboBox(comboBox_file, _sendFileExtension, false);
        }

        private void Populate_comboBox_file_onFolderChange()
        {
            Populate_comboBox_file();

            if (comboBox_file.Items.Count > 0)
            {
                comboBox_file.SelectedIndex = 0; // sets Text as well
            }
            else
            {
                comboBox_file.Text = "";
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

            // Ensure the folder exists
            Directory.CreateDirectory(AppSettings.SettingsFolder);

            // Combine folder + file
            string fullpath = Path.Combine(AppSettings.SettingsFolder, comboBox_file.Text + _sendFileExtension);

            if (File.Exists(fullpath))
            {
                var ret = MessageBox.Show($"File '{fullpath}' already exists, overwrite it?",
                                          "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ret == DialogResult.No)
                    return;
            }

            var rows = dataGridView1
                .Rows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .Select(r => new DataRowModel
                {
                    Description = Convert.ToString(r.Cells[0].Value),
                    HexData = Convert.ToString(r.Cells[1].Value)
                })
                .ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(rows, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(fullpath, json);

            MessageBox.Show(
                $"Preset saved to '{fullpath}'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public async void LoadPreset(string presetFileName)
        {
            string fullpath = Path.Combine(AppSettings.SettingsFolder, presetFileName + _sendFileExtension);

            if (!File.Exists(fullpath))
            {
                MessageBox.Show($"File '{fullpath}' doesn't exist!", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            comboBox_file.Text = presetFileName;

            var json = await File.ReadAllTextAsync(fullpath);
            var rows = System.Text.Json.JsonSerializer.Deserialize<List<DataRowModel>>(json);

            dataGridView1.SuspendLayout();
            foreach (var row in rows)
            {
                dataGridView1.Rows.Add(row.Description, row.HexData);
            }
            dataGridView1.ResumeLayout();
        }

        private void button_file_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_file.Text))
            {
                MessageBox.Show("Select a file to load.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadPreset(comboBox_file.Text);
        }

        private void button_file_delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_file.Text))
            {
                MessageBox.Show("Select a file to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullpath = Path.Combine(AppSettings.SettingsFolder, comboBox_file.Text + _sendFileExtension);

            if (!File.Exists(fullpath))
            {
                MessageBox.Show($"File '{fullpath}' does not exist.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete file '{fullpath}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                File.Delete(fullpath);
                comboBox_file.Text = "";

                // Refresh the ComboBox to reflect current files
                Populate_comboBox_file();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete '{fullpath}'.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // Clear cell and if entire row is empty remove the row
            if (e.KeyCode == Keys.Delete)
            {
                // Track rows that might need removal
                var rowsToCheck = dataGridView1.SelectedCells
                                     .Cast<DataGridViewCell>()
                                     .Select(c => c.OwningRow)
                                     .Where(r => !r.IsNewRow)
                                     .Distinct()
                                     .ToList();

                // Clear the selected cells first
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    if (!cell.OwningRow.IsNewRow && !cell.ReadOnly)
                        cell.Value = null;
                }

                // Remove rows where all cells are empty
                foreach (var row in rowsToCheck)
                {
                    bool allEmpty = row.Cells.Cast<DataGridViewCell>()
                                       .All(c => string.IsNullOrWhiteSpace(Convert.ToString(c.Value)));
                    if (allEmpty)
                        dataGridView1.Rows.Remove(row);
                }

                e.Handled = true;
            }

            // Copy multiple cells
            if (e.Control && e.KeyCode == Keys.C)
            {
                var selectedCells = dataGridView1.SelectedCells
                    .Cast<DataGridViewCell>()
                    .OrderBy(c => c.RowIndex)
                    .ThenBy(c => c.ColumnIndex);

                if (selectedCells.Any())
                {
                    int minRow = selectedCells.First().RowIndex;
                    int maxRow = selectedCells.Last().RowIndex;
                    int minCol = selectedCells.Min(c => c.ColumnIndex);
                    int maxCol = selectedCells.Max(c => c.ColumnIndex);

                    var rows = new List<string>();
                    for (int r = minRow; r <= maxRow; r++)
                    {
                        var cols = new List<string>();
                        for (int c = minCol; c <= maxCol; c++)
                        {
                            var cell = dataGridView1[c, r];
                            cols.Add(cell.Value?.ToString() ?? "");
                        }
                        rows.Add(string.Join("\t", cols));
                    }

                    Clipboard.SetText(string.Join(Environment.NewLine, rows));
                }
                e.Handled = true;
            }
            // Paste multiple cells
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (!dataGridView1.CurrentCell.ReadOnly)
                {
                    string pasteText = Clipboard.GetText();
                    string[] lines = pasteText.Split(new[] { "\r\n", "\n" },
                                                     StringSplitOptions.None);

                    int startRow = dataGridView1.CurrentCell.RowIndex;
                    int startCol = dataGridView1.CurrentCell.ColumnIndex;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i]))
                            continue;

                        string[] values = lines[i].Split('\t');

                        for (int j = 0; j < values.Length; j++)
                        {
                            int row = startRow + i;
                            int col = startCol + j;

                            // Make sure the row exists (auto-expand if at bottom)
                            if (row >= dataGridView1.Rows.Count)
                            {
                                dataGridView1.Rows.Add();
                            }

                            if (col < dataGridView1.Columns.Count)
                            {
                                var cell = dataGridView1[col, row];
                                if (!cell.ReadOnly)
                                {
                                    // Simulate editing so CellEndEdit formatting still works
                                    dataGridView1.CurrentCell = cell;
                                    dataGridView1.BeginEdit(true);

                                    if (dataGridView1.EditingControl is TextBox tb)
                                    {
                                        tb.Text = values[j];
                                        tb.SelectionStart = tb.Text.Length;
                                    }

                                    dataGridView1.EndEdit();
                                }
                            }
                        }
                    }
                }
                e.Handled = true;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // ignore header clicks
            {
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex]; // select the cell
                dataGridView1.BeginEdit(true); // start editing
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = dataGridView1.HitTest(e.X, e.Y);

            if (hit.Type != DataGridViewHitTestType.Cell)
            {
                dataGridView1.ClearSelection();      // deselect all cells
                dataGridView1.CurrentCell = null;    // optional: remove the current cell focus
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox tb)
            {
                tb.KeyPress -= Tb_KeyPress;

                tb.KeyPress += Tb_KeyPress;   // character validation
            }
        }

        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 1) // second column
            {
                 // Allow digits and control keys only
                if (!char.IsControl(e.KeyChar) && !Uri.IsHexDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                {
                    e.Handled = true; // ignore invalid input
                }
            }
        }

        private string FormatHexText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // Keep only hex digits, convert to uppercase
            var hexChars = text.Where(c => Uri.IsHexDigit(c))
                               .Select(c => char.ToUpper(c))
                               .ToArray();

            var formatted = Enumerable.Range(0, hexChars.Length / 2)
                                      .Select(i => new string(hexChars, i * 2, 2))
                                      .ToList();

            // Pad leftover single nibble
            if (hexChars.Length % 2 != 0)
                formatted.Add("0" + hexChars.Last());

            return string.Join(" ", formatted);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 1) return; // only second column
            var cell = dataGridView1[e.ColumnIndex, e.RowIndex];
            cell.Value = FormatHexText(Convert.ToString(cell.Value));
        }
    }
}
