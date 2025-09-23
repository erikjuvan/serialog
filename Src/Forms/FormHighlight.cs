using System.Data;
using System.Text.Json;

namespace serialog
{
    public partial class FormHighlight : Form
    {
        public static Highlights Highlights = new Highlights();
        private Color hlBgColor;
        private Color hlFgColor;

        public FormHighlight()
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
            SettingsManager.SettingsFolderChanged += (_, __) => Populate_comboBox_preset_onFolderChange();
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

        private void AddHighlightEntryToListView(ref ListView listView, Highlight highlightEntry)
        {
            ListViewItem item = new ListViewItem
            {
                Checked = highlightEntry.Enabled,
                Text = highlightEntry.Text,
                ForeColor = highlightEntry.ForeColor,
                BackColor = highlightEntry.BackColor,
            };

            // Optional markers for regex / ignorecase / hide
            item.SubItems.Add(highlightEntry.UseRegex ? "*" : "");
            item.SubItems.Add(highlightEntry.IgnoreCase ? "*" : "");
            item.SubItems.Add(highlightEntry.Hide ? "*" : "");

            var style = highlightEntry.Style ?? new HighlightStyle { FontStyle = FontStyle.Regular };
            item.Font = new Font("Courier New", 10, style.FontStyle);

            var addedItem = listView.Items.Add(item);
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

            Highlight highlightEntry = new Highlight
            {
                Enabled = true,
                UseRegex = checkBoxUseRegex.Checked,
                Text = textBox_string.Text,
                IgnoreCase = checkBox_ignorecase.Checked,
                Style = new HighlightStyle
                {
                    ForeColor = fgcol,
                    BackColor = bgcol,
                    FontStyle = (checkBox_bold.Checked ? FontStyle.Bold : FontStyle.Regular)
                              | (checkBox_italic.Checked ? FontStyle.Italic : FontStyle.Regular),
                    Hide = checkBox_hide.Checked
                }
            };

            Highlights.BeginUpdate();
            Highlights.Add(highlightEntry);
            AddHighlightEntryToListView(ref listView1, highlightEntry);
            Highlights.EndUpdate();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            Highlights.BeginUpdate();
            while (listView1.SelectedIndices.Count > 0)
            {
                int idx = listView1.SelectedIndices[listView1.SelectedIndices.Count - 1];
                Highlights.RemoveAt(idx);
                listView1.Items.RemoveAt(idx);
            }
            Highlights.EndUpdate();
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
            Highlights.BeginUpdate();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (lvi.Index > 0)
                {
                    int indexTo = lvi.Index - 1;
                    int indexFrom = lvi.Index;

                    listView1.Items.RemoveAt(indexFrom);
                    listView1.Items.Insert(indexTo, lvi);
                    listView1.Items[indexTo].Focused = true;

                    // Swap in Highlights
                    Highlights.Move(indexFrom, indexTo);
                }
            }
            Highlights.EndUpdate();
        }

        private void MoveItemDown()
        {
            Highlights.BeginUpdate();
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                if (lvi.Index < listView1.Items.Count - 1)
                {
                    int indexTo = lvi.Index + 1;
                    int indexFrom = lvi.Index;

                    listView1.Items.RemoveAt(indexFrom);
                    listView1.Items.Insert(indexTo, lvi);
                    listView1.Items[indexTo].Focused = true;

                    // Swap in Highlights
                    Highlights.Move(indexFrom, indexTo);
                }
            }
            Highlights.EndUpdate();
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

        private void comboBox_fgcolor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.Handled = true;

                if (listView1.SelectedItems.Count <= 0)
                {
                    return;
                }

                Color fgcol = Helpers.ParseColorInput(comboBox_fgcolor.Text, ListView.DefaultForeColor);

