using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Messaging;

namespace MSMQDemo
{
    public partial class Form1 : Form
    {
        MessageQueue billingQ = new MessageQueue();

        public Form1()
        {
            InitializeComponent();
            billingQ.Path = @".\private$\bills";
        }

        private void btnQueudata_Click(object sender, EventArgs e)
        {
            if (MessageQueue.Exists(billingQ.Path))
            {
                sendData2Queue();

            }
            else
            {
                // Creates the queue named "Bills"
                MessageQueue.Create(billingQ.Path);
                sendData2Queue();
            }


        }

        private void sendData2Queue()
        {

            billingQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            billingQ.ReceiveCompleted += billingQ_ReceiveCompleted;
            //Here i am just sending the current date as message..  u can send customer mailid's in the queue
            billingQ.Send(DateTime.Now.ToString());

            billingQ.BeginReceive();

            billingQ.Close();

        }
        void billingQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {

            try
            {
                var msg = billingQ.EndReceive(e.AsyncResult);
                string data = msg.Body.ToString();
                // Here just showing the sent message, but u can set the logic of sending mail over here as u will get customer email
                //id in the message
                MessageBox.Show(data);
                //Restart the asynchronous receive operation.
                billingQ.BeginReceive();
            }
            catch (MessageQueueException qexception)
            { }

        }
    }
}
