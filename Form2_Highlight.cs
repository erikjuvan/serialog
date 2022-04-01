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
            tempHighlightSettings = highlightSettings;

            foreach(var item in tempHighlightSettings.highlightEntries)
            {
                AddHighlightEntryToListView(ref listView1, item);
            }            
        }

        static public void InitializeHighlightSettings()
        {
            // Init highlightSettings from a file saved somewhere


        }

        private void button_add_Click(object sender, EventArgs e)
        {
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
            foreach(int index in listView1.SelectedIndices)
            {
                tempHighlightSettings.RemoveAt(index);
                listView1.Items.RemoveAt(index);
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