                Highlights.BeginUpdate();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.ForeColor = fgcol;
                    Highlights.Items[item.Index].ForeColor = fgcol;
                }
                Highlights.EndUpdate();
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

                Color bgcol = Helpers.ParseColorInput(comboBox_bgcolor.Text, ListView.DefaultBackColor);

                Highlights.BeginUpdate();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    item.BackColor = bgcol;
                    Highlights.Items[item.Index].BackColor = bgcol;
                }
                Highlights.EndUpdate();
            }
        }

        private void Populate_comboBox_preset()
        {
            Helpers.PopulateComboBox(comboBox_preset, SettingsManager.Default.HighlightPresetExt, false);
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
            Directory.CreateDirectory(SettingsManager.SettingsFolder);

            string fullpath = Path.Combine(SettingsManager.SettingsFolder, comboBox_preset.Text + SettingsManager.Default.HighlightPresetExt);

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
            string json = JsonSerializer.Serialize(Highlights.Items, options);
            File.WriteAllText(fullpath, json);

            MessageBox.Show(
                $"Preset saved to '{fullpath}'",
                "Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void LoadPreset(string presetFileName)
        {
            string fullpath = Path.Combine(SettingsManager.SettingsFolder, presetFileName + SettingsManager.Default.HighlightPresetExt);

            if (!File.Exists(fullpath))
            {
                MessageBox.Show($"Preset '{fullpath}' doesn't exist!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            comboBox_preset.Text = presetFileName;

            try
            {
                string json = File.ReadAllText(fullpath);
                var entries = JsonSerializer.Deserialize<List<Highlight>>(json);

                if (entries != null)
                {
                    Highlights.BeginUpdate();
                    foreach (var entry in entries)
                    {
                        Highlights.Add(entry);
                        AddHighlightEntryToListView(ref listView1, entry);
                    }
                    Highlights.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load preset: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_preset_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_preset.Text))
            {
                MessageBox.Show("Select a preset to load.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadPreset(comboBox_preset.Text);
        }

        private void button_deletepreset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox_preset.Text))
            {
                MessageBox.Show("Select a preset to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullpath = Path.Combine(SettingsManager.SettingsFolder, comboBox_preset.Text + SettingsManager.Default.HighlightPresetExt);

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
            Highlights.Items[e.Item.Index].Enabled = e.Item.Checked;
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

            var entry = Highlights.Items[listView1.SelectedIndices[0]];

            textBox_string.Text = entry.Text;
            checkBoxUseRegex.Checked = entry.UseRegex;
            comboBox_fgcolor.Text = ColorToNameOrHex(entry.ForeColor);
            comboBox_fgcolor.ForeColor = entry.ForeColor;
            comboBox_bgcolor.Text = ColorToNameOrHex(entry.BackColor);
            comboBox_bgcolor.BackColor = entry.BackColor;
            checkBox_ignorecase.Checked = entry.IgnoreCase;
            checkBox_bold.Checked = entry.Style.FontStyle.HasFlag(FontStyle.Bold);
            checkBox_italic.Checked = entry.Style.FontStyle.HasFlag(FontStyle.Italic);
            checkBox_hide.Checked = entry.Hide;

            textBox_string.Focus();
        }

        private void textBox_string_TextChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                item.Text = textBox_string.Text;
                Highlights.Items[item.Index].Text = item.Text;
            }
            Highlights.EndUpdate();
        }

        private void checkBox_bold_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                var entry = Highlights.Items[item.Index];

                // Start with the existing font style
                FontStyle style = item.Font.Style;

                if (checkBox_bold.Checked)
                    style |= FontStyle.Bold;  // add bold
                else
                    style &= ~FontStyle.Bold; // remove bold

                // Apply to the ListView item
                item.Font = new Font(item.Font, style);

                // Update the entry's HighlightStyle
                entry.Style.FontStyle = style;
            }
            Highlights.EndUpdate();
        }

        private void checkBox_italic_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                var entry = Highlights.Items[item.Index];

                // Start with the existing font style
                FontStyle style = item.Font.Style;

                if (checkBox_italic.Checked)
                    style |= FontStyle.Italic;  // add italic
                else
                    style &= ~FontStyle.Italic; // remove italic

                // Apply to the ListView item
                item.Font = new Font(item.Font, style);

                // Update the entry's HighlightStyle
                entry.Style.FontStyle = style;
            }
            Highlights.EndUpdate();
        }

        private void checkBoxUseRegex_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBoxUseRegex.Checked)
                {
                    item.SubItems[1].Text = "*";
                    Highlights.Items[item.Index].UseRegex = true;
                }
                else
                {
                    item.SubItems[1].Text = "";
                    Highlights.Items[item.Index].UseRegex = false;
                }
            }
            Highlights.EndUpdate();
        }

        private void checkBox_ignorecase_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBox_ignorecase.Checked)
                {
                    item.SubItems[2].Text = "*";
                    Highlights.Items[item.Index].IgnoreCase = true;
                }
                else
                {
                    item.SubItems[2].Text = "";
                    Highlights.Items[item.Index].IgnoreCase = false;
                }
            }
            Highlights.EndUpdate();
        }

        private void checkBox_hide_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            Highlights.BeginUpdate();
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (checkBox_hide.Checked)
                {
                    item.SubItems[3].Text = "*";
                    Highlights.Items[item.Index].Hide = true;
                }
                else
                {
                    item.SubItems[3].Text = "";
                    Highlights.Items[item.Index].Hide = false;
                }
            }
            Highlights.EndUpdate();
        }
    }
}
