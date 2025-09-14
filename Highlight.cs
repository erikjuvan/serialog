using System.ComponentModel;
using System.Text.Json.Serialization;

namespace serialog
{
    public class HighlightEntry : INotifyPropertyChanged
    {
        private bool _enabled = true;
        private string _text = "";
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.White;
        private bool _ignoreCase = false;
        private bool _bold = false;
        private bool _italic = false;
        private bool _hide = false;
        private bool _remove = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Enabled {
            get => _enabled;
            set { if (_enabled != value) { _enabled = value; OnPropertyChanged(nameof(Enabled)); } }
        }

        public string Text {
            get => _text;
            set { if (_text != value) { _text = value; OnPropertyChanged(nameof(Text)); } }
        }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color ForeColor {
            get => _foreColor;
            set { if (_foreColor != value) { _foreColor = value; OnPropertyChanged(nameof(ForeColor)); } }
        }

        [JsonConverter(typeof(ColorJsonConverter))]
        public Color BackColor {
            get => _backColor;
            set { if (_backColor != value) { _backColor = value; OnPropertyChanged(nameof(BackColor)); } }
        }

        public bool IgnoreCase {
            get => _ignoreCase;
            set { if (_ignoreCase != value) { _ignoreCase = value; OnPropertyChanged(nameof(IgnoreCase)); } }
        }

        public bool Bold {
            get => _bold;
            set { if (_bold != value) { _bold = value; OnPropertyChanged(nameof(Bold)); } }
        }

        public bool Italic {
            get => _italic;
            set { if (_italic != value) { _italic = value; OnPropertyChanged(nameof(Italic)); } }
        }

        public bool Hide {
            get => _hide;
            set { if (_hide != value) { _hide = value; OnPropertyChanged(nameof(Hide)); } }
        }

        public bool Remove {
            get => _remove;
            set { if (_remove != value) { _remove = value; OnPropertyChanged(nameof(Remove)); } }
        }

        // Constructors
        public HighlightEntry() { }

        public HighlightEntry(HighlightEntry entry)
        {
            Enabled = entry.Enabled;
            Text = entry.Text;
            ForeColor = entry.ForeColor;
            BackColor = entry.BackColor;
            IgnoreCase = entry.IgnoreCase;
            Bold = entry.Bold;
            Italic = entry.Italic;
            Hide = entry.Hide;
            Remove = entry.Remove;
        }

        public HighlightEntry(string text)
        {
            Text = text;
        }

        public HighlightEntry(ListViewItem listViewItem)
        {
            Enabled = listViewItem.Checked;
            Text = listViewItem.Text;
            ForeColor = listViewItem.ForeColor;
            BackColor = listViewItem.BackColor;
            if (listViewItem.SubItems.Count == 3)
            {
                IgnoreCase = listViewItem.SubItems[0].Text.Contains("*");
                Hide = listViewItem.SubItems[1].Text.Contains("*");
                Remove = listViewItem.SubItems[2].Text.Contains("*");
            }
            Bold = listViewItem.Font.Style.HasFlag(FontStyle.Bold);
            Italic = listViewItem.Font.Style.HasFlag(FontStyle.Italic);
        }
    }

    public class HighlightEntries
    {
        private List<HighlightEntry> items = new List<HighlightEntry>();

        // Event that fires when the collection or any entry changes
        public event EventHandler? EntriesChanged;

        public HighlightEntries() { }

        public HighlightEntries(HighlightEntries highlightEntries)
        {
            foreach (HighlightEntry item in highlightEntries.items)
            {
                Add(item); // Use Add so PropertyChanged subscription is set
            }
        }

        protected virtual void OnEntriesChanged()
        {
            EntriesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SubscribeToEntry(HighlightEntry entry)
        {
            entry.PropertyChanged += Entry_PropertyChanged;
        }

        private void UnsubscribeFromEntry(HighlightEntry entry)
        {
            entry.PropertyChanged -= Entry_PropertyChanged;
        }

        private void Entry_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Bubble up any entry change as a collection change
            OnEntriesChanged();
        }

        public void Add(HighlightEntry highlightEntry)
        {
            var entryCopy = new HighlightEntry(highlightEntry);
            items.Add(entryCopy);
            SubscribeToEntry(entryCopy);
            OnEntriesChanged();
        }

        public void Insert(int index, HighlightEntry highlightEntry)
        {
            var entryCopy = new HighlightEntry(highlightEntry);
            items.Insert(index, entryCopy);
            SubscribeToEntry(entryCopy);
            OnEntriesChanged();
        }

        public void Insert(int index, ListViewItem listViewItem)
        {
            var entryCopy = new HighlightEntry(listViewItem);
            items.Insert(index, entryCopy);
            SubscribeToEntry(entryCopy);
            OnEntriesChanged();
        }

        public void RemoveAt(int index)
        {
            UnsubscribeFromEntry(items[index]);
            items.RemoveAt(index);
            OnEntriesChanged();
        }

        public void Clear()
        {
            foreach (var entry in items)
                UnsubscribeFromEntry(entry);
            items.Clear();
            OnEntriesChanged();
        }

        public List<HighlightEntry> Items => items;

        public HighlightEntry this[int index] {
            get => items[index];
            set {
                UnsubscribeFromEntry(items[index]);
                var entryCopy = new HighlightEntry(value);
                items[index] = entryCopy;
                SubscribeToEntry(entryCopy);
                OnEntriesChanged();
            }
        }

        public int Count => items.Count;
    }
}
