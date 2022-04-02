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
        static public HighlightSettings highlightSettings = new HighlightSettings();
        private HighlightSettings tempHighlightSettings = new HighlightSettings();

        private void AddHighlightEntryToListView(ref ListView listView, HighlightEntry highlightEntry)
        {
            listView.Items.Add(highlightEntry.text);
            listView.Items[listView1.Items.Count - 1].ForeColor = highlightEntry.foreColor;
            listView.Items[listView1.Items.Count - 1].BackColor = highlightEntry.backColor;
        }

        public Form2_Highlight()
        {
            InitializeComponent();

            // copy to temp
            foreach (var item in highlightSettings.highlightEntries)
                tempHighlightSettings.Add(item);

            foreach (var item in tempHighlightSettings.highlightEntries)
            {
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
            if (e.KeyCode == Keys.Delete)
            {
                button_delete_Click(sender, e);
            }

            if (e.KeyCode == Keys.Enter)
            {
                button_add_Click(sender, e);
            }

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
            if (textBox_string.Text.Length == 0)
                return;

            Color bgcol;
            if (comboBox_bgcolor.Text.Length == 0)
                bgcol = Color.White;
            else
                bgcol = Color.FromName(comboBox_bgcolor.Text);

            HighlightEntry highlightEntry = new HighlightEntry(textBox_string.Text, 
                Color.FromName(comboBox_fgcolor.Text), bgcol, 
                checkBox_bold.Checked, checkBox_ignorecase.Checked);

            tempHighlightSettings.Add(highlightEntry);
            AddHighlightEntryToListView(ref listView1, highlightEntry);
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            highlightSettings = tempHighlightSettings;
            this.Close();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            while (listView1.SelectedIndices.Count > 0)
            {
                int idx = listView1.SelectedIndices[listView1.SelectedIndices.Count - 1];
                tempHighlightSettings.RemoveAt(idx);
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
            }
        }
    }


    public class HighlightEntry
    {
        public string text;
        public Color foreColor = Color.Black;
        public Color backColor = Color.White;
        public bool bold = false;
        public bool italic = false;
        public bool caseSensitive = true;

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
        public HighlightEntry(string text, Color foreColor, Color backColor, bool bold, bool caseSensitive)
        {
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.bold = bold;
            this.caseSensitive = caseSensitive;
        }
    }

    public class HighlightSettings
    {
        public List<HighlightEntry> highlightEntries = new List<HighlightEntry>();

        public void Add(HighlightEntry highlightEntry)
        {
            highlightEntries.Add(highlightEntry);
        }

        public void RemoveAt(int idx)
        {
            highlightEntries.RemoveAt(idx);
        }

        public void Clear()
        {
            highlightEntries.Clear();
        }
    }

}
