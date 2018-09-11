using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsyncTaskWithEvent;
using LogFiles;
using SerialLib;

using System.Messaging;
using Msmq;

using WindowsLogin;




namespace SerialPart
{
    public class CTest
    {
        static bool ConsoleWriteLineEnable = true;
        static bool LogWriteFileEnable = true;
        private static mAsyncTask mtask;
        static eTestState eState;
        static eWristState ewriststate;
        CLog log;
        ClassSerial mserial;
        CMsmqIPC cmsmq;

        WindowsLoginLib wlogin;


        private System.Diagnostics.EventLog eventLog1;

        public void AssignEventLog(ref System.Diagnostics.EventLog veventLog)
        {
            eventLog1 = veventLog;
        }

        enum eWristState
        {
            WristOn,
            WristOff,
            WristUnknown,
        }
        enum eTestState
        {
            initState,
            getUsbState,
            USBInit,
            USBInitWait,
            USBInitSuccess,
            USBInitFail,
            USBUnPlug,
            USBPlugIn,
            appExit,

        }

        private void ForCMsmqCallBack(string instr)
        {
            Msg("CMsmq:" + instr);
            if (instr.Contains("RequestWristState;"))
            {
                //  GetWristStrapState();


                switch (ewriststate)
                {
                    case eWristState.WristOff:
                        SendMsmqMsg("WristOff;");
                        break;
                    case eWristState.WristOn:
                        SendMsmqMsg("WristOn;");
                        break;
                    default:
                        SendMsmqMsg("WristError;");
                        break;
                }

            }
        }
        public void Init()
        {
            int a = 0;
            eState = eTestState.initState;
            ewriststate = eWristState.WristUnknown;
            log = new CLog();
            log.EnableLog = true;

            Msg("******  APP_START ******");
            Msg("LogFilePath : " + log.GetLogFileNamePath());
            mserial = new ClassSerial();
            mserial.AssignCallBackFunction(ForClassSerialCallBack);

            mtask = new mAsyncTask();
            mtask.AssignCallBackFunction(ForTaskCallBack);
            mtask.StartTask();

            cmsmq = new CMsmqIPC();
            cmsmq.AssignCallBackFunction(ForCMsmqCallBack);
            cmsmq.SetRole(eRole.IamSerialPart);

            wlogin = new WindowsLoginLib();
            wlogin.AssignCallBackFunction(ForWindowsLoginCallBack);
            wlogin.Init();

        }

        public void ForWindowsLoginCallBack(string instr)
        {
            Msg("WindowsLoginCallBack:" + instr);
        }
        public CTest()
        {

        }
        public void SendMsmqMsg(string instr)
        {
            if (cmsmq.erole == eRole.IamSerialPart)
            {
                cmsmq.sendData2Queue("SerialToUiTx:" + instr);
            }
            if (cmsmq.erole == eRole.IamUI)
            {
                cmsmq.sendData2Queue("UiToSerialTx:" + instr);
            }

        }
        private void ForClassSerialCallBack(string inStr)
        {
            inStr = inStr.Trim();
            Msg("ClassSerial:" + inStr);

            if (inStr.Equals("UsbCableUnplug;") && eState != eTestState.USBUnPlug)
            {
                this.SendMsmqMsg("UsbCableUnplug");
                eState = eTestState.USBUnPlug;
                ewriststate = eWristState.WristUnknown;
            }
            if (inStr.Equals("UsbCablePlugIn;") && (eState != eTestState.USBInitWait))
            {
                this.SendMsmqMsg("UsbCablePlugIn");
                eState = eTestState.USBPlugIn;
            }

            if (inStr.Contains("SerialDataReceived;"))
            {

            }
            if (eState == eTestState.USBInitWait)
            {
                if (inStr.Contains("COMPORT_OPEN_SUCCESS"))
                {
                    Msg("InitUSBSucess;");
                    GetWristStrapState();
                    eState = eTestState.USBInitSuccess;

                }
                if (inStr.Contains("COMPORT_NUMBER_NOT_FOUND"))
                {
                    Msg("InitUSBFail;");
                    SendMsmqMsg("InitUSBFail");
                    eState = eTestState.USBInitFail;
                }
            }
        }
        private void ForTaskCallBack(string taskResult)
        {
            System.Threading.Thread.Sleep(10);
            // Process Serial Query If have, at here
            if (!mserial.queryDone)
            {
                if (mserial.strRx.Count > 0)
                {

                    mserial.ProcessQuery(mserial.strRx[0]);
                }
                else
                {
                    mserial.ProcessQuery("null");
                }
            }
            if (taskResult.Length > 0)
            {
                Msg("ForTaskCallBack:" + taskResult);
            }
            if (mserial.strRx.Count > 0)
            {
                string sres = mserial.strRx[0];
                Msg("ClassSerial:Rx:" + mserial.strRx[0]);
                if ((sres.Contains("WristOn") == true) && (ewriststate != eWristState.WristOn))
                {
                    ewriststate = eWristState.WristOn;
                }
                if ((sres.Contains("WristOff")) && (ewriststate != eWristState.WristOff))
                {
                    // this.SendMsmqMsg("UsbCableUnplug");
                    ewriststate = eWristState.WristOff;
                }
                // consume here
                SendMsmqMsg(mserial.strRx[0]);
                mserial.strRx.RemoveAt(0);
            }
            //Console.WriteLine("test c=" +c.ToString());
            //log.LogWrite("test c=" + c.ToString());
            switch (eState)
            {
                case eTestState.initState:
                    Msg("InitState;");
                    eState = eTestState.USBInit;
                    break;
                case eTestState.USBInit:
                    Msg("InitUSB;");
                    eState = eTestState.USBInitWait;
                    mserial.Init();
                    break;
                case eTestState.USBInitSuccess:

                    break;
                case eTestState.appExit:
                    mtask.StopTask();
                    break;
                case eTestState.USBPlugIn:
                    Msg("InitAfterUsbPlugIn;");
                    eState = eTestState.USBInitWait;
                    mserial.Init();
                    break;
            }
        }
        private void Msg(string inStr)
        {
            if (eventLog1 != null)
            {
                eventLog1.WriteEntry(inStr);
            }

            if (LogWriteFileEnable)
            {
                log.WriteLog = inStr;
            }
            if (ConsoleWriteLineEnable)
            {
                Console.WriteLine(inStr);
            }
        }
        private void GetWristStrapState()
        {
            Msg("GetWristStrapState;");
            byte[] g = { 57, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //  vserial.sendBytes(g,1);

            List<string> s = new List<string>();
            s.Add("WristOff");
            s.Add("WristOn");
            s.Add("WristStrapDisconnect");
            s.Add("WristStrapConnect");
            s.Add("Calibration:On");
            s.Add("Calibration:Off");
            mserial.serialQuery(g, s, 100);
        }
    }
}
