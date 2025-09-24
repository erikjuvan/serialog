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

        // Smoother mouse scrolling
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_VSCROLL = 0x0115;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;

        public ListViewVirt()
        {
            this.View = View.Details;
            this.FullRowSelect = false;
            this.GridLines = false;
            this.VirtualMode = true;
            this.VirtualListSize = 0;
            this.OwnerDraw = true;

            this.RetrieveVirtualItem += OnRetrieveVirtualItem;
            this.DrawColumnHeader += OnDrawColumnHeader;
            this.DrawItem += OnDrawItem;
            this.DrawSubItem += OnDrawSubItem;
            this.KeyDown += OnKeyDown;
            this.MouseWheel += OnMouseWheel;
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

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            int linesToMove = e.Delta / 120;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
                linesToMove *= 15;

            for (int i = 0; i < Math.Abs(linesToMove); i++)
            {
                int command = linesToMove > 0 ? SB_LINEUP : SB_LINEDOWN;
                SendMessage(Handle, WM_VSCROLL, command, 0);
            }

            ((HandledMouseEventArgs)e).Handled = true;
        }
    }
}
