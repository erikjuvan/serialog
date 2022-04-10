namespace serialog
{
    public partial class Form3_progressbar : Form
    {
        public Form3_progressbar(string status)
        {
            InitializeComponent();

            label1.Text = status;

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
        }

        public void ProgressBarSetup(int max, int step)
        {
            label1.Invalidate();
            label1.Update();
            label1.Refresh();
            Application.DoEvents();

            progressBar1.Maximum = max;
            progressBar1.Step = step;
        }

        public void ProgressBarIncrement()
        {
            progressBar1.Increment(progressBar1.Step);
        }
    }
}
