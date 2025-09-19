using System.Text;
using System.Text.RegularExpressions;

namespace serialog
{
    public class ListViewVirt : ListViewNF
    {
        public List<HighlightEntry> HighlightItems { get; set; } = new List<HighlightEntry>();
        public bool HighlightsDisabled { get; set; } = false;
        public bool HideNonMatchingLines { get; set; } = false;

        private readonly Dictionary<FontStyle, Font> _fontCache = new Dictionary<FontStyle, Font>();
        private IReadOnlyList<DataEntry> _entries = Array.Empty<DataEntry>();

        public ListViewVirt()
        {
            this.View = View.Details;
            this.FullRowSelect = true;
            this.GridLines = false;
            this.VirtualMode = true;
            this.VirtualListSize = 0;
            this.OwnerDraw = true;

            this.RetrieveVirtualItem += OnRetrieveVirtualItem;
            this.DrawColumnHeader += OnDrawColumnHeader;
            this.DrawItem += OnDrawItem;
            this.DrawSubItem += OnDrawSubItem;
            this.KeyDown += OnKeyDown;
        }

        /// <summary>Bind this view to a snapshot of your DataLog.</summary>
        public void SetEntries(IReadOnlyList<DataEntry> entries)
        {
            _entries = entries ?? Array.Empty<DataEntry>();
            this.VirtualListSize = _entries.Count;
            this.Invalidate();
        }

        /// <summary>Refresh when new entries were added to the log.</summary>
        public void RefreshEntries()
        {
            this.VirtualListSize = _entries.Count;
            this.Invalidate();
        }

        private void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < _entries.Count)
            {
                var entry = _entries[e.ItemIndex];
                e.Item = new ListViewItem(entry.ToString());
            }
        }

        private void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(e.Header.Text, this.Font, Brushes.Black, e.Bounds);
        }

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Handled in DrawSubItem
        }

        private void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            var entry = _entries[e.ItemIndex];
            string line = entry.ToString();

            // Get highlight style
            if (!TryGetHighlightStyle(line, out Color fore, out Color back, out FontStyle style))
                return;

            // Selection overrides
            if (this.SelectedIndices.Contains(e.ItemIndex))
            {
                back = SystemColors.Highlight;
                fore = SystemColors.HighlightText;
            }

            DrawLine(e.Graphics, e.Bounds, line, fore, back, style);
        }

        private bool TryGetHighlightStyle(string line, out Color fore, out Color back, out FontStyle style)
        {
            fore = this.ForeColor;
            back = this.BackColor;
            style = FontStyle.Regular;

            if (HighlightsDisabled)
                return true;

            foreach (HighlightEntry highlight in HighlightItems)
            {
                if (!highlight.Enabled) continue;

                string haystack = highlight.IgnoreCase ? line.ToLowerInvariant() : line;
                string pattern = highlight.IgnoreCase ? highlight.Text.ToLowerInvariant() : highlight.Text;

                if (MatchesPattern(haystack, pattern, highlight.UseRegex))
                {
                    if (highlight.Hide)
                        return false;

                    fore = highlight.ForeColor;
                    back = highlight.BackColor;
                    if (highlight.Bold) style |= FontStyle.Bold;
                    if (highlight.Italic) style |= FontStyle.Italic;

                    return true; // first match wins
                }
            }

            if (HideNonMatchingLines)
                    return false;

            return true;
        }

        private bool MatchesPattern(string line, string pattern, bool useRegex)
        {
            if (useRegex)
            {
                return Regex.IsMatch(line, pattern);
            }

            return line.Contains(pattern);
        }

        private void DrawLine(Graphics g, Rectangle bounds, string line, Color fore, Color back, FontStyle style)
        {
            using (SolidBrush bg = new SolidBrush(back))
                g.FillRectangle(bg, bounds);

            if (!_fontCache.TryGetValue(style, out Font font))
            {
                font = new Font(this.Font, style);
                _fontCache[style] = font;
            }

            using (SolidBrush brush = new SolidBrush(fore))
                g.DrawString(line, font, brush, bounds);
        }

        private void SelectAll()
        {
            this.BeginUpdate();

            this.SelectedIndices.Clear();
            for (int i = 0; i < this.VirtualListSize; i++)
            {
                this.SelectedIndices.Add(i);
            }

            this.EndUpdate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAll();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedEntries();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelectedEntriesToClipboard();
                e.Handled = true;
            }
        }

        private void DeleteSelectedEntries()
        {
            var indices = this.SelectedIndices.Cast<int>().OrderByDescending(i => i);
            var list = _entries as List<DataEntry>; // only works if bound to a mutable list
            if (list == null) return;

            foreach (var idx in indices)
            {
                if (idx >= 0 && idx < list.Count)
                    list.RemoveAt(idx);
            }

            RefreshEntries();
        }

        private void CopySelectedEntriesToClipboard()
        {
            var sb = new StringBuilder();
            foreach (int idx in this.SelectedIndices)
            {
                if (idx >= 0 && idx < _entries.Count)
                    sb.AppendLine(_entries[idx].ToString());
            }
            if (sb.Length > 0)
                Clipboard.SetText(sb.ToString());
        }

        public void SetFont(Font baseFont)
        {
            // Dispose old fonts
            foreach (var f in _fontCache.Values)
                f.Dispose();
            _fontCache.Clear();

            // Add style variations
            _fontCache[FontStyle.Regular] = new Font(baseFont, FontStyle.Regular);
            _fontCache[FontStyle.Bold] = new Font(baseFont, FontStyle.Bold);
            _fontCache[FontStyle.Italic] = new Font(baseFont, FontStyle.Italic);
            _fontCache[FontStyle.Underline] = new Font(baseFont, FontStyle.Underline);
            _fontCache[FontStyle.Strikeout] = new Font(baseFont, FontStyle.Strikeout);

            Invalidate();
        }
    }
}
