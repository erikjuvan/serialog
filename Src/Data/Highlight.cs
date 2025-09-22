using System.ComponentModel;
using System.Text.Json.Serialization;

namespace serialog
{
    public class HighlightStyle
    {
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color ForeColor { get; set; }
        [JsonConverter(typeof(ColorJsonConverter))]
        public Color BackColor { get; set; }
        public FontStyle FontStyle { get; set; }
        public bool Hide { get; set; }
    }

    public class Highlight : INotifyPropertyChanged
    {
        private bool _enabled = true;
        private bool _useRegex = false;
        private string _text = "";
        private bool _ignoreCase = false;
        private HighlightStyle _style = new HighlightStyle
        {
            ForeColor = Color.Black,
            BackColor = Color.White,
            FontStyle = FontStyle.Regular,
            Hide = false
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Basic properties
        public bool Enabled {
            get => _enabled;
            set { if (_enabled != value) { _enabled = value; OnPropertyChanged(nameof(Enabled)); } }
        }

        public bool UseRegex {
            get => _useRegex;
            set { if (_useRegex != value) { _useRegex = value; OnPropertyChanged(nameof(UseRegex)); } }
        }

        public string Text {
            get => _text;
            set { if (_text != value) { _text = value; OnPropertyChanged(nameof(Text)); } }
        }

        public bool IgnoreCase {
            get => _ignoreCase;
            set { if (_ignoreCase != value) { _ignoreCase = value; OnPropertyChanged(nameof(IgnoreCase)); } }
        }

        // Style property exposing HighlightStyle
        public HighlightStyle Style {
            get => _style;
            set { if (_style != value) { _style = value; OnPropertyChanged(nameof(Style)); } }
        }

        // Convenience properties forwarding to Style
        [JsonIgnore]
        public Color ForeColor {
            get => _style.ForeColor;
            set { if (_style.ForeColor != value) { _style.ForeColor = value; OnPropertyChanged(nameof(ForeColor)); } }
        }

        [JsonIgnore]
        public Color BackColor {
            get => _style.BackColor;
            set { if (_style.BackColor != value) { _style.BackColor = value; OnPropertyChanged(nameof(BackColor)); } }
        }

        [JsonIgnore]
        public FontStyle FontStyle {
            get => _style.FontStyle;
            set { if (_style.FontStyle != value) { _style.FontStyle = value; OnPropertyChanged(nameof(FontStyle)); } }
        }

        [JsonIgnore]
        public bool Hide {
            get => _style.Hide;
            set { if (_style.Hide != value) { _style.Hide = value; OnPropertyChanged(nameof(Hide)); } }
        }

        // Constructors
        public Highlight() { }

        public Highlight(Highlight entry)
        {
            Enabled = entry.Enabled;
            UseRegex = entry.UseRegex;
            Text = entry.Text;
            IgnoreCase = entry.IgnoreCase;
            Style = new HighlightStyle
            {
                ForeColor = entry.ForeColor,
                BackColor = entry.BackColor,
                FontStyle = entry.FontStyle,
                Hide = entry.Hide
            };
        }
    }

    public class Highlights
    {
        private List<Highlight> _items = new List<Highlight>();
        public IReadOnlyList<Highlight> Items => _items.AsReadOnly();

        // Event that fires when the collection or any entry changes
        public event EventHandler? EntriesChanged;
        public int CurrentVersion { get; private set; } = 0;

        private bool _suspendNotifications = false;
        private bool _hasChangesDuringSuspend = false;

        public int Count => _items.Count;

        public Highlights() { }

        public Highlights(Highlights highlights)
        {
            BeginUpdate();
            foreach (Highlight item in highlights._items)
            {
                Add(item); // Use Add so PropertyChanged subscription is set
            }
            EndUpdate();
        }

        /// <summary>SS
        /// Begin suppressing notifications. Call EndUpdate() when done.
        /// </summary>
        public void BeginUpdate()
        {
            _suspendNotifications = true;
            _hasChangesDuringSuspend = false;
        }

        /// <summary>
        /// End update, firing a single EntriesChanged if anything happened.
        /// </summary>
        public void EndUpdate()
        {
            _suspendNotifications = false;
            if (_hasChangesDuringSuspend)
            {
                OnEntriesChanged();
                _hasChangesDuringSuspend = false;
            }
        }

        protected virtual void OnEntriesChanged()
        {
            if (_suspendNotifications)
            {
                _hasChangesDuringSuspend = true;
            }
            else
            {
                CurrentVersion++;
                EntriesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SubscribeToEntry(Highlight entry)
        {
            entry.PropertyChanged += Entry_PropertyChanged;
        }

        private void UnsubscribeFromEntry(Highlight entry)
        {
            entry.PropertyChanged -= Entry_PropertyChanged;
        }

        private void Entry_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Bubble up any entry change as a collection change
            OnEntriesChanged();
        }

        public void Add(Highlight entry)
        {
            var entryCopy = new Highlight(entry);
            _items.Add(entryCopy);
            SubscribeToEntry(entryCopy);
            OnEntriesChanged();
        }

        public void Insert(int index, Highlight entry)
        {
            var entryCopy = new Highlight(entry);
            _items.Insert(index, entryCopy);
            SubscribeToEntry(entryCopy);
            OnEntriesChanged();
        }

        public void RemoveAt(int index)
        {
            UnsubscribeFromEntry(_items[index]);
            _items.RemoveAt(index);
            OnEntriesChanged();
        }

        public void Clear()
        {
            foreach (var entry in _items)
                UnsubscribeFromEntry(entry);
            _items.Clear();
            OnEntriesChanged();
        }

        public Highlight this[int index] {
            get => _items[index];
            set {
                UnsubscribeFromEntry(_items[index]);
                var entryCopy = new Highlight(value);
                _items[index] = entryCopy;
                SubscribeToEntry(entryCopy);
                OnEntriesChanged();
            }
        }

        public void Move(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= _items.Count) return;
            if (newIndex < 0 || newIndex >= _items.Count) return;
            if (oldIndex == newIndex) return;

            var item = _items[oldIndex];
            _items.RemoveAt(oldIndex);
            _items.Insert(newIndex, item);
            OnEntriesChanged(); // notify UI
        }

        public HighlightStyle? GetStyle(string line)
        {
            return FindMatch(line)?.Style;
        }

        public Highlight? FindMatch(string line)
        {
            foreach (Highlight highlight in Items)
            {
                if (!highlight.Enabled) continue;

                string haystack = highlight.IgnoreCase ? line.ToLowerInvariant() : line;
                string pattern = highlight.IgnoreCase ? highlight.Text.ToLowerInvariant() : highlight.Text;

                if (Helpers.MatchesPattern(haystack, pattern, highlight.UseRegex))
                    return highlight; // first match wins
            }

            return null;
        }
    }
}
