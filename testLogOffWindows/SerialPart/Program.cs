using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using System.Runtime.InteropServices;

using AsyncTaskWithEvent;
using LogFiles;
using SerialLib;




namespace SerialPart
{
    public class Program
    {
        public CTest mtest;

        private static void Main(string[] args)
        {
            testGithub.TestGithub();
            CTest mtest = new CTest();
            mtest.Init();
            int a = 0;
            while(true)
            {
                //mtest.SendMsmqMsg("SerialToUi:" + a.ToString());
                System.Threading.Thread.Sleep(1000);
                a++;
            }
        }
        /*
        public class CTest
        {
            static bool ConsoleWriteLineEnable = true;
            static bool LogWriteFileEnable = true;
            private static mAsyncTask mtask;
            static eTestState eState;
            CLog log;
            ClassSerial mserial;

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

            public CTest()
            {
                int a = 0;
                eState = eTestState.initState;
                log= new CLog();
                log.EnableLog = true;
                Msg("******  APP_START ******");
                mserial = new ClassSerial();
                mserial.AssignCallBackFunction(ForClassSerialCallBack);

                mtask = new mAsyncTask();
                mtask.AssignCallBackFunction(ForTaskCallBack);
                mtask.StartTask();

                while (!mtask.TaskIsCompleted())
                {
                   System.Threading.Thread.Sleep(1000);
                    GetWristStrapState();
                //    log.WriteLog = a.ToString();
                 //   a++;
                }
                Msg("******  APP_STOP ******");
                //  Console.Read();
                //  mtask.StopTask();
            }

            private void ForClassSerialCallBack(string inStr)
            {
                Msg("ClassSerial:" + inStr);
                if(inStr.Equals("UsbCableUnplug;"))
                {
                    eState = eTestState.USBUnPlug;
                }
                if (inStr.Equals("UsbCablePlugIn;") && (eState != eTestState.USBInitWait) )
                {
                    eState = eTestState.USBPlugIn;
                }

                if (inStr.Contains("SerialDataReceived;")) 
                {
                   
                }
                if(eState == eTestState.USBInitWait)
                {
                    if(inStr.Contains("COMPORT_OPEN_SUCCESS"))
                    {
                        Msg("InitUSBSucess;");
                        eState = eTestState.USBInitSuccess;
                    }
                    if (inStr.Contains("COMPORT_NUMBER_NOT_FOUND"))
                    {
                        Msg("InitUSBFail;");
                        eState = eTestState.USBInitFail;
                    }
                }
            }
            private void ForTaskCallBack(string taskResult)
            {
                System.Threading.Thread.Sleep(10);

                // Process Serial Query If have, at here
                if(!mserial.queryDone)
                {
                    if(mserial.strRx.Count>0)
                    {
                        mserial.ProcessQuery(mserial.strRx[0]);
                    }
                    else
                    {
                        mserial.ProcessQuery("null");
                    }
                }
                if(taskResult.Length>0)
                {
                    Msg("ForTaskCallBack:" + taskResult);
                }
                if (mserial.strRx.Count > 0)
                {
                    Msg("ClassSerial:Rx:"+mserial.strRx[0]);
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
                if(LogWriteFileEnable)
                {
                    log.WriteLog = inStr;
                }
                if(ConsoleWriteLineEnable)
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
        */
    }
}
