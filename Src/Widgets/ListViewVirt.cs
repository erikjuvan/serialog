using System.Text;
using System.Text.RegularExpressions;

namespace serialog
{
    public class ListViewVirt : ListViewNF
    {
        public List<HighlightEntry> HighlightItems { get; set; } = new List<HighlightEntry>();
        public bool HighlightsDisabled { get; set; } = false;
        public bool HideNonMatchingLines { get; set; } = false;

        public DataLog DataLog { get;  set; }
        private readonly Dictionary<FontStyle, Font> _fontCache = new Dictionary<FontStyle, Font>();

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

        /// <summary>Refresh when new entries were added to the log.</summary>
        public void RefreshEntries()
        {
            this.VirtualListSize = DataLog.Count;
            this.Invalidate();
        }

        private void OnRetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < DataLog.Count)
            {
                var entry = DataLog[e.ItemIndex];
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
            var entry = DataLog[e.ItemIndex];
            string searchString = entry.GetContent();

            // Get highlight style
            if (!GetHighlightStyle(searchString, out Color fore, out Color back, out FontStyle style))
                return;

            // Selection overrides
            if (this.SelectedIndices.Contains(e.ItemIndex))
            {
                back = SystemColors.Highlight;
                fore = SystemColors.HighlightText;
            }

            string line = entry.ToString();
            DrawLine(e.Graphics, e.Bounds, line, fore, back, style);
        }

        private bool GetHighlightStyle(string line, out Color fore, out Color back, out FontStyle style)
        {
            // Defaults
            fore = this.ForeColor;
            back = this.BackColor;
            style = FontStyle.Regular;

            if (HighlightsDisabled)
                return true; // Just use defaults

            var match = FindHighlightMatch(line);

            if (match == null) // No highlight matched
            {
                if (HideNonMatchingLines) return false; // Hide it
                else return true; // Just use defaults
            }

            if (match.Hide)
                return false;

            // Apply style from the match
            fore = match.ForeColor;
            back = match.BackColor;
            if (match.Bold) style |= FontStyle.Bold;
            if (match.Italic) style |= FontStyle.Italic;

            return true;
        }

        public HighlightEntry? FindHighlightMatch(string line)
        {
            foreach (HighlightEntry highlight in HighlightItems)
            {
                if (!highlight.Enabled) continue;

                string haystack = highlight.IgnoreCase ? line.ToLowerInvariant() : line;
                string pattern = highlight.IgnoreCase ? highlight.Text.ToLowerInvariant() : highlight.Text;

                if (Helpers.MatchesPattern(haystack, pattern, highlight.UseRegex))
                    return highlight; // first match wins
            }

            return null;
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

        private void DeleteSelectedEntries()
        {
            var indices = this.SelectedIndices.Cast<int>().OrderByDescending(i => i);

            foreach (var idx in indices)
            {
                if (idx >= 0 && idx < DataLog.Count)
                    DataLog.RemoveAt(idx);
            }

            RefreshEntries();
        }

        private void CopySelectedEntriesToClipboard()
        {
            var sb = new StringBuilder();
            foreach (int idx in this.SelectedIndices)
            {
                if (idx >= 0 && idx < DataLog.Count)
                    sb.AppendLine(DataLog[idx].ToString());
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

        public void ClearView()
        {
            DataLog.Clear();
            RefreshEntries();
        }

        public void Follow()
        {
            if (VirtualListSize > 0)
            {
                this.EnsureVisible(VirtualListSize - 1);
            }
        }
    }
}
