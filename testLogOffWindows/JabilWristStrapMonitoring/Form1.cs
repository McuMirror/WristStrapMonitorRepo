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
    public partial class MainForm : Form
    {
        public cWindowsIdentity wIdentity;
        public MainForm()
        {
            InitializeComponent();

            wIdentity = new cWindowsIdentity();

            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = false;

            Bitmap bm = new Bitmap( "jabil.png");
            pictureBox1.Image = bm;

            Bitmap channel1Passbm = new Bitmap("channel1Pass.png");
            Bitmap channel1Failbm = new Bitmap("channel1Fail.png");
            Bitmap channel1Disablebm = new Bitmap("channel1Disable.png");
            resultChannelOnePictureBox.Image = channel1Passbm;

            Bitmap channel2Passbm = new Bitmap("channel2Pass.png");
            Bitmap channel2Failbm = new Bitmap("channel2Fail.png");
            Bitmap channel2Disablebm = new Bitmap("channel2Disable.png");
            resultChannelTwoPictureBox.Image = channel2Disablebm;

            msgRtb.Text = "Test 123";

            wristStrapNotify.BalloonTipClosed += wristStrapNotify_BalloonTipClosed;
            wristStrapNotify.BalloonTipShown += wristStrapNotify_BalloonTipShown;

            wristStrapNotify.BalloonTipText = "Starting";
            wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";

            List<string> lstring = new List<string>();
            lstring.Add("init");
            ExportImportFile.StringArrayToFile(lstring, "log.txt",true);
        }

        void wristStrapNotify_BalloonTipShown(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void wristStrapNotify_BalloonTipClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void wristStrapNotify_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }

        private void setupBtn_Click(object sender, EventArgs e)
        {
                AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();

                if (aform.successAuthenticate == true)
                {
                    SetupForm sform = new SetupForm();
                    sform.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Wrong Password", "Error");
                }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            string currentUserName = cWindowsIdentity.GetCurrentLoginUserName();

            List<string> lstring = new List<string>();
            lstring.Add(currentUserName);
            ExportImportFile.StringArrayToFile(lstring, "log.txt", true);
        }

    }
}
