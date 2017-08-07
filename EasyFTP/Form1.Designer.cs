namespace EasyFTP
{
    partial class Form1
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
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label_ip = new System.Windows.Forms.Label();
            this.label_user = new System.Windows.Forms.Label();
            this.textBox_user = new System.Windows.Forms.TextBox();
            this.label_pass = new System.Windows.Forms.Label();
            this.textBox_pass = new System.Windows.Forms.TextBox();
            this.button_showFiles = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(12, 51);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(163, 20);
            this.textBox_ip.TabIndex = 0;
            // 
            // label_ip
            // 
            this.label_ip.AutoSize = true;
            this.label_ip.Location = new System.Drawing.Point(12, 35);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(58, 13);
            this.label_ip.TabIndex = 1;
            this.label_ip.Text = "IP Address";
            // 
            // label_user
            // 
            this.label_user.AutoSize = true;
            this.label_user.Location = new System.Drawing.Point(12, 88);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(55, 13);
            this.label_user.TabIndex = 3;
            this.label_user.Text = "Username";
            // 
            // textBox_user
            // 
            this.textBox_user.Location = new System.Drawing.Point(12, 104);
            this.textBox_user.Name = "textBox_user";
            this.textBox_user.Size = new System.Drawing.Size(163, 20);
            this.textBox_user.TabIndex = 2;
            // 
            // label_pass
            // 
            this.label_pass.AutoSize = true;
            this.label_pass.Location = new System.Drawing.Point(12, 146);
            this.label_pass.Name = "label_pass";
            this.label_pass.Size = new System.Drawing.Size(53, 13);
            this.label_pass.TabIndex = 5;
            this.label_pass.Text = "Password";
            // 
            // textBox_pass
            // 
            this.textBox_pass.Location = new System.Drawing.Point(12, 162);
            this.textBox_pass.Name = "textBox_pass";
            this.textBox_pass.PasswordChar = '●';
            this.textBox_pass.Size = new System.Drawing.Size(163, 20);
            this.textBox_pass.TabIndex = 4;
            // 
            // button_showFiles
            // 
            this.button_showFiles.Location = new System.Drawing.Point(12, 209);
            this.button_showFiles.Name = "button_showFiles";
            this.button_showFiles.Size = new System.Drawing.Size(75, 23);
            this.button_showFiles.TabIndex = 6;
            this.button_showFiles.Text = "Show Files";
            this.button_showFiles.UseVisualStyleBackColor = true;
            this.button_showFiles.Click += new System.EventHandler(this.showFiles);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(249, 51);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(486, 234);
            this.treeView1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 455);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button_showFiles);
            this.Controls.Add(this.label_pass);
            this.Controls.Add(this.textBox_pass);
            this.Controls.Add(this.label_user);
            this.Controls.Add(this.textBox_user);
            this.Controls.Add(this.label_ip);
            this.Controls.Add(this.textBox_ip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label_ip;
        private System.Windows.Forms.Label label_user;
        private System.Windows.Forms.TextBox textBox_user;
        private System.Windows.Forms.Label label_pass;
        private System.Windows.Forms.TextBox textBox_pass;
        private System.Windows.Forms.Button button_showFiles;
        private System.Windows.Forms.TreeView treeView1;
    }
}

