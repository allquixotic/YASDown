namespace YASDown
{
    partial class ConfigControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.localFolderButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.sftpCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sftpRemoteFolderBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.browseKeyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.sftpPasswordBox = new System.Windows.Forms.TextBox();
            this.sftpUsernameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelaoiqweif = new System.Windows.Forms.Label();
            this.sftpPortBox = new System.Windows.Forms.TextBox();
            this.label209 = new System.Windows.Forms.Label();
            this.sftpServerBox = new System.Windows.Forms.TextBox();
            this.folderBrowse = new Ookii.Dialogs.VistaFolderBrowserDialog();
            this.privateKeyBrowse = new System.Windows.Forms.OpenFileDialog();
            this.spotifyBitrate = new System.Windows.Forms.ComboBox();
            this.configControlBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lameBitrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // localFolderButton
            // 
            this.localFolderButton.Location = new System.Drawing.Point(3, 3);
            this.localFolderButton.Name = "localFolderButton";
            this.localFolderButton.Size = new System.Drawing.Size(115, 23);
            this.localFolderButton.TabIndex = 0;
            this.localFolderButton.Text = "Browse Local Folder";
            this.localFolderButton.UseVisualStyleBackColor = true;
            this.localFolderButton.Click += new System.EventHandler(this.localFolderButton_Click);
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(3, 331);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(75, 23);
            this.continueButton.TabIndex = 1;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Remote Server";
            // 
            // sftpCheckbox
            // 
            this.sftpCheckbox.AutoSize = true;
            this.sftpCheckbox.Location = new System.Drawing.Point(3, 105);
            this.sftpCheckbox.Name = "sftpCheckbox";
            this.sftpCheckbox.Size = new System.Drawing.Size(102, 17);
            this.sftpCheckbox.TabIndex = 3;
            this.sftpCheckbox.Text = "Upload to SFTP";
            this.sftpCheckbox.UseVisualStyleBackColor = true;
            this.sftpCheckbox.CheckedChanged += new System.EventHandler(this.sftpCheckbox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sftpRemoteFolderBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.browseKeyButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.sftpPasswordBox);
            this.groupBox1.Controls.Add(this.sftpUsernameBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelaoiqweif);
            this.groupBox1.Controls.Add(this.sftpPortBox);
            this.groupBox1.Controls.Add(this.label209);
            this.groupBox1.Controls.Add(this.sftpServerBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 197);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SFTP Info";
            // 
            // sftpRemoteFolderBox
            // 
            this.sftpRemoteFolderBox.Enabled = false;
            this.sftpRemoteFolderBox.Location = new System.Drawing.Point(90, 170);
            this.sftpRemoteFolderBox.Name = "sftpRemoteFolderBox";
            this.sftpRemoteFolderBox.Size = new System.Drawing.Size(100, 20);
            this.sftpRemoteFolderBox.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Remote Folder";
            // 
            // browseKeyButton
            // 
            this.browseKeyButton.Enabled = false;
            this.browseKeyButton.Location = new System.Drawing.Point(90, 135);
            this.browseKeyButton.Name = "browseKeyButton";
            this.browseKeyButton.Size = new System.Drawing.Size(75, 23);
            this.browseKeyButton.TabIndex = 11;
            this.browseKeyButton.Text = "Browse";
            this.browseKeyButton.UseVisualStyleBackColor = true;
            this.browseKeyButton.Click += new System.EventHandler(this.browseKeyButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Private Key";
            // 
            // sftpPasswordBox
            // 
            this.sftpPasswordBox.Enabled = false;
            this.sftpPasswordBox.Location = new System.Drawing.Point(90, 104);
            this.sftpPasswordBox.Name = "sftpPasswordBox";
            this.sftpPasswordBox.Size = new System.Drawing.Size(100, 20);
            this.sftpPasswordBox.TabIndex = 9;
            this.sftpPasswordBox.UseSystemPasswordChar = true;
            // 
            // sftpUsernameBox
            // 
            this.sftpUsernameBox.Enabled = false;
            this.sftpUsernameBox.Location = new System.Drawing.Point(90, 78);
            this.sftpUsernameBox.Name = "sftpUsernameBox";
            this.sftpUsernameBox.Size = new System.Drawing.Size(100, 20);
            this.sftpUsernameBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // labelaoiqweif
            // 
            this.labelaoiqweif.AutoSize = true;
            this.labelaoiqweif.Location = new System.Drawing.Point(6, 78);
            this.labelaoiqweif.Name = "labelaoiqweif";
            this.labelaoiqweif.Size = new System.Drawing.Size(55, 13);
            this.labelaoiqweif.TabIndex = 6;
            this.labelaoiqweif.Text = "Username";
            // 
            // sftpPortBox
            // 
            this.sftpPortBox.Enabled = false;
            this.sftpPortBox.Location = new System.Drawing.Point(90, 48);
            this.sftpPortBox.Name = "sftpPortBox";
            this.sftpPortBox.Size = new System.Drawing.Size(100, 20);
            this.sftpPortBox.TabIndex = 5;
            // 
            // label209
            // 
            this.label209.AutoSize = true;
            this.label209.Location = new System.Drawing.Point(6, 48);
            this.label209.Name = "label209";
            this.label209.Size = new System.Drawing.Size(26, 13);
            this.label209.TabIndex = 4;
            this.label209.Text = "Port";
            // 
            // sftpServerBox
            // 
            this.sftpServerBox.Enabled = false;
            this.sftpServerBox.Location = new System.Drawing.Point(90, 16);
            this.sftpServerBox.Name = "sftpServerBox";
            this.sftpServerBox.Size = new System.Drawing.Size(100, 20);
            this.sftpServerBox.TabIndex = 3;
            // 
            // privateKeyBrowse
            // 
            this.privateKeyBrowse.FileOk += new System.ComponentModel.CancelEventHandler(this.privateKeyBrowse_FileOk);
            // 
            // spotifyBitrate
            // 
            this.spotifyBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.spotifyBitrate.FormattingEnabled = true;
            this.spotifyBitrate.Location = new System.Drawing.Point(93, 41);
            this.spotifyBitrate.Name = "spotifyBitrate";
            this.spotifyBitrate.Size = new System.Drawing.Size(121, 21);
            this.spotifyBitrate.TabIndex = 5;
            // 
            // configControlBindingSource
            // 
            this.configControlBindingSource.DataSource = typeof(YASDown.ConfigControl);
            // 
            // lameBitrate
            // 
            this.lameBitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lameBitrate.FormattingEnabled = true;
            this.lameBitrate.Items.AddRange(new object[] {
            "64",
            "96",
            "128",
            "160",
            "192",
            "256",
            "320"});
            this.lameBitrate.Location = new System.Drawing.Point(93, 68);
            this.lameBitrate.Name = "lameBitrate";
            this.lameBitrate.Size = new System.Drawing.Size(121, 21);
            this.lameBitrate.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Spotify Bitrate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Encode Bitrate";
            // 
            // ConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lameBitrate);
            this.Controls.Add(this.spotifyBitrate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sftpCheckbox);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.localFolderButton);
            this.DoubleBuffered = true;
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(215, 372);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configControlBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button localFolderButton;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox sftpCheckbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button browseKeyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sftpPasswordBox;
        private System.Windows.Forms.TextBox sftpUsernameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelaoiqweif;
        private System.Windows.Forms.TextBox sftpPortBox;
        private System.Windows.Forms.Label label209;
        private System.Windows.Forms.TextBox sftpServerBox;
        private System.Windows.Forms.TextBox sftpRemoteFolderBox;
        private System.Windows.Forms.Label label4;
        private Ookii.Dialogs.VistaFolderBrowserDialog folderBrowse;
        private System.Windows.Forms.OpenFileDialog privateKeyBrowse;
        private System.Windows.Forms.ComboBox spotifyBitrate;
        private System.Windows.Forms.ComboBox lameBitrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource configControlBindingSource;
    }
}
