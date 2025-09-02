namespace serialog
{
    partial class Form5_Send
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
            this.button1_add = new System.Windows.Forms.Button();
            this.button2_send = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // button1_add
            // 
            this.button1_add.Location = new System.Drawing.Point(130, 12);
            this.button1_add.Name = "button1_add";
            this.button1_add.Size = new System.Drawing.Size(112, 34);
            this.button1_add.TabIndex = 2;
            this.button1_add.Text = "Add";
            this.button1_add.UseVisualStyleBackColor = true;
            this.button1_add.Click += new System.EventHandler(this.button1_add_Click);
            // 
            // button2_send
            // 
            this.button2_send.Location = new System.Drawing.Point(12, 12);
            this.button2_send.Name = "button2_send";
            this.button2_send.Size = new System.Drawing.Size(112, 34);
            this.button2_send.TabIndex = 3;
            this.button2_send.Text = "Send";
            this.button2_send.UseVisualStyleBackColor = true;
            this.button2_send.Click += new System.EventHandler(this.button2_send_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(248, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(540, 31);
            this.textBox1.TabIndex = 4;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxHex_KeyPress);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 52);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(776, 444);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // Form5_Send
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2_send);
            this.Controls.Add(this.button1_add);
            this.Name = "Form5_Send";
            this.Text = "Form5_Send";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button button1_add;
        private Button button2_send;
        private TextBox textBox1;
        private ListView listView1;
    }
}