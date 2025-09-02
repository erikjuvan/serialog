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
    public partial class Form4_Highlights : Form
    {
        public Form4_Highlights()
        {
            InitializeComponent();
        }

        private void Form4_Highlights_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
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
    }
}
