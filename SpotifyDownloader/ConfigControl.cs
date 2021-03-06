﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YASDown
{
    public partial class ConfigControl : UserControl
    {
        private AppConfig config = null;

        public int[] spotValues = new int[]{ 96, 160, 320};

        public ConfigControl(AppConfig conf = null)
        {
            InitializeComponent();

            config = conf;
            if(config == null)
            {
                config = new AppConfig();
            }

            spotifyBitrate.Items.Clear();
            spotifyBitrate.Items.Add(96);
            spotifyBitrate.Items.Add(160);
            spotifyBitrate.Items.Add(320);
            lameBitrate.Items.Clear();
            lameBitrate.Items.Add(96);
            lameBitrate.Items.Add(128);
            lameBitrate.Items.Add(160);
            lameBitrate.Items.Add(192);
            lameBitrate.Items.Add(256);
            lameBitrate.Items.Add(320);

            Binding binding1 = new Binding("Checked", config, "sftpEnabled");
            Binding binding2 = new Binding("Text", config, "sftpRemoteServer");
            Binding binding3 = new Binding("Text", config, "sftpUsername");
            Binding binding4 = new Binding("Text", config, "sftpPassword");
            Binding binding7 = new Binding("Text", config, "sftpRemoteFolder");
            Binding binding8 = new Binding("Text", config, "sftpPort");
            Binding binding9 = new Binding("SelectedItem", config, "spotifyBitrate");
            Binding binding10 = new Binding("SelectedItem", config, "lameBitrate");
            sftpCheckbox.DataBindings.Add(binding1);
            sftpPasswordBox.DataBindings.Add(binding4);
            sftpPortBox.DataBindings.Add(binding8);
            sftpRemoteFolderBox.DataBindings.Add(binding7);
            sftpServerBox.DataBindings.Add(binding2);
            sftpUsernameBox.DataBindings.Add(binding3);
            spotifyBitrate.DataBindings.Add(binding9);
            lameBitrate.DataBindings.Add(binding10);
            folderBrowse.SelectedPath = (config.localBaseFolder != null ? config.localBaseFolder : "");
            privateKeyBrowse.FileName = (config.sftpPrivateKeyPath != null ? config.sftpPrivateKeyPath : "");
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if(config.localBaseFolder == null 
                || config.localBaseFolder.Trim().Length <= 0
                || (config.sftpEnabled && (bad(config.sftpRemoteServer, config.sftpUsername) || bothBad(config.sftpPassword, config.sftpPrivateKeyPath)))
                ) 
            {
                MessageBox.Show("Unable to continue; required fields missing.");
            }
            else
            {
                Form1 frm = ((Form1)Parent.Parent);
                frm.Config = config;
                config.Save();
                if(frm.LoggedIn)
                {
                    frm.LoadMain();
                }
                else
                {
                    frm.LoadLogin();
                }
            }
        }

        private bool bad(params string[] s)
        {
            foreach(string ss in s)
            {
                if (ss == null || ss.Trim().Length == 0)
                    return true;
            }
            return false;
        }

        private bool bothBad(params string[] s)
        {
            foreach (string ss in s)
            {
                if (!(s == null || ss.Trim().Length == 0))
                    return false;
            }
            return true;
        }

        private void privateKeyBrowse_FileOk(object sender, CancelEventArgs e)
        {
            config.sftpPrivateKeyPath = privateKeyBrowse.FileName;
        }

        private void browseKeyButton_Click(object sender, EventArgs e)
        {
            privateKeyBrowse.ShowDialog(this.Parent);
        }

        private void localFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowse.ShowDialog() == DialogResult.OK)
                config.localBaseFolder = folderBrowse.SelectedPath;
        }

        private void sftpCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach(Control c in groupBox1.Controls.Cast<Control>().Where((x) => x.Name.StartsWith("sftp")))
            {
                if(c != sftpCheckbox)
                    c.Enabled = sftpCheckbox.Checked;
            }
            if(sftpCheckbox.Enabled && sftpPortBox.Text == "0")
            {
                sftpPortBox.Text = "22";
            }
            browseKeyButton.Enabled = sftpCheckbox.Checked;
        }
    }
}
