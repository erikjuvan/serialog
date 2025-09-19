namespace serialog
{
    partial class FormSerialSend
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
            this.button2_send = new System.Windows.Forms.Button();
            this.button_file_delete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_file_load = new System.Windows.Forms.Button();
            this.button_file_save = new System.Windows.Forms.Button();
            this.comboBox_file = new System.Windows.Forms.ComboBox();
            this.button_send_every = new System.Windows.Forms.Button();
            this.textBox_send_every = new System.Windows.Forms.TextBox();
            this.timer_send_every_ms = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2_send
            // 
            this.button2_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2_send.Location = new System.Drawing.Point(12, 801);
            this.button2_send.Name = "button2_send";
            this.button2_send.Size = new System.Drawing.Size(160, 34);
            this.button2_send.TabIndex = 2;
            this.button2_send.Text = "Send";
            this.button2_send.UseVisualStyleBackColor = true;
            this.button2_send.Click += new System.EventHandler(this.button2_send_Click);
            // 
            // button_file_delete
            // 
            this.button_file_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_file_delete.Location = new System.Drawing.Point(717, 839);
            this.button_file_delete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_file_delete.Name = "button_file_delete";
            this.button_file_delete.Size = new System.Drawing.Size(71, 38);
            this.button_file_delete.TabIndex = 8;
            this.button_file_delete.Text = "Delete";
            this.button_file_delete.UseVisualStyleBackColor = true;
            this.button_file_delete.Click += new System.EventHandler(this.button_file_delete_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(510, 806);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 25);
            this.label4.TabIndex = 29;
            this.label4.Text = "File:";
            // 
            // button_file_load
            // 
            this.button_file_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_file_load.Location = new System.Drawing.Point(638, 839);
            this.button_file_load.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_file_load.Name = "button_file_load";
            this.button_file_load.Size = new System.Drawing.Size(71, 38);
            this.button_file_load.TabIndex = 7;
            this.button_file_load.Text = "Load";
            this.button_file_load.UseVisualStyleBackColor = true;
            this.button_file_load.Click += new System.EventHandler(this.button_file_load_Click);
            // 
            // button_file_save
            // 
            this.button_file_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_file_save.Location = new System.Drawing.Point(558, 839);
            this.button_file_save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_file_save.Name = "button_file_save";
            this.button_file_save.Size = new System.Drawing.Size(71, 38);
            this.button_file_save.TabIndex = 6;
            this.button_file_save.Text = "Save";
            this.button_file_save.UseVisualStyleBackColor = true;
            this.button_file_save.Click += new System.EventHandler(this.button_file_save_Click);
            // 
            // comboBox_file
            // 
            this.comboBox_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_file.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_file.FormattingEnabled = true;
            this.comboBox_file.Location = new System.Drawing.Point(560, 802);
            this.comboBox_file.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_file.Name = "comboBox_file";
            this.comboBox_file.Size = new System.Drawing.Size(228, 33);
            this.comboBox_file.TabIndex = 5;
            this.comboBox_file.DropDown += new System.EventHandler(this.comboBox_file_DropDown);
            // 
            // button_send_every
            // 
            this.button_send_every.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_send_every.Location = new System.Drawing.Point(12, 841);
            this.button_send_every.Name = "button_send_every";
            this.button_send_every.Size = new System.Drawing.Size(160, 34);
            this.button_send_every.TabIndex = 4;
            this.button_send_every.Text = "Send every (ms):";
            this.button_send_every.UseVisualStyleBackColor = true;
            this.button_send_every.Click += new System.EventHandler(this.button_send_every_Click);
            // 
            // textBox_send_every
            // 
            this.textBox_send_every.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_send_every.Location = new System.Drawing.Point(178, 843);
            this.textBox_send_every.Name = "textBox_send_every";
            this.textBox_send_every.Size = new System.Drawing.Size(150, 31);
            this.textBox_send_every.TabIndex = 3;
            this.textBox_send_every.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_send_every_KeyPress);
            // 
            // timer_send_every_ms
            // 
            this.timer_send_every_ms.Tick += new System.EventHandler(this.timer_send_every_ms_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(776, 782);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 30F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Description";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Command";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // Form4_Send
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 887);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox_send_every);
            this.Controls.Add(this.button_send_every);
            this.Controls.Add(this.button_file_delete);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button_file_load);
            this.Controls.Add(this.button_file_save);
            this.Controls.Add(this.comboBox_file);
            this.Controls.Add(this.button2_send);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "Form4_Send";
            this.Text = "Serial Send";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button button2_send;
        private Button button_file_delete;
        private Label label4;
        private Button button_file_load;
        private Button button_file_save;
        private ComboBox comboBox_file;
        private Button button_send_every;
        private TextBox textBox_send_every;
        private System.Windows.Forms.Timer timer_send_every_ms;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}