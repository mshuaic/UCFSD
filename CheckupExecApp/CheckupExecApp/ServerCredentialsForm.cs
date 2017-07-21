using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckupExecApp
{
    public partial class ServerCredentialsForm : Form
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public ServerCredentialsForm()
        {
            InitializeComponent();
        }

        private void ServerCredentialsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void RemoteLoginCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            // If user checks "Remote Login", allow them to enter server credentials
            if(RemoteLoginCheckBox1.Checked)
            {
                ServerNameTextBox1.Enabled = true;
                UserNameTextBox1.Enabled = true;
                PasswordTextBox1.Enabled = true;
            }
            else if(!RemoteLoginCheckBox1.Checked)
            {
                ServerNameTextBox1.Enabled = false;
                UserNameTextBox1.Enabled = false;
                PasswordTextBox1.Enabled = false;
            }
        }

        private void LoginButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // Begin cursor loading animation
                Cursor.Current = Cursors.WaitCursor;

                // If user is logging in to server remotely
                if(RemoteLoginCheckBox1.Checked)
                {
                    // Pass in user-entered credentials
                    Form mainForm = new MainForm(true, PasswordTextBox1.Text, ServerNameTextBox1.Text, UserNameTextBox1.Text);
                    this.Hide();
                    mainForm.Show();
                }
                // If user is logging in to server locally
                else if(!RemoteLoginCheckBox1.Checked)
                {
                    Form mainForm = new MainForm(false, null, null, null);
                    this.Hide();
                    mainForm.Show();
                }

                // End cursor loading animation
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error with server login credentials.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error(ex.Message);
            }
            
        }

        // Cancels login and closes application
        private void CancelButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
