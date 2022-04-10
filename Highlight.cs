namespace serialog
{
    public class HighlightEntry
    {
        public bool enabled = true;
        public string text;
        public Color foreColor = Color.Black;
        public Color backColor = Color.White;
        public bool ignoreCase = false;
        public bool bold = false;
        public bool italic = false;
        public bool hide = false;
        public bool remove = false;

        public HighlightEntry(string text)
        {
            this.enabled = true;
            this.text = text;
        }

        public HighlightEntry(bool enabled, string text)
        {
            this.enabled = enabled;
            this.text = text;
        }
        public HighlightEntry(bool enabled, string text, Color foreColor)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
        }
        public HighlightEntry(bool enabled, string text, Color foreColor, Color backColor)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
        }
        public HighlightEntry(bool enabled, string text, Color foreColor, Color backColor, bool ignoreCase, bool bold)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.bold = bold;
            this.ignoreCase = ignoreCase;
        }

        public HighlightEntry(bool enabled, string text, Color foreColor, Color backColor, bool ignoreCase, bool bold, bool hide)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.ignoreCase = ignoreCase;
            this.bold = bold;
            this.hide = hide;
        }

        public HighlightEntry(bool enabled, string text, Color foreColor, Color backColor, bool ignoreCase, bool bold, bool italic, bool hide)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.ignoreCase = ignoreCase;
            this.bold = bold;
            this.italic = italic;
            this.hide = hide;
        }

        public HighlightEntry(bool enabled, string text, Color foreColor, Color backColor, bool ignoreCase, bool bold, bool italic, bool hide, bool remove)
        {
            this.enabled = enabled;
            this.text = text;
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.ignoreCase = ignoreCase;
            this.bold = bold;
            this.italic = italic;
            this.hide = hide;
            this.remove = remove;
        }

        public HighlightEntry(ListViewItem listViewItem)
        {
            enabled = listViewItem.Checked;
            text = listViewItem.Text;
            foreColor = listViewItem.ForeColor;
            backColor = listViewItem.BackColor;
            if (listViewItem.SubItems.Count == 3)
            {
                if (listViewItem.SubItems[0].Text.Contains("*"))
                {
                    ignoreCase = true;
                }

                if (listViewItem.SubItems[1].Text.Contains("*"))
                {
                    hide = true;
                }

                if (listViewItem.SubItems[2].Text.Contains("*"))
                {
                    remove = true;
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

    public class HighlightEntries
    {
        private List<HighlightEntry> items = new List<HighlightEntry>();

        public HighlightEntries()
        {
            items = new List<HighlightEntry>();
        }

        public HighlightEntries(HighlightEntries highlightEntires)
        {
            foreach (HighlightEntry item in highlightEntires.items)
            {
                items.Add(item);
            }
        }

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
