using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;


namespace Msmq
{
    class CMsmqIPC : IDisposable
    {
        public delegate void TaskCompletedCallBack(string taskResult);
        private TaskCompletedCallBack callback;
        public void AssignCallBackFunction(TaskCompletedCallBack taskCompletedCallBack)
        {
            callback = taskCompletedCallBack;
        }
        public eRole erole;
        private MessageQueue msgQRx;
        private MessageQueue msgQTx;
        private bool initSuccess = false;




        private bool IsDisposed = false;

        public void Free()
        {
            if (IsDisposed)
                throw new System.ObjectDisposedException("Object Name");
        }

        //Call Dispose to free resources explicitly

        public void Dispose()
        {

            //Pass true in dispose method to clean managed resources too and say GC to skip finalize 
            //in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        ~CMsmqIPC()
        {
            //Pass false as param because no need to free managed resources when you call finalize it
            //will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);
        }



        //Implement dispose to free resources

        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
             
                if (disposedStatus)
                {
                    // Released managed Resources
                    msgQRx.ReceiveCompleted -= msgQRx_ReceiveCompleted;
                    msgQRx.Close();
                    //    msgQRx.Dispose();
                    msgQTx.Close();
                    //    msgQTx.Dispose();
                }
            }
        }















      
        public CMsmqIPC()
        {
            try
            {
                msgQRx = new MessageQueue();
                msgQTx = new MessageQueue();
            }
            catch (Exception e)
            {
                callback("Exception:" + e.Message);
            }
        }
        public void sendData2Queue(string instr)
        {
            if (initSuccess)
            {
                msgQTx.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                //   billingQ.ReceiveCompleted += billingQ_ReceiveCompleted;
                //Here i am just sending the current date as message..  u can send customer mailid's in the queue
                //string strTx = DateTime.Now.ToString();
                callback("SendMessageSuccess:" + instr);


                msgQTx.Send(instr);
                //   billingQ.BeginReceive();
                msgQTx.Close();
            }
            else
            {
                callback("SendMsgNotAllow;");
            }


        }
        void msgQRx_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = msgQRx.EndReceive(e.AsyncResult);
                string data = msg.Body.ToString();
                // Here just showing the sent message, but u can set the logic of sending mail over here as u will get customer email
                //id in the message
                callback("Rx:" + data);
                //Restart the asynchronous receive operation.
                msgQRx.BeginReceive();
            }
            catch (Exception er)
            {
                callback("ExceptionHit:" + er.Message);
            }
        }
    
        public void SetRole(eRole erole)
        {
            try
            {
                string SerialToUi = @".\private$\SerialToUi";
                string UiToSerial = @".\private$\UiToSerial";
                if (erole == eRole.IamSerialPart)
                {
                    // Define SerialPart receive    
                    msgQRx.Path = UiToSerial;
                    if (!MessageQueue.Exists(msgQRx.Path))
                    {
                        callback("QueueNotExistCreateNow:" + msgQRx.Path + ";");
                        MessageQueue.Create(msgQRx.Path);
                    }
                    else
                    {
                        callback("QueueExist:" + msgQRx.Path + ";");
                    }
                    msgQRx.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    msgQRx.ReceiveCompleted += msgQRx_ReceiveCompleted;
                    //Here i am just sending the current date as message..  u can send customer mailid's in the queue
                    // billingQ.Send(DateTime.Now.ToString());
                    msgQRx.BeginReceive();
                   
                    //   billingQ.Close();
                    // Define SerialPart transmit
                    msgQTx.Path = SerialToUi;
                    if (!MessageQueue.Exists(msgQTx.Path))
                    {
                        callback("QueueNotExistCreateNow:" + msgQTx.Path + ";");
                        MessageQueue.Create(msgQTx.Path);
                    }
                    else
                    {
                        callback("QueueExist:" + msgQTx.Path + ";");
                    }
                    initSuccess = true;
                }

                if (erole == eRole.IamUI)
                {
                    //define UI received  
                    msgQRx.Path = SerialToUi;
                    msgQRx.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    msgQRx.ReceiveCompleted += msgQRx_ReceiveCompleted;
                    //Here i am just sending the current date as message..  u can send customer mailid's in the queue
                    // billingQ.Send(DateTime.Now.ToString());
                    msgQRx.BeginReceive();
                    // define UI transmit
                    msgQTx.Path = UiToSerial;
                    if (!MessageQueue.Exists(msgQTx.Path))
                    {
                        callback("QueueNotExistCreateNow:" + msgQTx.Path + ";");
                        MessageQueue.Create(msgQTx.Path);
                    }
                    else
                    {
                        callback("QueueExist:" + msgQTx.Path + ";");
                    }
                    initSuccess = true;
                }
            }
            catch (Exception e)
            {
                callback("Exception:" + e.Message);
            }
        }
    }
    enum eRole
    {
        IamSerialPart,
        IamUI,
    };
}
