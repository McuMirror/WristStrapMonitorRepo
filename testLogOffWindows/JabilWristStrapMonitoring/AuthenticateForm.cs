using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JabilWristStrapMonitoring
{
    public partial class AuthenticateForm : Form
    {
        public bool successAuthenticate;
       
        public AuthenticateForm()
        {
            InitializeComponent();
          
            successAuthenticate = false;
            passTextBox.PasswordChar = '*';
            this.Focus();
            setActiveControlTextBox(ref passTextBox);
        }

        public void setActiveControlTextBox(ref TextBox incontrol, string from = "")
        {

            TextBox inrtbout = incontrol;
            //    Debug.Print(from + " " + DateTime.Now.ToLongTimeString());
            if (inrtbout.InvokeRequired)
            {
                inrtbout.Invoke((Action)delegate
                {
                    inrtbout.Focus();
                    // inrtbout.SelectAll();
                    this.ActiveControl = inrtbout;
                    //inrtbout.Focus = true;

                });
            }
            else
            {
                inrtbout.Focus();
                // inrtbout.SelectAll();
                this.ActiveControl = inrtbout;
            }
        }

        private void enterBtn_Click(object sender, EventArgs e)
        {
            if(passTextBox.Text.Equals("jabil123"))
            {
                successAuthenticate = true;
               
            }
            this.Close();
        }

        private void passTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                enterBtn.PerformClick();
                //enter key is down
            }
        }

        private void passTextBox_Leave(object sender, EventArgs e)
        {
            setActiveControlTextBox(ref passTextBox);
        }
    }
}
