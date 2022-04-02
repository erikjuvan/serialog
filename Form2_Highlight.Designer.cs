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
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_ok.Location = new System.Drawing.Point(182, 426);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(81, 23);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_cancel.Location = new System.Drawing.Point(269, 426);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(81, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(338, 222);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 300;
            // 
            // button_add
            // 
            this.button_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_add.Location = new System.Drawing.Point(12, 240);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(81, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "Add";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_delete
            // 
            this.button_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_delete.Location = new System.Drawing.Point(97, 240);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(81, 23);
            this.button_delete.TabIndex = 4;
            this.button_delete.Text = "Delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_moveup
            // 
            this.button_moveup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_moveup.Location = new System.Drawing.Point(182, 240);
            this.button_moveup.Name = "button_moveup";
            this.button_moveup.Size = new System.Drawing.Size(81, 23);
            this.button_moveup.TabIndex = 5;
            this.button_moveup.Text = "Move Up";
            this.button_moveup.UseVisualStyleBackColor = true;
            // 
            // button_movedown
            // 
            this.button_movedown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_movedown.Location = new System.Drawing.Point(269, 240);
            this.button_movedown.Name = "button_movedown";
            this.button_movedown.Size = new System.Drawing.Size(81, 23);
            this.button_movedown.TabIndex = 6;
            this.button_movedown.Text = "Move Down";
            this.button_movedown.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Foreground Color:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Background Color:";
            // 
            // comboBox_bgcolor
            // 
            this.comboBox_bgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_bgcolor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_bgcolor.FormattingEnabled = true;
            this.comboBox_bgcolor.Location = new System.Drawing.Point(194, 294);
            this.comboBox_bgcolor.Name = "comboBox_bgcolor";
            this.comboBox_bgcolor.Size = new System.Drawing.Size(123, 23);
            this.comboBox_bgcolor.TabIndex = 10;
            // 
            // button_fgcolor
            // 
            this.button_fgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_fgcolor.FlatAppearance.BorderSize = 0;
            this.button_fgcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_fgcolor.Image = ((System.Drawing.Image)(resources.GetObject("button_fgcolor.Image")));
            this.button_fgcolor.Location = new System.Drawing.Point(141, 294);
            this.button_fgcolor.Name = "button_fgcolor";
            this.button_fgcolor.Size = new System.Drawing.Size(27, 23);
            this.button_fgcolor.TabIndex = 14;
            this.button_fgcolor.TabStop = false;
            this.button_fgcolor.UseVisualStyleBackColor = true;
            this.button_fgcolor.Click += new System.EventHandler(this.button_fgcolor_Click);
            // 
            // comboBox_fgcolor
            // 
            this.comboBox_fgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_fgcolor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_fgcolor.FormattingEnabled = true;
            this.comboBox_fgcolor.Location = new System.Drawing.Point(12, 294);
            this.comboBox_fgcolor.Name = "comboBox_fgcolor";
            this.comboBox_fgcolor.Size = new System.Drawing.Size(123, 23);
            this.comboBox_fgcolor.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 335);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 15;
            this.label3.Text = "String:";
            // 
            // textBox_string
            // 
            this.textBox_string.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_string.Location = new System.Drawing.Point(12, 353);
            this.textBox_string.Name = "textBox_string";
            this.textBox_string.Size = new System.Drawing.Size(338, 23);
            this.textBox_string.TabIndex = 16;
            // 
            // checkBox_ignorecase
            // 
            this.checkBox_ignorecase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ignorecase.AutoSize = true;
            this.checkBox_ignorecase.Location = new System.Drawing.Point(12, 391);
            this.checkBox_ignorecase.Name = "checkBox_ignorecase";
            this.checkBox_ignorecase.Size = new System.Drawing.Size(88, 19);
            this.checkBox_ignorecase.TabIndex = 17;
            this.checkBox_ignorecase.Text = "Ignore Case";
            this.checkBox_ignorecase.UseVisualStyleBackColor = true;
            // 
            // checkBox_bold
            // 
            this.checkBox_bold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_bold.AutoSize = true;
            this.checkBox_bold.Location = new System.Drawing.Point(106, 391);
            this.checkBox_bold.Name = "checkBox_bold";
            this.checkBox_bold.Size = new System.Drawing.Size(50, 19);
            this.checkBox_bold.TabIndex = 18;
            this.checkBox_bold.Text = "Bold";
            this.checkBox_bold.UseVisualStyleBackColor = true;
            // 
            // button_bgcolor
            // 
            this.button_bgcolor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_bgcolor.FlatAppearance.BorderSize = 0;
            this.button_bgcolor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_bgcolor.Image = ((System.Drawing.Image)(resources.GetObject("button_bgcolor.Image")));
            this.button_bgcolor.Location = new System.Drawing.Point(323, 294);
            this.button_bgcolor.Name = "button_bgcolor";
            this.button_bgcolor.Size = new System.Drawing.Size(27, 23);
            this.button_bgcolor.TabIndex = 19;
            this.button_bgcolor.TabStop = false;
            this.button_bgcolor.UseVisualStyleBackColor = true;
            this.button_bgcolor.Click += new System.EventHandler(this.button_bgcolor_Click);
            // 
            // Form2_Highlight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 461);
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
            this.MaximumSize = new System.Drawing.Size(378, 800);
            this.MinimumSize = new System.Drawing.Size(378, 500);
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
    }
}