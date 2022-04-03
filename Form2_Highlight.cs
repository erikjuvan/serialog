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

        public Form2_Highlight()
        {
            InitializeComponent();

            // copy to temp
            foreach (var item in highlightEntries.Items)
            {
                tempHighlightEntires.Add(item);
                AddHighlightEntryToListView(ref listView1, item);
            }                

            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor knowColor in colors)
            {
                Color color = Color.FromKnownColor(knowColor);
                comboBox_bgcolor.Items.Add(color);
                comboBox_fgcolor.Items.Add(color);
            }
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

            HighlightEntry highlightEntry = new HighlightEntry(textBox_string.Text, 
                Color.FromName(comboBox_fgcolor.Text), bgcol,
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
            if (MessageBox.Show("Save changes?", "Save?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                highlightEntries = tempHighlightEntires;
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
    }


    public class HighlightEntry
    {
        public string text;
        public Color foreColor = Color.Black;
        public Color backColor = Color.White;
        public bool ignoreCase = false;
        public bool bold = false;
        public bool italic = false;
        public bool hide = false;

        public HighlightEntry(string text)
        {
            this.text = text;
        }
        public HighlightEntry(string text, Color foreColor)
        {
            this.text = text;
            this.foreColor = foreColor;
        }
        public HighlightEntry(string text, Color foreColor, Color backColor)
        {
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
        }
        public HighlightEntry(string text, Color foreColor, Color backColor, bool ignoreCase, bool bold)
        {
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.bold = bold;
            this.ignoreCase = ignoreCase;
        }

        public HighlightEntry(string text, Color foreColor, Color backColor, bool ignoreCase, bool bold, bool hide)
        {
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.ignoreCase = ignoreCase;
            this.bold = bold;
            this.hide = hide;
        }

        public HighlightEntry(string text, Color foreColor, Color backColor, bool ignoreCase, bool bold, bool italic, bool hide)
        {
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.ignoreCase = ignoreCase;
            this.bold = bold;
            this.italic = italic;
            this.hide = hide;
        }

        public HighlightEntry(ListViewItem listViewItem)
        {
            text = listViewItem.Text;
            foreColor = listViewItem.ForeColor;
            backColor = listViewItem.BackColor;
            if (listViewItem.SubItems.Count == 2)
            {
                if (listViewItem.SubItems[0].Text.Contains("*"))
                {
                    ignoreCase = true;
                }

                if (listViewItem.SubItems[1].Text.Contains("*"))
                {
                    hide = true;
                }
            }
            if (listViewItem.Font.Style == FontStyle.Bold)
            {
                bold = true;
            }
            if (listViewItem.Font.Style == FontStyle.Italic)
            {
                italic = true;
            }
        }
    }

    public class HighlightEntires
    {
        private List<HighlightEntry> items = new List<HighlightEntry>();

        public void Add(HighlightEntry highlightEntry)
        {
            items.Add(highlightEntry);
        }
        
        public void Insert(int index, HighlightEntry highlightEntry)
        {
            items.Insert(index, highlightEntry);
        }

        public void Insert(int index, ListViewItem listViewItem)
        {
            items.Insert(index, new HighlightEntry(listViewItem));
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public List<HighlightEntry> Items
        {
            get { return items; }
        }

        public void Clear()
        {
            items.Clear();
        }
    }

}
