namespace serialog
{
    partial class Form2_Highlight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2_Highlight));
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.button_add = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_moveup = new System.Windows.Forms.Button();
            this.button_movedown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_bgcolor = new System.Windows.Forms.ComboBox();
            this.button_fgcolor = new System.Windows.Forms.Button();
            this.comboBox_fgcolor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_string = new System.Windows.Forms.TextBox();
            this.checkBox_ignorecase = new System.Windows.Forms.CheckBox();
            this.checkBox_bold = new System.Windows.Forms.CheckBox();
            this.button_bgcolor = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.comboBox_preset = new System.Windows.Forms.ComboBox();
            this.button_preset_save = new System.Windows.Forms.Button();
            this.button_preset_load = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_hide = new System.Windows.Forms.CheckBox();
            this.checkBox_italic = new System.Windows.Forms.CheckBox();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_deletepreset = new System.Windows.Forms.Button();
            this.checkBox_remove = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ok.Location = new System.Drawing.Point(206, 495);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(80, 23);
            this.button_ok.TabIndex = 12;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(292, 495);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(80, 23);
            this.button_cancel.TabIndex = 13;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(360, 249);
            this.listView1.TabIndex = 19;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Contains Text";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Ign C";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Hide";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Remove";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_add
            // 
            this.button_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_add.Location = new System.Drawing.Point(12, 267);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(81, 23);
            this.button_add.TabIndex = 8;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_delete
            // 
            this.button_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_delete.Location = new System.Drawing.Point(105, 267);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(81, 23);
            this.button_delete.TabIndex = 9;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_moveup
            // 
            this.button_moveup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_moveup.Location = new System.Drawing.Point(198, 267);
            this.button_moveup.Name = "button_moveup";
            this.button_moveup.Size = new System.Drawing.Size(81, 23);
            this.button_moveup.TabIndex = 10;
            this.button_moveup.Text = "Move Up";
            this.button_moveup.UseVisualStyleBackColor = true;
            this.button_moveup.Click += new System.EventHandler(this.button_moveup_Click);
            // 
            // button_movedown
            // 
            this.button_movedown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_movedown.Location = new System.Drawing.Point(291, 267);
            this.button_movedown.Name = "button_movedown";
            this.button_movedown.Size = new System.Drawing.Size(81, 23);
            this.button_movedown.TabIndex = 11;
            this.button_movedown.Text = "Move Down";
            this.button_movedown.UseVisualStyleBackColor = true;
            this.button_movedown.Click += new System.EventHandler(this.button_movedown_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Foreground Color:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 309);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Background Color:";
            // 
            // comboBox_bgcolor
            // 
            this.comboBox_bgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_bgcolor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox_bgcolor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_bgcolor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_bgcolor.FormattingEnabled = true;
            this.comboBox_bgcolor.Location = new System.Drawing.Point(216, 327);
            this.comboBox_bgcolor.Name = "comboBox_bgcolor";
            this.comboBox_bgcolor.Size = new System.Drawing.Size(123, 23);
            this.comboBox_bgcolor.TabIndex = 3;
            this.comboBox_bgcolor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_bgcolor_KeyDown);
            // 
            // button_fgcolor
            // 
            this.button_fgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_fgcolor.FlatAppearance.BorderSize = 0;
            this.button_fgcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_fgcolor.Image = ((System.Drawing.Image)(resources.GetObject("button_fgcolor.Image")));
            this.button_fgcolor.Location = new System.Drawing.Point(141, 327);
            this.button_fgcolor.Name = "button_fgcolor";
            this.button_fgcolor.Size = new System.Drawing.Size(27, 23);
            this.button_fgcolor.TabIndex = 17;
            this.button_fgcolor.TabStop = false;
            this.button_fgcolor.UseVisualStyleBackColor = true;
            this.button_fgcolor.Click += new System.EventHandler(this.button_fgcolor_Click);
            // 
            // comboBox_fgcolor
            // 
            this.comboBox_fgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_fgcolor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox_fgcolor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_fgcolor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_fgcolor.FormattingEnabled = true;
            this.comboBox_fgcolor.Location = new System.Drawing.Point(12, 327);
            this.comboBox_fgcolor.Name = "comboBox_fgcolor";
            this.comboBox_fgcolor.Size = new System.Drawing.Size(123, 23);
            this.comboBox_fgcolor.TabIndex = 2;
            this.comboBox_fgcolor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_fgcolor_KeyDown);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 369);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "String:";
            // 
            // textBox_string
            // 
            this.textBox_string.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_string.Location = new System.Drawing.Point(12, 387);
            this.textBox_string.Name = "textBox_string";
            this.textBox_string.Size = new System.Drawing.Size(360, 23);
            this.textBox_string.TabIndex = 1;
            this.textBox_string.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_string_KeyDown);
            // 
            // checkBox_ignorecase
            // 
            this.checkBox_ignorecase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ignorecase.AutoSize = true;
            this.checkBox_ignorecase.Location = new System.Drawing.Point(152, 416);
            this.checkBox_ignorecase.Name = "checkBox_ignorecase";
            this.checkBox_ignorecase.Size = new System.Drawing.Size(88, 19);
            this.checkBox_ignorecase.TabIndex = 4;
            this.checkBox_ignorecase.Text = "Ignore Case";
            this.checkBox_ignorecase.UseVisualStyleBackColor = true;
            // 
            // checkBox_bold
            // 
            this.checkBox_bold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_bold.AutoSize = true;
            this.checkBox_bold.Location = new System.Drawing.Point(12, 416);
            this.checkBox_bold.Name = "checkBox_bold";
            this.checkBox_bold.Size = new System.Drawing.Size(50, 19);
            this.checkBox_bold.TabIndex = 5;
            this.checkBox_bold.Text = "Bold";
            this.checkBox_bold.UseVisualStyleBackColor = true;
            // 
            // button_bgcolor
            // 
            this.button_bgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_bgcolor.FlatAppearance.BorderSize = 0;
            this.button_bgcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_bgcolor.Image = ((System.Drawing.Image)(resources.GetObject("button_bgcolor.Image")));
            this.button_bgcolor.Location = new System.Drawing.Point(345, 327);
            this.button_bgcolor.Name = "button_bgcolor";
            this.button_bgcolor.Size = new System.Drawing.Size(27, 23);
            this.button_bgcolor.TabIndex = 18;
            this.button_bgcolor.TabStop = false;
            this.button_bgcolor.UseVisualStyleBackColor = true;
            this.button_bgcolor.Click += new System.EventHandler(this.button_bgcolor_Click);
            // 
            // comboBox_preset
            // 
            this.comboBox_preset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_preset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_preset.FormattingEnabled = true;
            this.comboBox_preset.Location = new System.Drawing.Point(12, 466);
            this.comboBox_preset.Name = "comboBox_preset";
            this.comboBox_preset.Size = new System.Drawing.Size(161, 23);
            this.comboBox_preset.TabIndex = 14;
            this.comboBox_preset.DropDown += new System.EventHandler(this.comboBox_preset_DropDown);
            // 
            // button_preset_save
            // 
            this.button_preset_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_preset_save.Location = new System.Drawing.Point(12, 495);
            this.button_preset_save.Name = "button_preset_save";
            this.button_preset_save.Size = new System.Drawing.Size(50, 23);
            this.button_preset_save.TabIndex = 15;
            this.button_preset_save.Text = "Save";
            this.button_preset_save.UseVisualStyleBackColor = true;
            this.button_preset_save.Click += new System.EventHandler(this.button_preset_save_Click);
            // 
            // button_preset_load
            // 
            this.button_preset_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_preset_load.Location = new System.Drawing.Point(68, 495);
            this.button_preset_load.Name = "button_preset_load";
            this.button_preset_load.Size = new System.Drawing.Size(50, 23);
            this.button_preset_load.TabIndex = 16;
            this.button_preset_load.Text = "Load";
            this.button_preset_load.UseVisualStyleBackColor = true;
            this.button_preset_load.Click += new System.EventHandler(this.button_preset_load_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 448);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Preset:";
            // 
            // checkBox_hide
            // 
            this.checkBox_hide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_hide.AutoSize = true;
            this.checkBox_hide.Location = new System.Drawing.Point(246, 416);
            this.checkBox_hide.Name = "checkBox_hide";
            this.checkBox_hide.Size = new System.Drawing.Size(51, 19);
            this.checkBox_hide.TabIndex = 7;
            this.checkBox_hide.Text = "Hide";
            this.checkBox_hide.UseVisualStyleBackColor = true;
            // 
            // checkBox_italic
            // 
            this.checkBox_italic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_italic.AutoSize = true;
            this.checkBox_italic.Location = new System.Drawing.Point(68, 416);
            this.checkBox_italic.Name = "checkBox_italic";
            this.checkBox_italic.Size = new System.Drawing.Size(51, 19);
            this.checkBox_italic.TabIndex = 6;
            this.checkBox_italic.Text = "Italic";
            this.checkBox_italic.UseVisualStyleBackColor = true;
            // 
            // button_apply
            // 
            this.button_apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_apply.Location = new System.Drawing.Point(292, 466);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(80, 23);
            this.button_apply.TabIndex = 24;
            this.button_apply.Text = "Apply";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.button_apply_Click);
            // 
            // button_deletepreset
            // 
            this.button_deletepreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_deletepreset.Location = new System.Drawing.Point(123, 495);
            this.button_deletepreset.Name = "button_deletepreset";
            this.button_deletepreset.Size = new System.Drawing.Size(50, 23);
            this.button_deletepreset.TabIndex = 25;
            this.button_deletepreset.Text = "Delete";
            this.button_deletepreset.UseVisualStyleBackColor = true;
            this.button_deletepreset.Click += new System.EventHandler(this.button_deletepreset_Click);
            // 
            // checkBox_remove
            // 
            this.checkBox_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_remove.AutoSize = true;
            this.checkBox_remove.Location = new System.Drawing.Point(303, 416);
            this.checkBox_remove.Name = "checkBox_remove";
            this.checkBox_remove.Size = new System.Drawing.Size(69, 19);
            this.checkBox_remove.TabIndex = 26;
            this.checkBox_remove.Text = "Remove";
            this.checkBox_remove.UseVisualStyleBackColor = true;
            // 
            // Form2_Highlight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 530);
            this.Controls.Add(this.checkBox_remove);
            this.Controls.Add(this.button_deletepreset);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.checkBox_italic);
            this.Controls.Add(this.checkBox_hide);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_preset_load);
            this.Controls.Add(this.button_preset_save);
            this.Controls.Add(this.comboBox_preset);
            this.Controls.Add(this.button_bgcolor);
            this.Controls.Add(this.checkBox_bold);
            this.Controls.Add(this.checkBox_ignorecase);
            this.Controls.Add(this.textBox_string);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_fgcolor);
            this.Controls.Add(this.comboBox_fgcolor);
            this.Controls.Add(this.comboBox_bgcolor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_movedown);
            this.Controls.Add(this.button_moveup);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(400, 798);
            this.MinimumSize = new System.Drawing.Size(400, 498);
            this.Name = "Form2_Highlight";
            this.Text = "Highlight";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_Highlight_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button_ok;
        private Button button_cancel;
        private ListView listView1;
        private Button button_add;
        private Button button_delete;
        private Button button_moveup;
        private Button button_movedown;
        private Label label1;
        private Label label2;
        private ComboBox comboBox_bgcolor;
        private Button button_fgcolor;
        private ComboBox comboBox_fgcolor;
        private Label label3;
        private TextBox textBox_string;
        private CheckBox checkBox_ignorecase;
        private CheckBox checkBox_bold;
        private Button button_bgcolor;
        private ColumnHeader columnHeader1;
        private ColorDialog colorDialog1;
        private Button button_preset_save;
        private Button button_preset_load;
        private Label label4;
        private ColumnHeader columnHeader3;
        private CheckBox checkBox_hide;
        private CheckBox checkBox_italic;
        private Button button_apply;
        private Button button_deletepreset;
        private ComboBox comboBox_preset;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader2;
        private CheckBox checkBox_remove;
    }
}