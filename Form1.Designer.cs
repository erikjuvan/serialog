namespace serialog
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listView1 = new serialog.ListViewNF();
            this.column1 = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_hiderest = new System.Windows.Forms.ToolStripMenuItem();
            this.addTimestampsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.comboBox_port = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timer_addtolist = new System.Windows.Forms.Timer(this.components);
            this.comboBox_baud = new System.Windows.Forms.ComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_findnext = new System.Windows.Forms.Button();
            this.button_findprev = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_find = new System.Windows.Forms.TextBox();
            this.button_findall = new System.Windows.Forms.Button();
            this.label_processinfo = new System.Windows.Forms.Label();
            this.timer_updatesysinfo = new System.Windows.Forms.Timer(this.components);
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column1});
            this.listView1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(14, 81);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(1097, 958);
            this.listView1.TabIndex = 12;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Scrolled += new System.EventHandler<System.EventArgs>(this.listView1_Scrolled);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // column1
            // 
            this.column1.Text = "";
            this.column1.Width = 930;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1125, 30);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveSelectedToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(303, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(303, 26);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveSelectedToolStripMenuItem
            // 
            this.saveSelectedToolStripMenuItem.Name = "saveSelectedToolStripMenuItem";
            this.saveSelectedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveSelectedToolStripMenuItem.Size = new System.Drawing.Size(303, 26);
            this.saveSelectedToolStripMenuItem.Text = "Save Selected As...";
            this.saveSelectedToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(303, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.reloadToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click_1);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highlightsToolStripMenuItem,
            this.toolStripMenuItem_hiderest,
            this.addTimestampsToolStripMenuItem,
            this.fontToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // highlightsToolStripMenuItem
            // 
            this.highlightsToolStripMenuItem.Name = "highlightsToolStripMenuItem";
            this.highlightsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.highlightsToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.highlightsToolStripMenuItem.Text = "Highlighting...";
            this.highlightsToolStripMenuItem.Click += new System.EventHandler(this.highlightsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_hiderest
            // 
            this.toolStripMenuItem_hiderest.CheckOnClick = true;
            this.toolStripMenuItem_hiderest.Name = "toolStripMenuItem_hiderest";
            this.toolStripMenuItem_hiderest.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.toolStripMenuItem_hiderest.Size = new System.Drawing.Size(237, 26);
            this.toolStripMenuItem_hiderest.Text = "Hide Rest";
            this.toolStripMenuItem_hiderest.Click += new System.EventHandler(this.toolStripMenuItem_hiderest_Click);
            // 
            // addTimestampsToolStripMenuItem
            // 
            this.addTimestampsToolStripMenuItem.CheckOnClick = true;
            this.addTimestampsToolStripMenuItem.Name = "addTimestampsToolStripMenuItem";
            this.addTimestampsToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.addTimestampsToolStripMenuItem.Text = "Add Timestamps";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // button_run
            // 
            this.button_run.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_run.Location = new System.Drawing.Point(349, 43);
            this.button_run.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(38, 31);
            this.button_run.TabIndex = 3;
            this.button_run.Text = "▶";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // comboBox_port
            // 
            this.comboBox_port.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_port.FormattingEnabled = true;
            this.comboBox_port.Location = new System.Drawing.Point(57, 44);
            this.comboBox_port.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox_port.Name = "comboBox_port";
            this.comboBox_port.Size = new System.Drawing.Size(114, 28);
            this.comboBox_port.TabIndex = 1;
            this.comboBox_port.DropDown += new System.EventHandler(this.comboBox_port_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Baud:";
            // 
            // button_stop
            // 
            this.button_stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_stop.Location = new System.Drawing.Point(393, 43);
            this.button_stop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(38, 31);
            this.button_stop.TabIndex = 5;
            this.button_stop.Text = "■";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(452, 46);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 24);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Follow";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // timer_addtolist
            // 
            this.timer_addtolist.Enabled = true;
            this.timer_addtolist.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBox_baud
            // 
            this.comboBox_baud.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_baud.FormattingEnabled = true;
            this.comboBox_baud.Items.AddRange(new object[] {
            "9600",
            "115200",
            "1000000"});
            this.comboBox_baud.Location = new System.Drawing.Point(227, 44);
            this.comboBox_baud.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox_baud.Name = "comboBox_baud";
            this.comboBox_baud.Size = new System.Drawing.Size(114, 28);
            this.comboBox_baud.TabIndex = 2;
            // 
            // button_findnext
            // 
            this.button_findnext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_findnext.ForeColor = System.Drawing.Color.Black;
            this.button_findnext.Location = new System.Drawing.Point(741, 43);
            this.button_findnext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_findnext.Name = "button_findnext";
            this.button_findnext.Size = new System.Drawing.Size(38, 31);
            this.button_findnext.TabIndex = 9;
            this.button_findnext.Text = ">";
            this.button_findnext.UseVisualStyleBackColor = true;
            this.button_findnext.Click += new System.EventHandler(this.button_findnext_Click);
            // 
            // button_findprev
            // 
            this.button_findprev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_findprev.ForeColor = System.Drawing.Color.Black;
            this.button_findprev.Location = new System.Drawing.Point(698, 43);
            this.button_findprev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_findprev.Name = "button_findprev";
            this.button_findprev.Size = new System.Drawing.Size(38, 31);
            this.button_findprev.TabIndex = 8;
            this.button_findprev.Text = "<";
            this.button_findprev.UseVisualStyleBackColor = true;
            this.button_findprev.Click += new System.EventHandler(this.button_findprev_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(532, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "Find:";
            // 
            // textBox_find
            // 
            this.textBox_find.Location = new System.Drawing.Point(576, 45);
            this.textBox_find.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_find.Name = "textBox_find";
            this.textBox_find.Size = new System.Drawing.Size(114, 27);
            this.textBox_find.TabIndex = 7;
            // 
            // button_findall
            // 
            this.button_findall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_findall.ForeColor = System.Drawing.Color.Black;
            this.button_findall.Location = new System.Drawing.Point(784, 43);
            this.button_findall.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_findall.Name = "button_findall";
            this.button_findall.Size = new System.Drawing.Size(38, 31);
            this.button_findall.TabIndex = 10;
            this.button_findall.Text = "All";
            this.button_findall.UseVisualStyleBackColor = true;
            this.button_findall.Click += new System.EventHandler(this.button_findall_Click);
            // 
            // label_processinfo
            // 
            this.label_processinfo.AutoSize = true;
            this.label_processinfo.Location = new System.Drawing.Point(840, 48);
            this.label_processinfo.Name = "label_processinfo";
            this.label_processinfo.Size = new System.Drawing.Size(50, 20);
            this.label_processinfo.TabIndex = 0;
            this.label_processinfo.Text = "label4";
            // 
            // timer_updatesysinfo
            // 
            this.timer_updatesysinfo.Enabled = true;
            this.timer_updatesysinfo.Interval = 1000;
            this.timer_updatesysinfo.Tick += new System.EventHandler(this.timer_updatesysinfo_Tick);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(237, 26);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 1055);
            this.Controls.Add(this.label_processinfo);
            this.Controls.Add(this.button_findall);
            this.Controls.Add(this.textBox_find);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_findprev);
            this.Controls.Add(this.button_findnext);
            this.Controls.Add(this.comboBox_baud);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_port);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(895, 384);
            this.Name = "Form1";
            this.Text = "serialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Label label1;
        private Button button_run;
        private ComboBox comboBox_port;
        private Label label2;
        private Button button_stop;
        private ToolStripMenuItem highlightsToolStripMenuItem;
        private CheckBox checkBox1;
        private System.Windows.Forms.Timer timer_addtolist;
        private ColumnHeader column1;
        private ListViewNF listView1;
        private ComboBox comboBox_baud;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem saveSelectedToolStripMenuItem;
        private Button button_findnext;
        private Button button_findprev;
        private Label label3;
        private TextBox textBox_find;
        private Button button_findall;
        private ToolStripMenuItem toolStripMenuItem_hiderest;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem clearAllToolStripMenuItem;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private Label label_processinfo;
        private System.Windows.Forms.Timer timer_updatesysinfo;
        private ToolStripMenuItem addTimestampsToolStripMenuItem;
        private FontDialog fontDialog1;
        private ToolStripMenuItem fontToolStripMenuItem;
    }
}