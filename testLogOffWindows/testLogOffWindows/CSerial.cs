



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace testLogOffWindows
{
    class CSerial
    {
        private int error;
        private SerialPort mserial;
        private serialEventArgs args;
        private string rxstr;
        private string[] arrRxStr;
        private int arrRxStrSize;
        private Timer t;
        private bool debugMessageOn;
        public bool communicationErrorBool;

        public static bool IsPortAvailable(string portName)
        {
            // Retrieve the list of ports currently mounted by 
            // the operating system (sorted by name)
            string[] ports = SerialPort.GetPortNames();
            if (ports != null && ports.Length > 0)
            {
                return ports.Where(new Func<string, bool>((s) =>
                {
                    return s.Equals(portName,
                                    StringComparison.InvariantCultureIgnoreCase);
                })).Count() == 1;
            }
            return false;
        }
        public void setCommunicationError(bool inbool)
        {
            communicationErrorBool = inbool;
        }
        ~CSerial()
        {
            if (mserial.IsOpen)
            {
                this.ClosePort();
            }
        }
        public CSerial(bool debugOnOff)
        {
            communicationErrorBool = false;

            debugMessageOn = debugOnOff;
            mserial = new SerialPort();
            mserial.BaudRate = 9600;
            mserial.DataBits = 8;
            mserial.Parity = Parity.None;
            mserial.Handshake = Handshake.None;
            mserial.DtrEnable = false;  // For Arduino Leonardo 
            mserial.RtsEnable = false; // for using rs232          
            mserial.DataReceived += mserial_DataReceived;
            mserial.ErrorReceived += mserial_ErrorReceived;
            mserial.PinChanged += mserial_PinChanged;

            arrRxStr = new string[100];
            arrRxStrSize = 0;
            rxstr = "";
            args = new serialEventArgs();

            queryDone = 0x01;
            sxlist = new serialRxList[100];
             for (byte a = 0; a < 100; a++)
             {
                 sxlist[a] = new serialRxList();
             }
           //  

        }

        void mserial_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            try
            {
                string m = e.ToString();


            }
            catch (Exception f)
            {
                string errMsg = f.Message;

            }
        }

        void mserial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            try
            {
                string m =e.ToString();
             

            }
            catch (Exception f)
            {
                string errMsg = f.Message;
             
            }
           
        }

        private  void TimerCallback(Object o)
        {
            // Display the date/time when this method got called.
           // Console.WriteLine("In TimerCallback: " + DateTime.Now);

            ProcessQuery();

            // Force a garbage collection to occur for this demo.
            GC.Collect();
        }


        public void dbgMsg(string instr)
        {
            if(debugMessageOn)
            {
                string[] arrRxStr = new string[1];
                arrRxStr[0] = "DBG: " + instr;
                args.serialData.serialRxStr = arrRxStr;
                args.serialData.size = 1;
                OnSerialReached(args);
            }
          
        }

        private string queryExpectedReturn;
        private List<string> queryArrExpectedReturn;
        private int queryTimeOutMilliSec;
        private byte queryDone;
        private DateTime queryStartTime;

       // private List<string> querylist = new List<string>();
        
         private serialRxList[] sxlist;

        public void ProcessQuery()
        {
            if (queryDone == 0x00)
            {
                for(byte a = 0;a<100;a++)
                {
                   // dbgMsg("ProcessQuery:a=" + a.ToString() + ":str=" + sxlist[a].strRx);
                    if (sxlist[a].doneUsing == false)
                    {
                        foreach (string s in queryArrExpectedReturn)
                        {
                            if (sxlist[a].strRx.Contains(s) == true)
                            {
                                queryDone = 0x01;
                                dbgMsg("Query:Done;");
                                break;
                            }

                        }
                      
                    }
                    sxlist[a].doneUsing = true;
                }
                Double elapsedMillisecs = ((TimeSpan)(DateTime.Now - queryStartTime)).TotalMilliseconds;
                if (elapsedMillisecs > queryTimeOutMilliSec)
                {
                    dbgMsg("Query:TimeOut;");
                    queryDone = 0x01;
                }
            }
        }
        public void serialQuery(byte[] commandIn, string vqueryExpectedReturn, int vqueryTimeOutMilliSec)
        {
            if(!communicationErrorBool)
            {
                queryStartTime = DateTime.Now;
                queryDone = 0x00;
                queryExpectedReturn = vqueryExpectedReturn;
                queryTimeOutMilliSec = vqueryTimeOutMilliSec;
                sendBytes(commandIn);
            }
           
        }

        public void serialQuery(byte[] commandIn,int commandInSize,List<string> vqueryExpectedReturn, int vqueryTimeOutMilliSec)
        {
            if (!communicationErrorBool)
            {
                queryStartTime = DateTime.Now;
                queryDone = 0x00;
                queryArrExpectedReturn = vqueryExpectedReturn;
                queryTimeOutMilliSec = vqueryTimeOutMilliSec;
                sendBytes(commandIn, commandInSize);
            }
        }


        public class serialRxList
        {
            public string strRx;
            public bool doneUsing;
            public serialRxList()
            {
                strRx = "";
                doneUsing = true;
            }
        }

        public void ToggleDtr()
        {
            mserial.DtrEnable = true;
            //System.Threading.Thread.Sleep(100);
            mserial.DtrEnable = false;
        }
        public void sendBytes(byte[] inbytes)
        {
            if (!communicationErrorBool)
            {
                Byte[] commandBytes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int a = 0; a < 10; a++)
                {
                    commandBytes[a] = inbytes[a];
                }
                mserial.Write(commandBytes, 0, 10);
            }
          
        }
        public void sendBytes(byte[] inbytes, int size)
        {
            if (!communicationErrorBool)
            {
                mserial.Write(inbytes, 0, size);
            }
           
        }
        void mserial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
             int commaCount = 0;
             int inbytelen = mserial.BytesToRead;
             if (inbytelen > 0)
             {
                 byte[] inbyte = new byte[inbytelen];
                 for (int b = 0; b < inbytelen; b++)
                 {
                     // int tint = mserial.ReadByte();
                     char temp = Convert.ToChar(mserial.ReadByte());
                     rxstr = rxstr + temp.ToString();
                     if (temp == ';')
                     {
                         commaCount++;
                     }
                 }
             }
             string[] stemp = new string[100];

             if (commaCount > 0)
             {
                 stemp = rxstr.Split(';');
                 arrRxStrSize = stemp.Length;
                 for (int a = 0; a < commaCount; a++)
                 {
                     arrRxStr[a] = stemp[a].Trim();

                     if (queryDone == 0x00)
                     {
                         for (byte b = 0; b < 100; b++)
                         {
                             if (sxlist[b].doneUsing == true)
                             {
                                 sxlist[b].strRx = arrRxStr[a];
                                 sxlist[b].doneUsing = false;
                                 b = 200; // to exit
                             }
                             if (b == 99)
                             {
                                 dbgMsg("sxlist:OverFlow;");
                             }
                         }
                     }
                   //  rxSerialList.Add(sx);
                 }
                 if (commaCount != arrRxStrSize)
                 {
                     rxstr = "";
                     rxstr = stemp[arrRxStrSize - 1];
                 }
                 else
                 {
                     rxstr = "";
                 }
                 args.serialData.serialRxStr = arrRxStr;
                 args.serialData.size = commaCount;
                 OnSerialReached(args);
             }
        }
        public void ClosePort()
        {
            mserial.DiscardInBuffer();
            mserial.DiscardOutBuffer();
           if (!communicationErrorBool)
            {
                mserial.Close();
            }
           
        }
        public int OpenPort(string comPort)
        {
            error = 0x00;
            try
            {
               
                if (!mserial.IsOpen)
                {
                    error = 0x00;
                    mserial.PortName = comPort;
                    mserial.Open();
                    mserial.DiscardInBuffer();
                    mserial.DiscardOutBuffer();
                    if (!mserial.IsOpen)
                    {
                        error = 0x01;
                        return error;
                    }
                }
            }
            catch (Exception e)
            {
                string errMsg = e.Message;
                error = 0x02;
                return error;
            }
            t = new Timer(TimerCallback, null, 0, 500);
            return error;
        }
        protected virtual void OnSerialReached(serialEventArgs e)
        {  // event for update UI
            EventHandler<serialEventArgs> handler = serialReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<serialEventArgs> serialReached;
    }
    public class serialEventArgs : EventArgs
    {
        public serialDt serialData { get; set; }
        public serialEventArgs()
        {
            serialData = new serialDt();
        }
    }
    public class serialDt
    {
        public string [] serialRxStr;
        public int size;
        public serialDt()
        {
            serialRxStr = new string[10];
        }
    }

}

