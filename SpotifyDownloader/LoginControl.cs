using System;
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
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool? b = null;
            string username = usernameBox.Text, password = passwordBox.Text;
            Program.nito.DoSynchronously(() =>
            {
                b = Session.Login(username, password);
            });

            if(b == true)
            {
                ((Form1)Parent.Parent).LoadMain();
            }
            else
            {
                MessageBox.Show("Invalid credentials");
            }
        }
    }
}
