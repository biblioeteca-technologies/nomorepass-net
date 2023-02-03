namespace exampleNoMorePassNet
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
            this.QRPasswordBack = new System.Windows.Forms.Button();
            this.PasswordBoxBack = new System.Windows.Forms.TextBox();
            this.UserBoxBack = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.QrBox2 = new System.Windows.Forms.PictureBox();
            this.QRLabel = new System.Windows.Forms.Label();
            this.TestButton = new System.Windows.Forms.Button();
            this.UserBox = new System.Windows.Forms.TextBox();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.QrBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.QrBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrBox)).BeginInit();
            this.SuspendLayout();
            // 
            // QRPasswordBack
            // 
            this.QRPasswordBack.Location = new System.Drawing.Point(248, 378);
            this.QRPasswordBack.Name = "QRPasswordBack";
            this.QRPasswordBack.Size = new System.Drawing.Size(242, 73);
            this.QRPasswordBack.TabIndex = 25;
            this.QRPasswordBack.Text = "Generate QR";
            this.QRPasswordBack.UseVisualStyleBackColor = true;
            this.QRPasswordBack.Click += new System.EventHandler(this.QRPasswordBack_Click);
            // 
            // PasswordBoxBack
            // 
            this.PasswordBoxBack.Location = new System.Drawing.Point(248, 319);
            this.PasswordBoxBack.Multiline = true;
            this.PasswordBoxBack.Name = "PasswordBoxBack";
            this.PasswordBoxBack.PlaceholderText = "Password";
            this.PasswordBoxBack.ReadOnly = true;
            this.PasswordBoxBack.Size = new System.Drawing.Size(242, 40);
            this.PasswordBoxBack.TabIndex = 24;
            // 
            // UserBoxBack
            // 
            this.UserBoxBack.Location = new System.Drawing.Point(248, 262);
            this.UserBoxBack.Multiline = true;
            this.UserBoxBack.Name = "UserBoxBack";
            this.UserBoxBack.PlaceholderText = "User";
            this.UserBoxBack.ReadOnly = true;
            this.UserBoxBack.Size = new System.Drawing.Size(242, 40);
            this.UserBoxBack.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(17, 455);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(213, 23);
            this.label2.TabIndex = 22;
            this.label2.Text = "Qr to get your passwords";
            // 
            // QrBox2
            // 
            this.QrBox2.Location = new System.Drawing.Point(32, 262);
            this.QrBox2.Name = "QrBox2";
            this.QrBox2.Size = new System.Drawing.Size(197, 189);
            this.QrBox2.TabIndex = 21;
            this.QrBox2.TabStop = false;
            // 
            // QRLabel
            // 
            this.QRLabel.AutoSize = true;
            this.QRLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.QRLabel.Location = new System.Drawing.Point(17, 221);
            this.QRLabel.Name = "QRLabel";
            this.QRLabel.Size = new System.Drawing.Size(227, 23);
            this.QRLabel.TabIndex = 20;
            this.QRLabel.Text = "QR to send your passwords";
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(248, 141);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(242, 73);
            this.TestButton.TabIndex = 19;
            this.TestButton.Text = "Generate QR with your user and password";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // UserBox
            // 
            this.UserBox.Location = new System.Drawing.Point(248, 25);
            this.UserBox.MaxLength = 14;
            this.UserBox.Multiline = true;
            this.UserBox.Name = "UserBox";
            this.UserBox.PlaceholderText = "User - usertest (default)";
            this.UserBox.Size = new System.Drawing.Size(242, 40);
            this.UserBox.TabIndex = 18;
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(248, 85);
            this.PasswordBox.MaxLength = 14;
            this.PasswordBox.Multiline = true;
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PlaceholderText = "Password - mypassword (default)";
            this.PasswordBox.Size = new System.Drawing.Size(242, 40);
            this.PasswordBox.TabIndex = 17;
            // 
            // QrBox
            // 
            this.QrBox.Location = new System.Drawing.Point(33, 25);
            this.QrBox.Name = "QrBox";
            this.QrBox.Size = new System.Drawing.Size(197, 189);
            this.QrBox.TabIndex = 16;
            this.QrBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 505);
            this.Controls.Add(this.QRPasswordBack);
            this.Controls.Add(this.PasswordBoxBack);
            this.Controls.Add(this.UserBoxBack);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.QrBox2);
            this.Controls.Add(this.QRLabel);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.UserBox);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.QrBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ExampleApp NoMorePass";
            ((System.ComponentModel.ISupportInitialize)(this.QrBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QrBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button QRPasswordBack;
        private TextBox PasswordBoxBack;
        private TextBox UserBoxBack;
        private Label label2;
        private PictureBox QrBox2;
        private Label QRLabel;
        private Button TestButton;
        private TextBox UserBox;
        private TextBox PasswordBox;
        private PictureBox QrBox;
    }
}