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
    public partial class Form3_Highlights : Form
    {
        // In FormHighlights
        private List<int> _highlightedIndices = new List<int>();
        
        public Form3_Highlights()
        {
            InitializeComponent();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                ListView.SelectedListViewItemCollection selectedItems = listView1.SelectedItems;
                String text = "";
                foreach (ListViewItem item in selectedItems)
                {
                    text += item.Text + "\n";
                }
                if (text.Length > 0)
                    Clipboard.SetText(text);
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    item.Selected = true;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                // Remove all selected items
                while (listView1.SelectedItems.Count > 0)
                {
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                }
            }
        }

        //private void RebuildHighlightedIndices()
        //{
        //    _highlightedIndices.Clear();

        //    for (int i = 0; i < _serialDataList.Count; i++)
        //    {
        //        if (IsHighlighted(_serialDataList[i], out _))
        //            _highlightedIndices.Add(i);
        //    }

        //    listViewHighlights.VirtualListSize = _highlightedIndices.Count;
        //    listViewHighlights.Invalidate();
        //}

        //private void listViewHighlights_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        //{
        //    int sourceIndex = _highlightedIndices[e.ItemIndex];
        //    string line = _serialDataList[sourceIndex];
        //    var item = new ListViewItem(line);

        //    if (IsHighlighted(line, out var style) && style != null)
        //    {
        //        item.ForeColor = style.ForeColor;
        //        item.BackColor = style.BackColor;
        //        if (style.Font != null)
        //            item.Font = style.Font;
        //    }

        //    e.Item = item;
        //}
    }
}
