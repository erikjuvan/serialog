using System.Text;
using System.Runtime.InteropServices;

namespace serialog
{
    public class ListViewVirt : ListViewNF
    {
        public bool HighlightsDisabled { get; set; } = false;
        public bool HideNonMatchingLines { get; set; } = false;

        public DataLog DataLog { get;  set; }
        private readonly Dictionary<FontStyle, Font> _fontCache = new Dictionary<FontStyle, Font>();

        // For scrolling
        private int scrollAccumulatedDelta = 0;
        private System.Windows.Forms.Timer scrollTimer;

        public ListViewVirt()
        {
            this.View = View.Details;
            this.FullRowSelect = false;
            this.GridLines = false;
            this.VirtualMode = true;
            this.VirtualListSize = 0;
            this.OwnerDraw = true;

            this.RetrieveVirtualItem += OnRetrieveVirtualItem;
            this.DrawItem += OnDrawItem;
            this.DrawSubItem += OnDrawSubItem;
            this.KeyDown += OnKeyDown;
            //this.MouseWheel += OnMouseWheel;
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

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Handled in DrawSubItem
        }

        private void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            var entry = DataLog[e.ItemIndex];

            // Compute or reuse highlight
            entry.EnsureHighlightUpToDate();

            // Default
            HighlightStyle style = new HighlightStyle
            {
                ForeColor = Color.FromKnownColor(KnownColor.WindowText),
                BackColor = Color.FromKnownColor(KnownColor.Window),
                FontStyle = FontStyle.Regular,
                Hide = false
            };
            
            if (!HighlightsDisabled)
            {
                var effective = entry.Style;

                if (effective == null) // No highlight match
                {
                    if (HideNonMatchingLines)
                        return; // skip drawing completely
                }
                else
                {
                    style = effective.Clone();
                }

                if (style.Hide)
                    return;
            }

            // Selection overrides
            if (SelectedIndices.Contains(e.ItemIndex))
            {
                style.ForeColor = SystemColors.HighlightText;
                style.BackColor = SystemColors.Highlight;
            }

            string line = entry.ToString();
            DrawLine(e.Graphics, e.Bounds, line, style.ForeColor, style.BackColor, style.FontStyle);
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            scrollAccumulatedDelta += e.Delta;
            if (scrollTimer == null)
            {
                scrollTimer = new System.Windows.Forms.Timer();
                scrollTimer.Interval = 15; // 15ms per “scroll frame”
                scrollTimer.Tick += (s, args) =>
                {
                    scrollTimer.Stop();

                    int lines = scrollAccumulatedDelta / 120;
                    scrollAccumulatedDelta = 0;

                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                        lines *= 30;

                    int newTopIndex = Math.Max(0, Math.Min(Items.Count - 1, TopItem.Index - lines));
                    if (newTopIndex != TopItem.Index)
                        TopItem = Items[newTopIndex];
                };
            }

            scrollTimer.Stop();
            scrollTimer.Start();
        }
    }
}
