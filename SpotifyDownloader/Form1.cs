using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YASDown
{
    public partial class Form1 : Form
    {
        private LoginControl loginCtrl = new LoginControl();
        private MainControl mainCtrl = new MainControl();
        private ConfigControl configCtrl = null;
        public bool LoggedIn { get; private set; }
        public AppConfig Config { get; set; }
        public Form1(bool loggedIn)
        {
            InitializeComponent();
            Session.OnLoggedOut += Session_OnLoggedOut;
            LoggedIn = loggedIn;
            AppConfig config = AppConfig.Load();
            Config = config;
            configCtrl = new ConfigControl(config);
            if (config == null)
            {
                LoadConfig();
            }
            else
            {
                if (loggedIn)
                {
                    LoadMain();
                }
                else
                {
                    LoadLogin();
                }
            }
        }

        void Session_OnLoggedOut(IntPtr obj)
        {
            Dispose();
            Application.Exit();
        }

        public void LoadMain()
        {
            LoadGeneric(mainCtrl, false, false, true);
        }

        public void LoadLogin()
        {
            LoadGeneric(loginCtrl, false, false, true);
        }

        public void LoadConfig()
        {
            LoadGeneric(configCtrl);
        }

        private void LoadGeneric(Control child, bool login = false, bool download = false, bool settings = false)
        {
            SuspendLayout();
            panel1.Controls.Clear();
            panel1.Controls.Add(child);
            loginToolStripMenuItem.Enabled = login;
            downloadToolStripMenuItem.Enabled = download;
            settingsToolStripMenuItem.Enabled = settings;
            ResumeLayout(false);
            PerformLayout();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLogin();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMain();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Session.IsLoggedIn)
            {
                e.Cancel = true;
                Session.Logout();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }
    }
}
