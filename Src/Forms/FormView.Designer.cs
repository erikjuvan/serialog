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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideUnhighlightedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new serialog.ListViewVirt();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.comboMatch = new System.Windows.Forms.ComboBox();
            this.comboSearch = new System.Windows.Forms.ComboBox();
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
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.DataLog = null;
            this.listView1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideNonMatchingLines = false;
            this.listView1.HighlightsDisabled = false;
            this.listView1.Location = new System.Drawing.Point(12, 46);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(1554, 886);
            this.listView1.TabIndex = 2;
            this.listView1.TabStop = false;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // comboMatch
            // 
            this.comboMatch.FormattingEnabled = true;
            this.comboMatch.Location = new System.Drawing.Point(12, 7);
            this.comboMatch.Name = "comboMatch";
            this.comboMatch.Size = new System.Drawing.Size(783, 33);
            this.comboMatch.TabIndex = 4;
            this.comboMatch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboMatch_KeyDown);
            // 
            // comboSearch
            // 
            this.comboSearch.FormattingEnabled = true;
            this.comboSearch.Location = new System.Drawing.Point(801, 7);
            this.comboSearch.Name = "comboSearch";
            this.comboSearch.Size = new System.Drawing.Size(765, 33);
            this.comboSearch.TabIndex = 5;
            this.comboSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboSearch_KeyDown);
            // 
            // FormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 944);
            this.Controls.Add(this.comboSearch);
            this.Controls.Add(this.comboMatch);
            this.Controls.Add(this.listView1);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FormView";
            this.Text = "View (3)";
            this.Load += new System.EventHandler(this.FormView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormView_KeyDown);
            this.Resize += new System.EventHandler(this.FormView_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem hideUnhighlightedToolStripMenuItem;
        private ColumnHeader columnHeader1;
        public ListViewVirt listView1;
        private ComboBox comboMatch;
        private ComboBox comboSearch;
    }
}