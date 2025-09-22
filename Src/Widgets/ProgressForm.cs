public class ProgressForm : Form
{
    public ProgressBar ProgressBar { get; private set; }
    private Label lblStatus;

    public ProgressForm(string title = "Working...")
    {
        Text = title;
        Width = 300;
        Height = 120;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        ControlBox = false;

        lblStatus = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Please wait..."
        };

        ProgressBar = new ProgressBar
        {
            Dock = DockStyle.Bottom,
            Height = 30,
            Style = ProgressBarStyle.Continuous,
            Minimum = 0,
            Maximum = 100,
            Value = 0
        };

        Controls.Add(lblStatus);
        Controls.Add(ProgressBar);
    }

    /// <summary>
    /// Update progress bar and optional status text.
    /// </summary>
    public void UpdateProgress(int percent, string status = null)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => UpdateProgress(percent, status)));
        }
        else
        {
            ProgressBar.Value = Math.Min(ProgressBar.Maximum, percent);
            if (status != null)
            {
                lblStatus.Text = status;
                lblStatus.Refresh();  // force immediate repaint
            }
        }
    }

    // ------------------------------------------------
    // Helper methods for using ProgressForm easily
    // ------------------------------------------------
    public static class Helper
    {
        /// <summary>
        /// Runs a progress form and executes a looped async action.
        /// Automatically handles progress updates and keeps UI responsive.
        /// Use the optional 'onUIThread' flag to run the action on the UI thread.
        /// </summary>
        /// <param name="owner">Parent form for centering the progress window.</param>
        /// <param name="title">Progress window title.</param>
        /// <param name="totalSteps">Number of iterations / steps.</param>
        /// <param name="asyncActionPerStep">Async action to execute per iteration. Receives current step index (0-based).</param>
        /// <param name="onUIThread">Whether to run the action on the UI thread (true for UI updates like ListView).</param>
        public static async Task RunWithProgressAsync(
                Form owner,
                string title,
                int totalSteps,
                Func<int, Task> asyncActionPerStep,
                bool onUIThread = false)
        {
            // Capture the currently active form before showing the progress form
            Form previouslyActive = Form.ActiveForm;

            using (var progressForm = new ProgressForm(title))
            {
                // Center the progress form
                progressForm.StartPosition = FormStartPosition.Manual;
                progressForm.Left = owner.Location.X + owner.Width / 2 - progressForm.Width / 2;
                progressForm.Top = owner.Location.Y + owner.Height / 2 - progressForm.Height / 2;
                progressForm.Owner = owner;

                progressForm.ProgressBar.Minimum = 0;
                progressForm.ProgressBar.Maximum = 100;
                progressForm.ProgressBar.Value = 0;

                progressForm.Show();

                int updateInterval = Math.Max(1, totalSteps / 100); // update ~1% increments

                for (int i = 0; i < totalSteps; i++)
                {
                    if (onUIThread)
                        await asyncActionPerStep(i);
                    else
                        await Task.Run(() => asyncActionPerStep(i));

                    // update only every ~1% or last step
                    if (i % updateInterval == 0 || i == totalSteps - 1)
                    {
                        int percent = (int)((i + 1) * 100.0 / totalSteps);
                        progressForm.UpdateProgress(percent, $"Step {i + 1}/{totalSteps}");
                    }
                }

                progressForm.Close();

                // Restore the form that was active before showing the progress dialog
                if (previouslyActive != null && !previouslyActive.IsDisposed)
                {
                    previouslyActive.BeginInvoke((Action)(() =>
                    {
                        if (previouslyActive.WindowState == FormWindowState.Minimized)
                            previouslyActive.WindowState = FormWindowState.Normal;

                        previouslyActive.Activate();
                        previouslyActive.BringToFront();
                    }));
                }
            }
        }
    }
}