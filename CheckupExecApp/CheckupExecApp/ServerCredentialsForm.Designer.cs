namespace CheckupExecApp
{
    partial class ServerCredentialsForm
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
            this.ServerNameTextBox1 = new System.Windows.Forms.TextBox();
            this.UserNameTextBox1 = new System.Windows.Forms.TextBox();
            this.PasswordTextBox1 = new System.Windows.Forms.TextBox();
            this.ServerNameLabel1 = new System.Windows.Forms.Label();
            this.UserNameLabel1 = new System.Windows.Forms.Label();
            this.PasswordLabel1 = new System.Windows.Forms.Label();
            this.RemoteLoginCheckBox1 = new System.Windows.Forms.CheckBox();
            this.LoginButton1 = new System.Windows.Forms.Button();
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerNameTextBox1
            // 
            this.ServerNameTextBox1.Enabled = false;
            this.ServerNameTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameTextBox1.Location = new System.Drawing.Point(163, 48);
            this.ServerNameTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ServerNameTextBox1.Name = "ServerNameTextBox1";
            this.ServerNameTextBox1.Size = new System.Drawing.Size(411, 29);
            this.ServerNameTextBox1.TabIndex = 0;
            // 
            // UserNameTextBox1
            // 
            this.UserNameTextBox1.Enabled = false;
            this.UserNameTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameTextBox1.Location = new System.Drawing.Point(163, 108);
            this.UserNameTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.UserNameTextBox1.Name = "UserNameTextBox1";
            this.UserNameTextBox1.Size = new System.Drawing.Size(411, 29);
            this.UserNameTextBox1.TabIndex = 0;
            // 
            // PasswordTextBox1
            // 
            this.PasswordTextBox1.Enabled = false;
            this.PasswordTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox1.Location = new System.Drawing.Point(163, 168);
            this.PasswordTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PasswordTextBox1.Name = "PasswordTextBox1";
            this.PasswordTextBox1.Size = new System.Drawing.Size(411, 29);
            this.PasswordTextBox1.TabIndex = 0;
            this.PasswordTextBox1.UseSystemPasswordChar = true;
            // 
            // ServerNameLabel1
            // 
            this.ServerNameLabel1.AutoSize = true;
            this.ServerNameLabel1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameLabel1.Location = new System.Drawing.Point(35, 50);
            this.ServerNameLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ServerNameLabel1.Name = "ServerNameLabel1";
            this.ServerNameLabel1.Size = new System.Drawing.Size(124, 25);
            this.ServerNameLabel1.TabIndex = 1;
            this.ServerNameLabel1.Text = "Server Name:";
            // 
            // UserNameLabel1
            // 
            this.UserNameLabel1.AutoSize = true;
            this.UserNameLabel1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserNameLabel1.Location = new System.Drawing.Point(35, 110);
            this.UserNameLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.UserNameLabel1.Name = "UserNameLabel1";
            this.UserNameLabel1.Size = new System.Drawing.Size(109, 25);
            this.UserNameLabel1.TabIndex = 1;
            this.UserNameLabel1.Text = "User Name:";
            // 
            // PasswordLabel1
            // 
            this.PasswordLabel1.AutoSize = true;
            this.PasswordLabel1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel1.Location = new System.Drawing.Point(35, 170);
            this.PasswordLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PasswordLabel1.Name = "PasswordLabel1";
            this.PasswordLabel1.Size = new System.Drawing.Size(95, 25);
            this.PasswordLabel1.TabIndex = 1;
            this.PasswordLabel1.Text = "Password:";
            // 
            // RemoteLoginCheckBox1
            // 
            this.RemoteLoginCheckBox1.AutoSize = true;
            this.RemoteLoginCheckBox1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoteLoginCheckBox1.Location = new System.Drawing.Point(163, 213);
            this.RemoteLoginCheckBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RemoteLoginCheckBox1.Name = "RemoteLoginCheckBox1";
            this.RemoteLoginCheckBox1.Size = new System.Drawing.Size(146, 29);
            this.RemoteLoginCheckBox1.TabIndex = 2;
            this.RemoteLoginCheckBox1.Text = "Remote Login";
            this.RemoteLoginCheckBox1.UseVisualStyleBackColor = true;
            this.RemoteLoginCheckBox1.CheckedChanged += new System.EventHandler(this.RemoteLoginCheckBox1_CheckedChanged);
            // 
            // LoginButton1
            // 
            this.LoginButton1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton1.Location = new System.Drawing.Point(304, 257);
            this.LoginButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoginButton1.Name = "LoginButton1";
            this.LoginButton1.Size = new System.Drawing.Size(128, 37);
            this.LoginButton1.TabIndex = 3;
            this.LoginButton1.Text = "Login";
            this.LoginButton1.UseVisualStyleBackColor = true;
            this.LoginButton1.Click += new System.EventHandler(this.LoginButton1_Click);
            // 
            // CancelButton1
            // 
            this.CancelButton1.Font = new System.Drawing.Font("Segoe UI", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton1.Location = new System.Drawing.Point(446, 257);
            this.CancelButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CancelButton1.Name = "CancelButton1";
            this.CancelButton1.Size = new System.Drawing.Size(128, 37);
            this.CancelButton1.TabIndex = 3;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.UseVisualStyleBackColor = true;
            this.CancelButton1.Click += new System.EventHandler(this.CancelButton1_Click);
            // 
            // ServerCredentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 329);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.LoginButton1);
            this.Controls.Add(this.RemoteLoginCheckBox1);
            this.Controls.Add(this.PasswordLabel1);
            this.Controls.Add(this.UserNameLabel1);
            this.Controls.Add(this.ServerNameLabel1);
            this.Controls.Add(this.PasswordTextBox1);
            this.Controls.Add(this.UserNameTextBox1);
            this.Controls.Add(this.ServerNameTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ServerCredentialsForm";
            this.Text = "Server Login";
            this.Load += new System.EventHandler(this.ServerCredentialsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerNameTextBox1;
        private System.Windows.Forms.TextBox UserNameTextBox1;
        private System.Windows.Forms.TextBox PasswordTextBox1;
        private System.Windows.Forms.Label ServerNameLabel1;
        private System.Windows.Forms.Label UserNameLabel1;
        private System.Windows.Forms.Label PasswordLabel1;
        private System.Windows.Forms.CheckBox RemoteLoginCheckBox1;
        private System.Windows.Forms.Button LoginButton1;
        private System.Windows.Forms.Button CancelButton1;
    }
}