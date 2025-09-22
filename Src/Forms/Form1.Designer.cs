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
            this.listView1 = new serialog.ListViewVirt();
            this.column1 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomRowContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.formHighlightContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideUnhighlightedContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableHighlightsContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSerialAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formHighlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideUnhighlightedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableHighlightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addStartStopTimestampToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsFolderChangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsFolderResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonPrintableCharsAsHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsAddViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsSerialSendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.button_run = new System.Windows.Forms.Button();
            this.comboBox_port = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_stop = new System.Windows.Forms.Button();
            this.checkBox_follow = new System.Windows.Forms.CheckBox();
            this.timer_addtolist = new System.Windows.Forms.Timer(this.components);
            this.comboBox_baud = new System.Windows.Forms.ComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_findnext = new System.Windows.Forms.Button();
            this.button_findprev = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_find = new System.Windows.Forms.TextBox();
            this.button_findall = new System.Windows.Forms.Button();
            this.timer_updatesysinfo = new System.Windows.Forms.Timer(this.components);
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.button_prev_highlight = new System.Windows.Forms.Button();
            this.button_next_highlight = new System.Windows.Forms.Button();
            this.checkBoxRegex = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
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
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.DataLog = null;
            this.listView1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.GridLines = true;
            this.listView1.HideNonMatchingLines = false;
            this.listView1.HighlightsDisabled = false;
            this.listView1.Location = new System.Drawing.Point(17, 102);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(1304, 1479);
            this.listView1.TabIndex = 12;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.Scrolled += new System.EventHandler<System.EventArgs>(this.listView1_Scrolled);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // column1
            // 
            this.column1.Text = "";
            this.column1.Width = 2000;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllContextMenuItem,
            this.clearAllContextMenuItem,
            this.addCustomRowContextMenuItem,
            this.toolStripMenuItem2,
            this.formHighlightContextMenuItem,
            this.hideUnhighlightedContextMenuItem,
            this.disableHighlightsContextMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(259, 202);
            // 
            // selectAllContextMenuItem
            // 
            this.selectAllContextMenuItem.Name = "selectAllContextMenuItem";
            this.selectAllContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.selectAllContextMenuItem.Text = "Select All";
            this.selectAllContextMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // clearAllContextMenuItem
            // 
            this.clearAllContextMenuItem.Name = "clearAllContextMenuItem";
            this.clearAllContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.clearAllContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.clearAllContextMenuItem.Text = "Clear All";
            this.clearAllContextMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // addCustomRowContextMenuItem
            // 
            this.addCustomRowContextMenuItem.Name = "addCustomRowContextMenuItem";
            this.addCustomRowContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.addCustomRowContextMenuItem.Text = "Add Custom Row...";
            this.addCustomRowContextMenuItem.Click += new System.EventHandler(this.addCustomRowToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
            // 
            // formHighlightContextMenuItem
            // 
            this.formHighlightContextMenuItem.Name = "formHighlightContextMenuItem";
            this.formHighlightContextMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.formHighlightContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.formHighlightContextMenuItem.Text = "Highlighting...";
            this.formHighlightContextMenuItem.Click += new System.EventHandler(this.formHighlightToolStripMenuItem_Click);
            // 
            // hideUnhighlightedContextMenuItem
            // 
            this.hideUnhighlightedContextMenuItem.Name = "hideUnhighlightedContextMenuItem";
            this.hideUnhighlightedContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.hideUnhighlightedContextMenuItem.Text = "Hide Unhighlighted";
            this.hideUnhighlightedContextMenuItem.Click += new System.EventHandler(this.hideUnhighlightedToolStripMenuItem_Click);
            // 
            // disableHighlightsContextMenuItem
            // 
            this.disableHighlightsContextMenuItem.Name = "disableHighlightsContextMenuItem";
            this.disableHighlightsContextMenuItem.Size = new System.Drawing.Size(258, 32);
            this.disableHighlightsContextMenuItem.Text = "Disable Highlighting";
            this.disableHighlightsContextMenuItem.Click += new System.EventHandler(this.disableHighlightsToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1334, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveSelectedToolStripMenuItem,
            this.saveSerialAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(368, 34);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(368, 34);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveSelectedToolStripMenuItem
            // 
            this.saveSelectedToolStripMenuItem.Name = "saveSelectedToolStripMenuItem";
            this.saveSelectedToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveSelectedToolStripMenuItem.Size = new System.Drawing.Size(368, 34);
            this.saveSelectedToolStripMenuItem.Text = "Save Selected As...";
            this.saveSelectedToolStripMenuItem.Click += new System.EventHandler(this.saveSelectedToolStripMenuItem_Click);
            // 
            // saveSerialAsToolStripMenuItem
            // 
            this.saveSerialAsToolStripMenuItem.Name = "saveSerialAsToolStripMenuItem";
            this.saveSerialAsToolStripMenuItem.Size = new System.Drawing.Size(368, 34);
            this.saveSerialAsToolStripMenuItem.Text = "Save \"Serial\" As ...";
            this.saveSerialAsToolStripMenuItem.Click += new System.EventHandler(this.saveSerialAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(368, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.clearAllToolStripMenuItem,
            this.addCustomRowToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(58, 29);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(266, 34);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(266, 34);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // addCustomRowToolStripMenuItem
            // 
            this.addCustomRowToolStripMenuItem.Name = "addCustomRowToolStripMenuItem";
            this.addCustomRowToolStripMenuItem.Size = new System.Drawing.Size(266, 34);
            this.addCustomRowToolStripMenuItem.Text = "Add Custom Row...";
            this.addCustomRowToolStripMenuItem.Click += new System.EventHandler(this.addCustomRowToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formHighlightToolStripMenuItem,
            this.hideUnhighlightedToolStripMenuItem,
            this.disableHighlightsToolStripMenuItem,
            this.addStartStopTimestampToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.settingsFolderToolStripMenuItem,
            this.nonPrintableCharsAsHexToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(92, 29);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // formHighlightToolStripMenuItem
            // 
            this.formHighlightToolStripMenuItem.Name = "formHighlightToolStripMenuItem";
            this.formHighlightToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.formHighlightToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.formHighlightToolStripMenuItem.Text = "Highlight...";
            this.formHighlightToolStripMenuItem.Click += new System.EventHandler(this.formHighlightToolStripMenuItem_Click);
            // 
            // hideUnhighlightedToolStripMenuItem
            // 
            this.hideUnhighlightedToolStripMenuItem.Name = "hideUnhighlightedToolStripMenuItem";
            this.hideUnhighlightedToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.hideUnhighlightedToolStripMenuItem.Text = "Hide Unhighlighted";
            this.hideUnhighlightedToolStripMenuItem.Click += new System.EventHandler(this.hideUnhighlightedToolStripMenuItem_Click);
            // 
            // disableHighlightsToolStripMenuItem
            // 
            this.disableHighlightsToolStripMenuItem.Name = "disableHighlightsToolStripMenuItem";
            this.disableHighlightsToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.disableHighlightsToolStripMenuItem.Text = "Disable Highlighting";
            this.disableHighlightsToolStripMenuItem.Click += new System.EventHandler(this.disableHighlightsToolStripMenuItem_Click);
            // 
            // addStartStopTimestampToolStripMenuItem
            // 
            this.addStartStopTimestampToolStripMenuItem.CheckOnClick = true;
            this.addStartStopTimestampToolStripMenuItem.Name = "addStartStopTimestampToolStripMenuItem";
            this.addStartStopTimestampToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.addStartStopTimestampToolStripMenuItem.Text = "Add Start/Stop Timestamp";
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.fontToolStripMenuItem.Text = "Font...";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // settingsFolderToolStripMenuItem
            // 
            this.settingsFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsFolderChangeToolStripMenuItem,
            this.settingsFolderResetToolStripMenuItem});
            this.settingsFolderToolStripMenuItem.Name = "settingsFolderToolStripMenuItem";
            this.settingsFolderToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.settingsFolderToolStripMenuItem.Text = "Settings folder";
            // 
            // settingsFolderChangeToolStripMenuItem
            // 
            this.settingsFolderChangeToolStripMenuItem.Name = "settingsFolderChangeToolStripMenuItem";
            this.settingsFolderChangeToolStripMenuItem.Size = new System.Drawing.Size(238, 34);
            this.settingsFolderChangeToolStripMenuItem.Text = "Change...";
            this.settingsFolderChangeToolStripMenuItem.Click += new System.EventHandler(this.settingsFolderChangeToolStripMenuItem_Click);
            // 
            // settingsFolderResetToolStripMenuItem
            // 
            this.settingsFolderResetToolStripMenuItem.Name = "settingsFolderResetToolStripMenuItem";
            this.settingsFolderResetToolStripMenuItem.Size = new System.Drawing.Size(238, 34);
            this.settingsFolderResetToolStripMenuItem.Text = "Reset to default";
            this.settingsFolderResetToolStripMenuItem.Click += new System.EventHandler(this.settingsFolderResetToolStripMenuItem_Click);
            // 
            // nonPrintableCharsAsHexToolStripMenuItem
            // 
            this.nonPrintableCharsAsHexToolStripMenuItem.CheckOnClick = true;
            this.nonPrintableCharsAsHexToolStripMenuItem.Name = "nonPrintableCharsAsHexToolStripMenuItem";
            this.nonPrintableCharsAsHexToolStripMenuItem.Size = new System.Drawing.Size(330, 34);
            this.nonPrintableCharsAsHexToolStripMenuItem.Text = "Non-Printable Chars as Hex";
            this.nonPrintableCharsAsHexToolStripMenuItem.Click += new System.EventHandler(this.nonPrintableCharsAsHexToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsAddViewToolStripMenuItem,
            this.toolsSerialSendToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolsAddViewToolStripMenuItem
            // 
            this.toolsAddViewToolStripMenuItem.Name = "toolsAddViewToolStripMenuItem";
            this.toolsAddViewToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.toolsAddViewToolStripMenuItem.Text = "Add View...";
            this.toolsAddViewToolStripMenuItem.Click += new System.EventHandler(this.toolsAddViewToolStripMenuItem_Click);
            // 
            // toolsSerialSendToolStripMenuItem
            // 
            this.toolsSerialSendToolStripMenuItem.Name = "toolsSerialSendToolStripMenuItem";
            this.toolsSerialSendToolStripMenuItem.Size = new System.Drawing.Size(213, 34);
            this.toolsSerialSendToolStripMenuItem.Text = "Serial Send...";
            this.toolsSerialSendToolStripMenuItem.Click += new System.EventHandler(this.toolsSerialSendToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // button_run
            // 
            this.button_run.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_run.Location = new System.Drawing.Point(370, 53);
            this.button_run.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_run.Name = "button_run";
            this.button_run.Size = new System.Drawing.Size(47, 38);
            this.button_run.TabIndex = 3;
            this.button_run.Text = "▶";
            this.button_run.UseVisualStyleBackColor = true;
            this.button_run.Click += new System.EventHandler(this.button_run_Click);
            // 
            // comboBox_port
            // 
            this.comboBox_port.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_port.FormattingEnabled = true;
            this.comboBox_port.Location = new System.Drawing.Point(71, 53);
            this.comboBox_port.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_port.Name = "comboBox_port";
            this.comboBox_port.Size = new System.Drawing.Size(93, 33);
            this.comboBox_port.TabIndex = 1;
            this.comboBox_port.DropDown += new System.EventHandler(this.comboBox_port_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "Baud:";
            // 
            // button_stop
            // 
            this.button_stop.Enabled = false;
            this.button_stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button_stop.Location = new System.Drawing.Point(426, 53);
            this.button_stop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(47, 38);
            this.button_stop.TabIndex = 4;
            this.button_stop.Text = "■";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // checkBox_follow
            // 
            this.checkBox_follow.AutoSize = true;
            this.checkBox_follow.Checked = true;
            this.checkBox_follow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_follow.Location = new System.Drawing.Point(487, 57);
            this.checkBox_follow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox_follow.Name = "checkBox_follow";
            this.checkBox_follow.Size = new System.Drawing.Size(90, 29);
            this.checkBox_follow.TabIndex = 5;
            this.checkBox_follow.Text = "Follow";
            this.checkBox_follow.UseVisualStyleBackColor = true;
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
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600",
            "1000000"});
            this.comboBox_baud.Location = new System.Drawing.Point(249, 53);
            this.comboBox_baud.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_baud.Name = "comboBox_baud";
            this.comboBox_baud.Size = new System.Drawing.Size(111, 33);
            this.comboBox_baud.TabIndex = 2;
            // 
            // button_findnext
            // 
            this.button_findnext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_findnext.ForeColor = System.Drawing.Color.Black;
            this.button_findnext.Location = new System.Drawing.Point(837, 53);
            this.button_findnext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_findnext.Name = "button_findnext";
            this.button_findnext.Size = new System.Drawing.Size(47, 38);
            this.button_findnext.TabIndex = 8;
            this.button_findnext.Text = ">";
            this.button_findnext.UseVisualStyleBackColor = true;
            this.button_findnext.Click += new System.EventHandler(this.button_findnext_Click);
            // 
            // button_findprev
            // 
            this.button_findprev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_findprev.ForeColor = System.Drawing.Color.Black;
            this.button_findprev.Location = new System.Drawing.Point(784, 53);
            this.button_findprev.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_findprev.Name = "button_findprev";
            this.button_findprev.Size = new System.Drawing.Size(47, 38);
            this.button_findprev.TabIndex = 7;
            this.button_findprev.Text = "<";
            this.button_findprev.UseVisualStyleBackColor = true;
            this.button_findprev.Click += new System.EventHandler(this.button_findprev_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(577, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Find:";
            // 
            // textBox_find
            // 
            this.textBox_find.Location = new System.Drawing.Point(631, 53);
            this.textBox_find.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_find.Name = "textBox_find";
            this.textBox_find.Size = new System.Drawing.Size(141, 31);
            this.textBox_find.TabIndex = 6;
            this.textBox_find.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_find_KeyDown);
            // 
            // button_findall
            // 
            this.button_findall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button_findall.ForeColor = System.Drawing.Color.Black;
            this.button_findall.Location = new System.Drawing.Point(891, 53);
            this.button_findall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_findall.Name = "button_findall";
            this.button_findall.Size = new System.Drawing.Size(47, 38);
            this.button_findall.TabIndex = 9;
            this.button_findall.Text = "All";
            this.button_findall.UseVisualStyleBackColor = true;
            this.button_findall.Click += new System.EventHandler(this.button_findall_Click);
            // 
            // timer_updatesysinfo
            // 
            this.timer_updatesysinfo.Enabled = true;
            this.timer_updatesysinfo.Interval = 1000;
            this.timer_updatesysinfo.Tick += new System.EventHandler(this.timer_updatesysinfo_Tick);
            // 
            // button_prev_highlight
            // 
            this.button_prev_highlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_prev_highlight.ForeColor = System.Drawing.Color.Black;
            this.button_prev_highlight.Location = new System.Drawing.Point(1108, 53);
            this.button_prev_highlight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_prev_highlight.Name = "button_prev_highlight";
            this.button_prev_highlight.Size = new System.Drawing.Size(47, 38);
            this.button_prev_highlight.TabIndex = 10;
            this.button_prev_highlight.Text = "<h";
            this.button_prev_highlight.UseVisualStyleBackColor = true;
            this.button_prev_highlight.Click += new System.EventHandler(this.button_prev_highlight_Click);
            // 
            // button_next_highlight
            // 
            this.button_next_highlight.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button_next_highlight.ForeColor = System.Drawing.Color.Black;
            this.button_next_highlight.Location = new System.Drawing.Point(1161, 53);
            this.button_next_highlight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_next_highlight.Name = "button_next_highlight";
            this.button_next_highlight.Size = new System.Drawing.Size(47, 38);
            this.button_next_highlight.TabIndex = 11;
            this.button_next_highlight.Text = "h>";
            this.button_next_highlight.UseVisualStyleBackColor = true;
            this.button_next_highlight.Click += new System.EventHandler(this.button_next_highlight_Click);
            // 
            // checkBoxRegex
            // 
            this.checkBoxRegex.AutoSize = true;
            this.checkBoxRegex.Location = new System.Drawing.Point(950, 57);
            this.checkBoxRegex.Name = "checkBoxRegex";
            this.checkBoxRegex.Size = new System.Drawing.Size(85, 29);
            this.checkBoxRegex.TabIndex = 19;
            this.checkBoxRegex.Text = "Regex";
            this.checkBoxRegex.UseVisualStyleBackColor = true;
            this.checkBoxRegex.CheckedChanged += new System.EventHandler(this.checkBoxRegex_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 1602);
            this.Controls.Add(this.checkBoxRegex);
            this.Controls.Add(this.button_prev_highlight);
            this.Controls.Add(this.button_next_highlight);
            this.Controls.Add(this.button_findall);
            this.Controls.Add(this.textBox_find);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_findprev);
            this.Controls.Add(this.button_findnext);
            this.Controls.Add(this.comboBox_baud);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.checkBox_follow);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_port);
            this.Controls.Add(this.button_run);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(969, 629);
            this.Name = "Form1";
            this.Text = "serialog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
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
        private ToolStripMenuItem formHighlightToolStripMenuItem;
        private CheckBox checkBox_follow;
        private System.Windows.Forms.Timer timer_addtolist;
        private ColumnHeader column1;
        private ComboBox comboBox_baud;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem saveSelectedToolStripMenuItem;
        private Button button_findnext;
        private Button button_findprev;
        private Label label3;
        private TextBox textBox_find;
        private Button button_findall;
        private ToolStripMenuItem hideUnhighlightedToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.Timer timer_updatesysinfo;
        private ToolStripMenuItem addStartStopTimestampToolStripMenuItem;
        private FontDialog fontDialog1;
        private ToolStripMenuItem fontToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem selectAllContextMenuItem;
        private ToolStripMenuItem clearAllContextMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem formHighlightContextMenuItem;
        private ToolStripMenuItem hideUnhighlightedContextMenuItem;
        private ToolStripMenuItem saveSerialAsToolStripMenuItem;
        private ToolStripMenuItem disableHighlightsToolStripMenuItem;
        private ToolStripMenuItem disableHighlightsContextMenuItem;
        private ToolStripMenuItem addCustomRowToolStripMenuItem;
        private ToolStripMenuItem addCustomRowContextMenuItem;
        private Button button_prev_highlight;
        private Button button_next_highlight;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem toolsAddViewToolStripMenuItem;
        private ToolStripMenuItem toolsSerialSendToolStripMenuItem;
        public ListViewVirt listView1;
        private ToolStripMenuItem settingsFolderToolStripMenuItem;
        private ToolStripMenuItem settingsFolderChangeToolStripMenuItem;
        private ToolStripMenuItem settingsFolderResetToolStripMenuItem;
        private ToolStripMenuItem nonPrintableCharsAsHexToolStripMenuItem;
        private CheckBox checkBoxRegex;
    }
}