
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

using System.Management; // need to add System.Management to your project references.
//using WristStrapMonitorService;
using USBUtility;
using LogFiles;
namespace SerialLib
{
    public class ClassSerial
    {
        private SerialPort mserial;
        private string sComPort;

        public delegate void ClassSerialTaskCompletedCallBack(string taskResult);
        private ClassSerialTaskCompletedCallBack callback;
        private eProgramState epState;
        public List<string> strRx;
        private string strBufRx;


        private ManagementEventWatcher pluggedInWatcher;
        private ManagementEventWatcher pluggedOutWatcher;
        private string deviceID = "VID_04B4&PID_0002|VID_04D8&PID_000A|VID_0000&PID_0000";


        enum eProgramState
        {
            UnknownState,
            COMPORT_FAIL_OPEN,
            COMPORT_NUMBER_NOT_FOUND,
            COMPORT_OPENED,
            UsbUnPlug,
            UsbPlugIn,
        }




        private void DevicePluggedInEventReceived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];


                string[] deviceArr = deviceID.Split('|');
                int deviceArrLen = deviceArr.Length;

                for (int a = 0; a < deviceArrLen; a++)
                {
                    if (instance.Properties["DeviceID"].Value.ToString().Contains(deviceArr[a]))
                    {
                        epState = eProgramState.UsbPlugIn;
                        /*
                        // if (vserial.communicationErrorBool == true)
                        {
                            vserial.communicationErrorBool = false;
                            //OnDevicePluggedIn(EventArgs.Empty);
                            // AppendToLogFile("USB_PLUG_IN_DETECTED");





                            string ard1 = "USB Serial Port";
                            string ard2 = "USB Serial Port";
                            string ard3 = "def";
                            string ard = ard1 + "|" + ard2 + "|" + ard3;
                            // string ardComPortNo = "";
                            finalComPortNumber = "null";
                            finalComPortNumber = doGetUsb(ard);//,ref outDeviceName);
                            string instanceId = doGetUsbDeviceId(ard, deviceID);
                            // bool resetResult = PortHelper.TryResetPortByInstanceId(@"USB\VID_04D8&PID_000A\9&22F2C77B&0&4");

                            bool portSuccess = false;
                            while (!portSuccess)
                            {
                                // bool resetResult = PortHelper.TryResetPortByName(finalComPortNumber);
                                bool resetResult = PortHelper.TryResetPortByInstanceId(instanceId);
                                System.Threading.Thread.Sleep(5000);
                                portSuccess = CSerial.IsPortAvailable(finalComPortNumber);
                            }

                            //TryResetPortByName
                            if (finalComPortNumber.Equals("null"))
                            {
                                msg("COM Port Error");
                                vserial.setCommunicationError(true);
                                //   getWristStrapStateBtn.Enabled = false;
                                //   calibrationBtn.Enabled = false;
                            }
                            else
                            {
                                int err = vserial.OpenPort(finalComPortNumber);
                                if (err > 0)
                                {
                                    msg("COM Port Error");
                                    vserial.setCommunicationError(true);
                                    //   getWristStrapStateBtn.Enabled = false;
                                    //   calibrationBtn.Enabled = false;
                                }
                                else
                                {
                                    msg("COM Port Opened Success");
                                    comPortOpenedSuccess = true;
                                    vserial.setCommunicationError(false);

                                    //    getWristStrapStateBtn.Enabled = true;
                                    //    calibrationBtn.Enabled = true;
                                    startStuffHere();
                                }
                            }

                            if (comPortOpenedSuccess == false)
                            {
                                outLabel(ref taskMessageLabel, "Com Port Fail,Please call support", false);


                                return;
                            }
                        }

                        */
                    }
                }


            }
            catch (Exception g)
            {
                // HandleCatchMessage(ref g);

            }
            callback("UsbCablePlugIn;");
            //msg("USB cable plug in detected");

        }
        private void DevicePluggedOutEventReceived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                //    if (vserial.communicationErrorBool == false)
                {
                    string[] deviceArr = deviceID.Split('|');
                    int deviceArrLen = deviceArr.Length;

                    for (int a = 0; a < deviceArrLen; a++)
                    {
                        if (instance.Properties["DeviceID"].Value.ToString().Contains(deviceArr[a]))
                        {

                            // getWristStrapStateBtn.Enabled = false;
                            // calibrationBtn.Enabled = false;
                            epState = eProgramState.UsbPlugIn;

                            callback("UsbCableUnplug;");// ("USB cable unplug detected");
                                                        //OnDevicePluggedIn(EventArgs.Empty);
                                                        //  AppendToLogFile("USB_PLUG_OUT_DETECTED");
                                                        //   vserial.setCommunicationError(true);
                                                        //   vserial.ClosePort();
                                                        //   LogOff();
                        }
                    }
                    //   vserial.communicationErrorBool = true;
                }
            }
            catch (Exception g)
            {
                //  HandleCatchMessage(ref g);

            }
        }
        private void SetupPnPEventWatcher()
        {
            string queryStr = "SELECT * FROM __InstanceCreationEvent WITHIN 2 "
                    + "WHERE TargetInstance ISA 'Win32_PnPEntity'";

            // string queryStr = @"Select * From Win32_PnPEntity WHERE Name LIKE '%(COM[0-9]%'";

            pluggedInWatcher = new ManagementEventWatcher(queryStr);
            pluggedInWatcher.EventArrived += new EventArrivedEventHandler(DevicePluggedInEventReceived);
            queryStr = "SELECT * FROM __InstanceDeletionEvent WITHIN 2 "
                 + "WHERE TargetInstance ISA 'Win32_PnPEntity'";

            pluggedOutWatcher = new ManagementEventWatcher(queryStr);
            pluggedOutWatcher.EventArrived += new EventArrivedEventHandler(DevicePluggedOutEventReceived);

        }

        private string queryExpectedReturn;
        private List<string> queryArrExpectedReturn;
        private int queryTimeOutMilliSec;
        public bool queryDone;
        private DateTime queryStartTime;

        public void serialQuery(byte[] commandIn, List<string> vqueryExpectedReturn, int vqueryTimeOutMilliSec)
        {
            try
            {
                if (epState == eProgramState.COMPORT_OPENED)
                {
                    queryStartTime = DateTime.Now;
                    queryDone = false;
                    queryArrExpectedReturn = vqueryExpectedReturn;
                    queryTimeOutMilliSec = vqueryTimeOutMilliSec;
                    sendBytes(commandIn, commandIn.Length);
                }
                else
                {
                    callback("SerialQueryNotAllowAsComPortFailed");
                }
            }
            catch (Exception e)
            {
                callback("ExceptionHit:" + e.Message + ";");
            }

        }
        public void sendBytes(byte[] inbytes)
        {
            if (epState == eProgramState.COMPORT_OPENED)
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
            if (epState == eProgramState.COMPORT_OPENED)
            {
                mserial.Write(inbytes, 0, size);
            }

        }











        public void AssignCallBackFunction(ClassSerialTaskCompletedCallBack ClassSerialtaskCompletedCallBack)
        {
            callback = ClassSerialtaskCompletedCallBack;
        }
        public ClassSerial()
        {
            queryDone = true;
            epState = eProgramState.UnknownState;
            strRx = new List<string>();
            mserial = new SerialPort();
            strBufRx = "";
            // sComPort = USBTools.doGetUsb(usbDeviceString);
        }

        public void Init()
        {
            try
            {

                SetupPnPEventWatcher();
                pluggedInWatcher.Start();
                pluggedOutWatcher.Start();


                string deviceID = "VID_04B4&PID_0002|VID_04D8&PID_000A|VID_0000&PID_0000";
                string ard1 = "USB Serial Port";
                string ard2 = "USB Serial Port";
                string ard3 = "def";
                string ard = ard1 + "|" + ard2 + "|" + ard3;
                // string ardComPortNo = "";
                sComPort = "null";
                sComPort = USBTools.doGetUsb2(ard, deviceID);//,ref outDeviceName);
                if (!sComPort.Contains("COM"))
                {
                    epState = eProgramState.COMPORT_NUMBER_NOT_FOUND;
                    callback("COMPORT_NUMBER_NOT_FOUND;");
                    return;
                }
                //callback("COMPORT_OPEN:SUCCESS:" + sComPort + ";");
                if (this.OpenPort(sComPort) == 0x01)
                {
                    epState = eProgramState.COMPORT_FAIL_OPEN;
                    callback("ERROR:COMPORT_OPEN_FAIL;");
                    return;
                }
                else
                {
                    callback("COMPORT_OPEN_SUCCESS:" + sComPort + ";");
                }
            }
            catch (Exception e)
            {
                callback("ClassSerial:ExceptionHit:" + e.Message + ";");
            }


        }
        public int OpenPort(string comPort)
        {
            int error = 0x00;
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
                    else
                    {
                        epState = eProgramState.COMPORT_OPENED;
                        mserial.DataReceived += Mserial_DataReceived;
                    }
                }
            }
            catch (Exception e)
            {
                string errMsg = e.Message;
                error = 0x02;
                return error;
            }
            return error;
        }

        public void ProcessQuery(string instr)
        {
            if (!queryDone)
            {
                foreach (string s in queryArrExpectedReturn)
                {
                    if (instr.Contains(s) == true)
                    {
                        queryDone = true;
                        callback("Query:Done;");
                        break;
                    }
                }
                Double elapsedMillisecs = ((TimeSpan)(DateTime.Now - queryStartTime)).TotalMilliseconds;
                if (elapsedMillisecs > queryTimeOutMilliSec)
                {
                    callback("Query:TimeOut;");
                    queryDone = true;
                }
            }
        }


        private void Mserial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            bool returnNeeded = false;
            int inbytelen = mserial.BytesToRead;
            if (inbytelen > 0)
            {
                byte[] inbyte = new byte[inbytelen];
                for (int b = 0; b < inbytelen; b++)
                {
                    char temp = Convert.ToChar(mserial.ReadByte());
                    strBufRx = strBufRx + temp.ToString();
                    if (temp == ';')
                    {

                        strRx.Add(strBufRx.Trim());
                        strBufRx = "";
                        returnNeeded = true;
                    }
                }
            }
            if (returnNeeded)
            {
                callback("SerialDataReceived;");
            }
        }



    }
}

