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
    public partial class FormView : Form
    {
        public FormView()
        {
            InitializeComponent();
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
    }
}
