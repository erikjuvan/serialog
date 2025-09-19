namespace serialog
{
    partial class FormView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormView));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideUnhighlightedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewVirt1 = new serialog.ListViewVirt();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideUnhighlightedToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(237, 36);
            // 
            // hideUnhighlightedToolStripMenuItem
            // 
            this.hideUnhighlightedToolStripMenuItem.CheckOnClick = true;
            this.hideUnhighlightedToolStripMenuItem.Name = "hideUnhighlightedToolStripMenuItem";
            this.hideUnhighlightedToolStripMenuItem.Size = new System.Drawing.Size(236, 32);
            this.hideUnhighlightedToolStripMenuItem.Text = "Hide unhighlighted";
            // 
            // listViewVirt1
            // 
            this.listViewVirt1.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewVirt1.DataLog = null;
            this.listViewVirt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewVirt1.FullRowSelect = true;
            this.listViewVirt1.GridLines = true;
            this.listViewVirt1.HideNonMatchingLines = false;
            this.listViewVirt1.HighlightsDisabled = false;
            this.listViewVirt1.Location = new System.Drawing.Point(0, 0);
            this.listViewVirt1.Name = "listViewVirt1";
            this.listViewVirt1.OwnerDraw = true;
            this.listViewVirt1.Size = new System.Drawing.Size(1578, 944);
            this.listViewVirt1.TabIndex = 1;
            this.listViewVirt1.UseCompatibleStateImageBehavior = false;
            this.listViewVirt1.View = System.Windows.Forms.View.Details;
            this.listViewVirt1.VirtualMode = true;
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 944);
            this.Controls.Add(this.listViewVirt1);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormView";
            this.Text = "View";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem hideUnhighlightedToolStripMenuItem;
        private ListViewVirt listViewVirt1;
    }
}