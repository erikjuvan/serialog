using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serialog
{
    public partial class Form2_Highlight : Form
    {
        static public HighlightEntires highlightEntries = new HighlightEntires();
        private HighlightEntires tempHighlightEntires = new HighlightEntires();

        public Form2_Highlight()
        {
            InitializeComponent();

            // copy to temp
            tempHighlightEntires = new HighlightEntires(highlightEntries);
            foreach (HighlightEntry entry in tempHighlightEntires.Items)
            {
                AddHighlightEntryToListView(ref listView1, entry);
            }

            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor knowColor in colors)
            {
                Color color = Color.FromKnownColor(knowColor);
                comboBox_bgcolor.Items.Add(color);
                comboBox_fgcolor.Items.Add(color);
            }
        }

        private void AddHighlightEntryToListView(ref ListView listView, HighlightEntry highlightEntry)
        {
            ListViewItem item = new ListViewItem();
            item.Text = highlightEntry.text;
            item.ForeColor = highlightEntry.foreColor;
            item.BackColor = highlightEntry.backColor;

            if (highlightEntry.ignoreCase)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.hide)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.bold && highlightEntry.italic)
                item.Font = new Font("Courier New", 10, FontStyle.Bold | FontStyle.Italic);
            else if (highlightEntry.bold)
                item.Font = new Font("Courier New", 10, FontStyle.Bold);
            else if (highlightEntry.italic)
                item.Font = new Font("Courier New", 10, FontStyle.Italic);

            var addedItem = listView.Items.Add(item);
            addedItem.EnsureVisible();
        }

        private void InsertHighlightEntryToListView(ref ListView listView, int index, HighlightEntry highlightEntry)
        {
            ListViewItem item = new ListViewItem();
            item.Text = highlightEntry.text;
            item.ForeColor = highlightEntry.foreColor;
            item.BackColor = highlightEntry.backColor;

            if (highlightEntry.ignoreCase)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.hide)
                item.SubItems.Add("*");
            else
                item.SubItems.Add("");

            if (highlightEntry.bold && highlightEntry.italic)
                item.Font = new Font("Courier New", 10, FontStyle.Bold | FontStyle.Italic);
            else if (highlightEntry.bold)
                item.Font = new Font("Courier New", 10, FontStyle.Bold);
            else if (highlightEntry.italic)
                item.Font = new Font("Courier New", 10, FontStyle.Italic);

            var addedItem = listView.Items.Insert(index, item);
            addedItem.EnsureVisible();
        }

        private void Form2_Highlight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        static public void InitializeHighlightSettings()
        {
            // Init highlightSettings from a file saved somewhere


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
                bgcol = Color.FromName(comboBox_bgcolor.Text);

            Color fgcol;
            if (comboBox_fgcolor.Text.Length == 0)
                fgcol = Color.Black;
            else
                fgcol = Color.FromName(comboBox_fgcolor.Text);

            HighlightEntry highlightEntry = new HighlightEntry(textBox_string.Text,
                fgcol, bgcol,
                checkBox_ignorecase.Checked, checkBox_bold.Checked, 
                checkBox_italic.Checked, checkBox_hide.Checked);

            // Check if same text already exists and ask to replace with existing
            foreach (ListViewItem item in listView1.Items)
            {
                if (highlightEntry.text == item.Text)
                {
                    if (MessageBox.Show("Replace existing item with this one?", "Replace?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int index = item.Index;
                        listView1.Items.RemoveAt(item.Index);
                        InsertHighlightEntryToListView(ref listView1, index, highlightEntry);
                        tempHighlightEntires.RemoveAt(index);
                        tempHighlightEntires.Insert(index, highlightEntry);
                    }

                    return;
                }
            }

            tempHighlightEntires.Add(highlightEntry);
            AddHighlightEntryToListView(ref listView1, highlightEntry);
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNoCancel);

            if (res == DialogResult.Cancel)
                return;

            if (res == DialogResult.Yes)
                highlightEntries = new HighlightEntires(tempHighlightEntires);
            this.Close();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            while (listView1.SelectedIndices.Count > 0)
            {
                int idx = listView1.SelectedIndices[listView1.SelectedIndices.Count - 1];
                tempHighlightEntires.RemoveAt(idx);
                listView1.Items.RemoveAt(idx);
            }
        }

        private void button_fgcolor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialog1.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            colorDialog1.ShowHelp = true;
            // Sets the initial color select to the current text color.
            colorDialog1.Color = Color.Red;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (comboBox_fgcolor.Items.Contains(colorDialog1.Color))
                {
                    comboBox_fgcolor.SelectedIndex = comboBox_fgcolor.FindString(colorDialog1.Color.Name);
                }
            }                
        }

        private void button_bgcolor_Click(object sender, EventArgs e)
        {
            // Keeps the user from selecting a custom color.
            colorDialog1.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            colorDialog1.ShowHelp = true;
            // Sets the initial color select.
            colorDialog1.Color = Color.Beige;

            // Update the text box color if the user clicks OK 
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if (comboBox_bgcolor.Items.Contains(colorDialog1.Color))
                {
                    comboBox_bgcolor.SelectedIndex = comboBox_bgcolor.FindString(colorDialog1.Color.Name);
                }
                else
                {
                    comboBox_bgcolor.Items.Add(colorDialog1.Color);
                }
            }
        }

        private void MoveItemUp()
        {
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
                    var tmpEntry = tempHighlightEntires.Items[indexTo];
                    tempHighlightEntires.Items[indexTo] = tempHighlightEntires.Items[indexFrom];
                    tempHighlightEntires.Items[indexFrom] = tmpEntry;
                }
            }
        }

        private void MoveItemDown()
        {
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
                    var tmpEntry = tempHighlightEntires.Items[indexTo];
                    tempHighlightEntires.Items[indexTo] = tempHighlightEntires.Items[indexFrom];
                    tempHighlightEntires.Items[indexFrom] = tmpEntry;
                }
            }

        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }
            }

            if (e.Alt && e.KeyCode == Keys.Up)
            {
                MoveItemUp();
            }

            if (e.Alt && e.KeyCode == Keys.Down)
            {
                MoveItemDown();
            }

            if (e.KeyCode == Keys.Delete)
            {
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
                button_add_Click(sender, e);
            }
        }

        private void comboBox_fgcolor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_add_Click(sender, e);
            }
        }

        private void comboBox_bgcolor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_add_Click(sender, e);
            }
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            highlightEntries = new HighlightEntires(tempHighlightEntires);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;

            var entry = tempHighlightEntires.Items[listView1.SelectedIndices[0]];

            textBox_string.Text = entry.text;
            comboBox_fgcolor.Text = entry.foreColor.Name;
            comboBox_bgcolor.Text = entry.backColor.Name;
            checkBox_ignorecase.Checked = entry.ignoreCase;
            checkBox_bold.Checked = entry.bold;
            checkBox_italic.Checked = entry.italic;
            checkBox_hide.Checked = entry.hide;
        }
    }
}
