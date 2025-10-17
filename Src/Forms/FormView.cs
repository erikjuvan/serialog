using System.Text.RegularExpressions;

namespace serialog
{
    public partial class FormView : Form
    {
        private Form1 _parentForm;
        private readonly DataLog _dataLog;
        private DataLog _dataLogList = new DataLog();
        private List<int> _dataLogListIndicies = new List<int>();

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
            FitTextboxes();
        }

        private void FormView_Resize(object sender, EventArgs e)
        {
            FitListviewToWidth();
            FitTextboxes();
        }

        private void FormView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                textBoxSearch.Select();
            }

            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    listView1.FindPrev(textBoxSearch.Text);
                }
                else
                {
                    listView1.FindNext(textBoxSearch.Text);
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

        private void FitTextboxes()
        {
            int margin = 10;
            int totalWidth = this.ClientSize.Width - (3 * margin);
            int halfWidth = totalWidth / 2;

            textBoxMatch.Width = halfWidth;
            textBoxSearch.Width = halfWidth;
            textBoxMatch.Left = margin;
            textBoxSearch.Left = textBoxMatch.Right + margin;
        }

        public void UpdateList()
        {
            _dataLogList.Clear();
            _dataLogListIndicies.Clear();

            for (int i = 0; i < _dataLog.Count; i++)
            {
                try
                {
                    if (Helpers.MatchesPattern(_dataLog[i].ToString(), textBoxMatch.Text, ListViewVirtExtensions.MatchCase, ListViewVirtExtensions.UseRegex))
                    {
                        _dataLogList.Add(_dataLog[i]);
                        _dataLogListIndicies.Add(i);
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

            if (filteredIndex < 0 || filteredIndex >= _dataLogListIndicies.Count)
                return;

            // Get the corresponding index in the original list
            int originalIndex = _dataLogListIndicies[filteredIndex];

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
            listView1.FindLive(textBoxSearch.Text);
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift)
                {
                    listView1.FindPrev(textBoxSearch.Text);
                }
                else
                {
                    listView1.FindNext(textBoxSearch.Text);
                }
            }
        }
    }
}
