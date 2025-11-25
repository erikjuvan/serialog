using System.Text.RegularExpressions;

namespace serialog
{
    public partial class FormView : Form
    {
        private Form1 _parentForm;
        private readonly DataLog _dataLog;
        private DataLog _dataLogList = new DataLog();
        public List<int> DataLogListIndicies = new List<int>();

        public FormView(Form1 parent, DataLog dataLog)
        {
            InitializeComponent();

            _parentForm = parent;
            _dataLog = dataLog;
            listView1.DataLog = _dataLogList;
        }

        private void FormView_Load(object sender, EventArgs e)
        {
            FitListviewToWidth();
            FitCombos();
        }

        private void FormView_Resize(object sender, EventArgs e)
        {
            FitListviewToWidth();
            FitCombos();
        }

        private void FormView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                comboSearch.Select();
            }

            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    listView1.FindPrev(comboSearch.Text);
                }
                else
                {
                    listView1.FindNext(comboSearch.Text);
                }
            }
        }

        private void FitListviewToWidth()
        {
            // Optionally subtract a bit for the form borders and padding
            int availableWidth = listView1.Width - 10 - SystemInformation.VerticalScrollBarWidth;
            // For single-column ListView, fill entire width
            listView1.Columns[0].Width = availableWidth;
        }

        private void FitCombos()
        {
            int margin = 10;
            int totalWidth = this.ClientSize.Width - (3 * margin);
            int halfWidth = totalWidth / 2;

            comboMatch.Width = halfWidth;
            comboSearch.Width = halfWidth;

            comboMatch.Left = margin;
            comboSearch.Left = comboMatch.Right + margin;
        }

        public void UpdateList()
        {
            _dataLogList.Clear();
            DataLogListIndicies.Clear();

            for (int i = 0; i < _dataLog.Count; i++)
            {
                try
                {
                    if (Helpers.MatchesPattern(_dataLog[i].ToString(), comboMatch.Text, ListViewVirtExtensions.MatchCase, ListViewVirtExtensions.UseRegex))
                    {
                        _dataLogList.Add(_dataLog[i]);
                        DataLogListIndicies.Add(i);
                    }
                }
                catch (ArgumentException)
                {
                }
            }

            listView1.VirtualListSize = _dataLogList.Count;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            // Get the selected index in the filtered list
            int filteredIndex = listView1.SelectedIndices[0];

            if (filteredIndex < 0 || filteredIndex >= DataLogListIndicies.Count)
                return;

            // Get the corresponding index in the original list
            int originalIndex = DataLogListIndicies[filteredIndex];

            // Example: select this index in another ListView (on another form)
            _parentForm.listView1.SelectedIndices.Clear();
            _parentForm.listView1.SelectedIndices.Add(originalIndex);
            _parentForm.listView1.EnsureVisible(originalIndex);
        }

        private void textBoxMatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // prevents "ding" sound
                UpdateList();
            }
        }

        private void textBoxMatch_TextChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            listView1.FindLive(comboSearch.Text);
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    listView1.FindPrev(comboSearch.Text);
                }
                else
                {
                    listView1.FindNext(comboSearch.Text);
                }
            }
        }

        private void combo_StoreOnEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var cb = (ComboBox)sender;

                string text = cb.Text.Trim();
                if (text.Length == 0)
                    return;

                // Only add if it's not already in history
                if (!cb.Items.Contains(text))
                    cb.Items.Add(text);

                e.SuppressKeyPress = true; // Prevent system beep
            }
        }

        private void combo_RemoveSelectedOnDelete(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var cb = (ComboBox)sender;

                // Only act when dropdown is open and there's a highlighted item
                if (cb.DroppedDown && cb.SelectedIndex >= 0)
                {
                    // Save the current text before modifying the list
                    string currentText = cb.Text;

                    // Remove the selected (highlighted) item
                    cb.Items.RemoveAt(cb.SelectedIndex);

                    // Deselect any item
                    //cb.SelectedIndex = -1; // Doesn't seem like it is needed

                    // Restore the user's text
                    cb.Text = currentText;

                    // Prevent beep and default delete behavior
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
        }

        private void comboMatch_KeyDown(object sender, KeyEventArgs e)
        {
            combo_StoreOnEnter(sender, e);
            combo_RemoveSelectedOnDelete(sender, e);
        }

        private void comboSearch_KeyDown(object sender, KeyEventArgs e)
        {
            combo_StoreOnEnter(sender, e);
            combo_RemoveSelectedOnDelete(sender, e);
        }
    }
}
