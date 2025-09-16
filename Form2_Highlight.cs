using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace serialog
{
    public partial class Form2_Highlight : Form
    {
        static public HighlightEntries highlightEntries = new HighlightEntries();
        private Color hlBgColor;
        private Color hlFgColor;
        private string _highlightsFileExtension = ".highlight";

        public Form2_Highlight()
        {
            InitializeComponent();

            // Fill colors
            comboBox_fgcolor.Items.AddRange(Enum.GetNames(typeof(KnownColor)));
            comboBox_bgcolor.Items.AddRange(Enum.GetNames(typeof(KnownColor)));

            // Add index changed event function
            comboBox_fgcolor.SelectedIndexChanged += comboBox_fgcolor_SelectedIndexChanged;
            comboBox_bgcolor.SelectedIndexChanged += comboBox_bgcolor_SelectedIndexChanged;

            // Populate preset combobox
            Populate_comboBox_preset_onFolderChange();

            // Subscribe to folder change
            AppSettings.SettingsFolderChanged += (_, __) => Populate_comboBox_preset_onFolderChange();
        }

        // Handle when user selects a color from the ComboBox
        private void comboBox_fgcolor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = comboBox_fgcolor.SelectedItem.ToString();
            Color color;
            if (selectedName.Equals("Transparent", StringComparison.OrdinalIgnoreCase))
            {
                // Special handling for transparent
                color = Color.White; // or some default background color
            }
            else
            {
                color = Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor), selectedName));
            }
            comboBox_fgcolor.ForeColor = color;
            hlFgColor = color;

            // Delay clearing selection until after the control updates
            this.BeginInvoke((Action)(() =>
            {
                comboBox_fgcolor.SelectionStart = comboBox_fgcolor.Text.Length;
                comboBox_fgcolor.SelectionLength = 0;
            }));
        }

        // Handle when user selects a color from the ComboBox
        private void comboBox_bgcolor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = comboBox_bgcolor.SelectedItem.ToString();
            Color color;
            if (selectedName.Equals("Transparent", StringComparison.OrdinalIgnoreCase))
            {
                // Special handling for transparent
                color = Color.White; // or some default background color
            }
            else
            {
                color = Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor), selectedName));
            }
            comboBox_bgcolor.BackColor = color;
            hlBgColor = color;

            // Delay clearing selection until after the control updates
            this.BeginInvoke((Action)(() =>
            {
                comboBox_bgcolor.SelectionStart = comboBox_bgcolor.Text.Length;
                comboBox_bgcolor.SelectionLength = 0;
            }));
        }

        private void AddHighlightEntryToListView(ref ListView listView, HighlightEntry highlightEntry)
        {
            ListViewItem item = new ListViewItem();
            item.Checked = highlightEntry.Enabled;
            item.Text = highlightEntry.Text;
            item.ForeColor = highlightEntry.ForeColor;
            item.BackColor = highlightEntry.BackColor;

            if (highlightEntry.IgnoreCase)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Hide)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Remove)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Bold && highlightEntry.Italic)
                item.Font = new Font("Courier New", 10, FontStyle.Bold | FontStyle.Italic);
            else if (highlightEntry.Bold)
                item.Font = new Font("Courier New", 10, FontStyle.Bold);
            else if (highlightEntry.Italic)
                item.Font = new Font("Courier New", 10, FontStyle.Italic);

            var addedItem = listView.Items.Add(item);
            addedItem.EnsureVisible();
        }

        private void InsertHighlightEntryToListView(ref ListView listView, int index, HighlightEntry highlightEntry)
        {
            ListViewItem item = new ListViewItem();
            item.Checked = highlightEntry.Enabled;
            item.Text = highlightEntry.Text;
            item.ForeColor = highlightEntry.ForeColor;
            item.BackColor = highlightEntry.BackColor;

            if (highlightEntry.IgnoreCase)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Hide)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Remove)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.Bold && highlightEntry.Italic)
                item.Font = new Font("Courier New", 10, FontStyle.Bold | FontStyle.Italic);
            else if (highlightEntry.Bold)
                item.Font = new Font("Courier New", 10, FontStyle.Bold);
            else if (highlightEntry.Italic)
                item.Font = new Font("Courier New", 10, FontStyle.Italic);

            var addedItem = listView.Items.Insert(index, item);
            addedItem.EnsureVisible();
        }

        private void Form2_Highlight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            // No text in textbox
            if (textBox_string.Text.Length == 0)
                return;

            Color bgcol;
            if (comboBox_bgcolor.Text.Length == 0)
                bgcol = Color.White;
            else
                bgcol = hlBgColor;

            Color fgcol;
            if (comboBox_fgcolor.Text.Length == 0)
                fgcol = Color.Black;
            else
                fgcol = hlFgColor;

            HighlightEntry highlightEntry = new HighlightEntry();
            highlightEntry.Enabled = true;
            highlightEntry.Text = textBox_string.Text;
            highlightEntry.ForeColor = fgcol;
            highlightEntry.BackColor = bgcol;
            highlightEntry.IgnoreCase = checkBox_ignorecase.Checked;
            highlightEntry.Bold = checkBox_bold.Checked;
            highlightEntry.Italic = checkBox_italic.Checked;
            highlightEntry.Hide = checkBox_hide.Checked;
            highlightEntry.Remove = checkBox_remove.Checked;

            highlightEntries.BeginUpdate();
            highlightEntries.Add(highlightEntry);
            AddHighlightEntryToListView(ref listView1, highlightEntry);
            highlightEntries.EndUpdate();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            highlightEntries.BeginUpdate();
            while (listView1.SelectedIndices.Count > 0)
            {
                int idx = listView1.SelectedIndices[listView1.SelectedIndices.Count - 1];
                highlightEntries.RemoveAt(idx);
                listView1.Items.RemoveAt(idx);
            }
            highlightEntries.EndUpdate();
        }

        private void button_fgcolor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialog1.AllowFullOpen = true;
            colorDialog1.FullOpen = true; // show advanced panel by default
            colorDialog1.AnyColor = true;
            colorDialog1.Color = Color.FromArgb(255, 0, 0); // Example: medium red

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog1.Color;

                // Save color
                hlFgColor = selectedColor;

                // Try to find if it's a known color name
                var knownName = Enum.GetValues(typeof(KnownColor))
                                    .Cast<KnownColor>()
                                    .FirstOrDefault(kc => Color.FromKnownColor(kc).ToArgb() == selectedColor.ToArgb());

                if (knownName != 0)
                {
                    comboBox_fgcolor.SelectedItem = knownName.ToString();
                }
                else
                {
                    // If not a known color, just show HEX
                    comboBox_fgcolor.Text = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
                }

                // Set Foreground color
                comboBox_fgcolor.ForeColor = selectedColor;
            }
        }

        private void button_bgcolor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialog1.AllowFullOpen = true;
            colorDialog1.FullOpen = true; // show advanced panel by default
            colorDialog1.AnyColor = true;
            colorDialog1.Color = Color.FromArgb(255, 180, 180); // Example: medium red

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog1.Color;

                // Save color
                hlBgColor = selectedColor;

                // Try to find if it's a known color name
                var knownName = Enum.GetValues(typeof(KnownColor))
                                    .Cast<KnownColor>()
                                    .FirstOrDefault(kc => Color.FromKnownColor(kc).ToArgb() == selectedColor.ToArgb());

                if (knownName != 0)
                {
                    comboBox_bgcolor.SelectedItem = knownName.ToString();
                }
                else
                {
                    // If not a known color, just show HEX
                    comboBox_bgcolor.Text = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
                }

                // Set Background color
                comboBox_bgcolor.BackColor = selectedColor;
            }
        }

        private void MoveItemUp()
        {
            highlightEntries.BeginUpdate();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int indexTo = lvi.Index - 1;
                    int indexFrom = lvi.Index;
                    listView1.Items.RemoveAt(indexFrom);
                    listView1.Items.Insert(indexTo, lvi);
                    listView1.Items[indexTo].Focused = true;

                    // Swap
                    var tmpEntry = highlightEntries.Items[indexTo];
                    highlightEntries.Items[indexTo] = highlightEntries.Items[indexFrom];
                    highlightEntries.Items[indexFrom] = tmpEntry;
                }
            }
            highlightEntries.EndUpdate();
        }

        private void MoveItemDown()
        {
            highlightEntries.BeginUpdate();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (lvi.Index < listView1.Items.Count - 1)
                {
                    int indexTo = lvi.Index + 1;
                    int indexFrom = lvi.Index;
                    listView1.Items.RemoveAt(indexFrom);
                    listView1.Items.Insert(indexTo, lvi);
                    listView1.Items[indexTo].Focused = true;

                    // Swap
                    var tmpEntry = highlightEntries.Items[indexTo];
                    highlightEntries.Items[indexTo] = highlightEntries.Items[indexFrom];
                    highlightEntries.Items[indexFrom] = tmpEntry;
                }
            }
            highlightEntries.EndUpdate();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }

                e.Handled = true;
            }

            if (e.Alt && e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                MoveItemUp();
            }

            if (e.Alt && e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                MoveItemDown();
            }

            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
                button_delete_Click(sender, e);
            }
        }

        private void button_moveup_Click(object sender, EventArgs e)
        {
            MoveItemUp();
            listView1.Focus();
        }

        private void button_movedown_Click(object sender, EventArgs e)
        {
            MoveItemDown();
            listView1.Focus();
        }

        private void textBox_string_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                button_add_Click(sender, e);
            }
        }

        private Color ParseColorInput(string input, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(input))
                return fallback;

            input = input.Trim();

            // Hex input?
            if (input.StartsWith("#"))
            {
                string hex = input.Substring(1);

                // #RRGGBB
                if (hex.Length == 6 &&
                    int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
                {
                    return Color.FromArgb(
                        (rgb >> 16) & 0xFF,
                        (rgb >> 8) & 0xFF,
                        rgb & 0xFF
                    );
                }

                // #AARRGGBB
                if (hex.Length == 8 &&
                    int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int argb))
                {
                    return Color.FromArgb(
                        (argb >> 24) & 0xFF,
                        (argb >> 16) & 0xFF,
                        (argb >> 8) & 0xFF,
                        argb & 0xFF
                    );
                }
            }

            // Named color
            var named = Color.FromName(input);
            if (!named.IsEmpty)
                return named;

            // Fallback if nothing matched
            return fallback;
        }

        private void comboBox_fgcolor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;

                if (listView1.SelectedItems.Count <= 0)
                {
                    return;
                }

                Color fgcol = ParseColorInput(comboBox_fgcolor.Text, ListView.DefaultForeColor);

                highlightEntries.BeginUpdate();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.ForeColor = fgcol;
                    highlightEntries.Items[item.Index].ForeColor = fgcol;
                }
                highlightEntries.EndUpdate();
            }
        }

        private void comboBox_bgcolor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;

                if (listView1.SelectedItems.Count <= 0)
                {
                    return;
                }

                Color bgcol = ParseColorInput(comboBox_bgcolor.Text, ListView.DefaultBackColor);

                highlightEntries.BeginUpdate();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.BackColor = bgcol;
                    highlightEntries.Items[item.Index].BackColor = bgcol;
                }
                highlightEntries.EndUpdate();
            }
        }

        private void Populate_comboBox_preset()
        {
            comboBox_preset.Items.Clear();

            try
            {
                string settingsDir = AppSettings.SettingsFolder;

                if (!Directory.Exists(settingsDir))
                    return;

                // Only look for .highlight files in .settings (no recursion)
                var listOfFiles = Directory.EnumerateFiles(settingsDir, "*" + _highlightsFileExtension, SearchOption.TopDirectoryOnly);

                foreach (var file in listOfFiles)
                {
                    string presetName = Path.GetFileNameWithoutExtension(file);
                    comboBox_preset.Items.Add(presetName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load presets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Populate_comboBox_preset_onFolderChange()
        {
            Populate_comboBox_preset();

            if (comboBox_preset.Items.Count > 0)
            {
                comboBox_preset.SelectedIndex = 0; // sets Text as well
            }
            else
            {
                comboBox_preset.Text = "";
            }
        }


        private void button_preset_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_preset.Text))
                return;

            // Ensure the folder exists
            Directory.CreateDirectory(AppSettings.SettingsFolder);

            string fullpath = Path.Combine(AppSettings.SettingsFolder, comboBox_preset.Text + _highlightsFileExtension);

            if (File.Exists(fullpath))
            {
                var ret = MessageBox.Show(
                    $"Preset '{fullpath}' already exists, overwrite it?",
                    "Overwrite?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ret == DialogResult.No)
                    return;
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(highlightEntries.Items, options);
            File.WriteAllText(fullpath, json);

            MessageBox.Show(
                $"Preset saved to '{fullpath}'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void button_preset_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_preset.Text))
            {
                MessageBox.Show("Select a preset to load.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullpath = Path.Combine(AppSettings.SettingsFolder, comboBox_preset.Text + _highlightsFileExtension);

            if (!File.Exists(fullpath))
            {
                MessageBox.Show($"Preset '{fullpath}' doesn't exist!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string json = File.ReadAllText(fullpath);
                var entries = JsonSerializer.Deserialize<List<HighlightEntry>>(json);

                if (entries != null)
                {
                    highlightEntries.BeginUpdate();
                    foreach (var entry in entries)
                    {
                        highlightEntries.Add(entry);
                        AddHighlightEntryToListView(ref listView1, entry);
                    }
                    highlightEntries.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load preset: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_deletepreset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_preset.Text))
            {
                MessageBox.Show("Select a preset to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullpath = Path.Combine(AppSettings.SettingsFolder, comboBox_preset.Text + _highlightsFileExtension);

            if (!File.Exists(fullpath))
            {
                MessageBox.Show($"Preset '{fullpath}' does not exist.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete preset '{fullpath}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                File.Delete(fullpath);
                comboBox_preset.Text = "";

                // Refresh the ComboBox to reflect current presets
                Populate_comboBox_preset();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete '{fullpath}'.\n\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_preset_DropDown(object sender, EventArgs e)
        {
            Populate_comboBox_preset();
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            highlightEntries.Items[e.Item.Index].Enabled = e.Item.Checked;
        }

        private string ColorToNameOrHex(Color color)
        {
            if (color.IsKnownColor)
                return color.Name; // e.g., "Red"
            else
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}"; // e.g., "#F1EFC2"
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;

            var entry = highlightEntries.Items[listView1.SelectedIndices[0]];

            textBox_string.Text = entry.Text;
            comboBox_fgcolor.Text = ColorToNameOrHex(entry.ForeColor);
            comboBox_fgcolor.ForeColor = entry.ForeColor;
            comboBox_bgcolor.Text = ColorToNameOrHex(entry.BackColor);
            comboBox_bgcolor.BackColor = entry.BackColor;
            checkBox_ignorecase.Checked = entry.IgnoreCase;
            checkBox_bold.Checked = entry.Bold;
            checkBox_italic.Checked = entry.Italic;
            checkBox_hide.Checked = entry.Hide;
            checkBox_remove.Checked = entry.Remove;
        }

        private void textBox_string_TextChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                item.Text = textBox_string.Text;
                highlightEntries.Items[item.Index].Text = item.Text;
            }
            highlightEntries.EndUpdate();
        }

        private void checkBox_bold_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                FontStyle style = item.Font.Style;

                if (checkBox_bold.Checked)
                    style |= FontStyle.Bold;  // add bold
                else
                    style &= ~FontStyle.Bold; // remove bold

                item.Font = new Font(item.Font, style);
                highlightEntries.Items[item.Index].Bold = checkBox_bold.Checked;
            }
            highlightEntries.EndUpdate();
        }

        private void checkBox_italic_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                FontStyle style = item.Font.Style;

                if (checkBox_italic.Checked)
                    style |= FontStyle.Italic;  // add italic
                else
                    style &= ~FontStyle.Italic; // remove italic

                item.Font = new Font(item.Font, style);
                highlightEntries.Items[item.Index].Italic = checkBox_italic.Checked;
            }
            highlightEntries.EndUpdate();
        }

        private void checkBox_ignorecase_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBox_ignorecase.Checked)
                {
                    item.SubItems[1].Text = "*";
                    highlightEntries.Items[item.Index].IgnoreCase = true;
                }
                else
                {
                    item.SubItems[1].Text = "";
                    highlightEntries.Items[item.Index].IgnoreCase = false;
                }
            }
            highlightEntries.EndUpdate();
        }

        private void checkBox_hide_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBox_hide.Checked)
                {
                    item.SubItems[2].Text = "*";
                    highlightEntries.Items[item.Index].Hide = true;
                }
                else
                {
                    item.SubItems[2].Text = "";
                    highlightEntries.Items[item.Index].Hide = false;
                }
            }
            highlightEntries.EndUpdate();
        }

        private void checkBox_remove_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            highlightEntries.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBox_remove.Checked)
                {
                    item.SubItems[3].Text = "*";
                    highlightEntries.Items[item.Index].Remove = true;
                }
                else
                {
                    item.SubItems[3].Text = "";
                    highlightEntries.Items[item.Index].Remove = false;
                }
            }
            highlightEntries.EndUpdate();
        }
    }
}
