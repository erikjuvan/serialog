using System.Text.RegularExpressions;

namespace serialog
{
    public partial class FormView : Form
    {
        private readonly DataLog _dataLog;
        private DataLog _dataLogList = new DataLog();

        public FormView(DataLog dataLog)
        {
            InitializeComponent();

            _dataLog = dataLog;
            listView1.DataLog = _dataLogList;
        }

        private void FormView_Load(object sender, EventArgs e)
        {
            FitListviewToWidth();
        }

        private void FormView_Resize(object sender, EventArgs e)
        {
            FitListviewToWidth();
        }

        private void FitListviewToWidth()
        {
            // Optionally subtract a bit for the form borders and padding
            int availableWidth = listView1.Width - 10 - SystemInformation.VerticalScrollBarWidth;
            // For single-column ListView, fill entire width
            listView1.Columns[0].Width = availableWidth;
        }

        public void UpdateList()
        {
            _dataLogList.Clear();

            for (int i = 0; i < _dataLog.Count; i++)
            {
                try
                {
                    if (Regex.IsMatch(_dataLog[i].Content, textBox1.Text, RegexOptions.IgnoreCase))
                    {
                        _dataLogList.Add(_dataLog[i]);
                    }
                }
                catch (ArgumentException)
                {
                }
            }

            listView1.VirtualListSize = _dataLogList.Count;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // prevents "ding" sound
                UpdateList();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateList();
        }
    }
}
