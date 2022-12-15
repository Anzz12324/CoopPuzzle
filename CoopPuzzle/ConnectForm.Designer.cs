namespace CoopPuzzle
{
    partial class ConnectForm
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
            this.radBtnHost = new System.Windows.Forms.RadioButton();
            this.radBtnJoin = new System.Windows.Forms.RadioButton();
            this.grpHost = new System.Windows.Forms.GroupBox();
            this.btnHost = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHostPassword = new System.Windows.Forms.TextBox();
            this.txtHostPort = new System.Windows.Forms.TextBox();
            this.grpJoin = new System.Windows.Forms.GroupBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.txtJoinPassword = new System.Windows.Forms.TextBox();
            this.txtJoinPort = new System.Windows.Forms.TextBox();
            this.txtJoinIp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpHost.SuspendLayout();
            this.grpJoin.SuspendLayout();
            this.SuspendLayout();
            // 
            // radBtnHost
            // 
            this.radBtnHost.AutoSize = true;
            this.radBtnHost.Location = new System.Drawing.Point(61, 69);
            this.radBtnHost.Name = "radBtnHost";
            this.radBtnHost.Size = new System.Drawing.Size(67, 19);
            this.radBtnHost.TabIndex = 0;
            this.radBtnHost.TabStop = true;
            this.radBtnHost.Text = "Hosting";
            this.radBtnHost.UseVisualStyleBackColor = true;
            this.radBtnHost.CheckedChanged += new System.EventHandler(this.radHost_CheckedChanged);
            // 
            // radBtnJoin
            // 
            this.radBtnJoin.AutoSize = true;
            this.radBtnJoin.Location = new System.Drawing.Point(179, 69);
            this.radBtnJoin.Name = "radBtnJoin";
            this.radBtnJoin.Size = new System.Drawing.Size(63, 19);
            this.radBtnJoin.TabIndex = 1;
            this.radBtnJoin.TabStop = true;
            this.radBtnJoin.Text = "Joining";
            this.radBtnJoin.UseVisualStyleBackColor = true;
            this.radBtnJoin.CheckedChanged += new System.EventHandler(this.radJoin_CheckedChanged);
            // 
            // grpHost
            // 
            this.grpHost.Controls.Add(this.btnHost);
            this.grpHost.Controls.Add(this.label2);
            this.grpHost.Controls.Add(this.label1);
            this.grpHost.Controls.Add(this.txtHostPassword);
            this.grpHost.Controls.Add(this.txtHostPort);
            this.grpHost.Location = new System.Drawing.Point(12, 94);
            this.grpHost.Name = "grpHost";
            this.grpHost.Size = new System.Drawing.Size(280, 187);
            this.grpHost.TabIndex = 2;
            this.grpHost.TabStop = false;
            this.grpHost.Text = "Host";
            // 
            // btnHost
            // 
            this.btnHost.Location = new System.Drawing.Point(94, 158);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(75, 23);
            this.btnHost.TabIndex = 4;
            this.btnHost.Text = "Host";
            this.btnHost.UseVisualStyleBackColor = true;
            this.btnHost.Click += new System.EventHandler(this.btnHost_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port";
            // 
            // txtHostPassword
            // 
            this.txtHostPassword.Location = new System.Drawing.Point(95, 51);
            this.txtHostPassword.Name = "txtHostPassword";
            this.txtHostPassword.Size = new System.Drawing.Size(125, 23);
            this.txtHostPassword.TabIndex = 1;
            this.txtHostPassword.Text = "SomeConnectionKey";
            // 
            // txtHostPort
            // 
            this.txtHostPort.Location = new System.Drawing.Point(95, 22);
            this.txtHostPort.Name = "txtHostPort";
            this.txtHostPort.Size = new System.Drawing.Size(125, 23);
            this.txtHostPort.TabIndex = 0;
            this.txtHostPort.Text = "27960";
            // 
            // grpJoin
            // 
            this.grpJoin.Controls.Add(this.btnJoin);
            this.grpJoin.Controls.Add(this.txtJoinPassword);
            this.grpJoin.Controls.Add(this.txtJoinPort);
            this.grpJoin.Controls.Add(this.txtJoinIp);
            this.grpJoin.Controls.Add(this.label5);
            this.grpJoin.Controls.Add(this.label4);
            this.grpJoin.Controls.Add(this.label3);
            this.grpJoin.Location = new System.Drawing.Point(12, 94);
            this.grpJoin.Name = "grpJoin";
            this.grpJoin.Size = new System.Drawing.Size(280, 187);
            this.grpJoin.TabIndex = 3;
            this.grpJoin.TabStop = false;
            this.grpJoin.Text = "Join";
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(94, 158);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(75, 23);
            this.btnJoin.TabIndex = 6;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // txtJoinPassword
            // 
            this.txtJoinPassword.Location = new System.Drawing.Point(94, 81);
            this.txtJoinPassword.Name = "txtJoinPassword";
            this.txtJoinPassword.Size = new System.Drawing.Size(125, 23);
            this.txtJoinPassword.TabIndex = 5;
            this.txtJoinPassword.Text = "SomeConnectionKey";
            // 
            // txtJoinPort
            // 
            this.txtJoinPort.Location = new System.Drawing.Point(94, 51);
            this.txtJoinPort.Name = "txtJoinPort";
            this.txtJoinPort.Size = new System.Drawing.Size(125, 23);
            this.txtJoinPort.TabIndex = 4;
            this.txtJoinPort.Text = "27960";
            // 
            // txtJoinIp
            // 
            this.txtJoinIp.Location = new System.Drawing.Point(95, 22);
            this.txtJoinIp.Name = "txtJoinIp";
            this.txtJoinIp.Size = new System.Drawing.Size(125, 23);
            this.txtJoinIp.TabIndex = 3;
            this.txtJoinIp.Text = "localhost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Ip";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Port";
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 293);
            this.Controls.Add(this.grpJoin);
            this.Controls.Add(this.grpHost);
            this.Controls.Add(this.radBtnJoin);
            this.Controls.Add(this.radBtnHost);
            this.Name = "ConnectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConnectForm";
            this.TopMost = true;
            this.grpHost.ResumeLayout(false);
            this.grpHost.PerformLayout();
            this.grpJoin.ResumeLayout(false);
            this.grpJoin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radBtnHost;
        private System.Windows.Forms.RadioButton radBtnJoin;
        private System.Windows.Forms.GroupBox grpHost;
        private System.Windows.Forms.GroupBox grpJoin;
        private System.Windows.Forms.TextBox txtHostPassword;
        private System.Windows.Forms.TextBox txtHostPort;
        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJoinIp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtJoinPassword;
        private System.Windows.Forms.TextBox txtJoinPort;
        private System.Windows.Forms.Button btnJoin;
    }
}