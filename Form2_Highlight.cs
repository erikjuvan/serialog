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

        public Form2_Highlight()
        {
            InitializeComponent();
        }

        static public void InitializeHighlightSettings()
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }


    public class HighlightEntry
    {
        public string text;
        public Color foreColor = Color.White;
        public Color backColor = Color.Black;
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
            this.foreColor = Color.White;
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

        public void Remove(HighlightEntry highlightEntry)
        {
            highlightEntries.Remove(highlightEntry);
        }

        public void Clear()
        {
            highlightEntries.Clear();
        }
    }

}
