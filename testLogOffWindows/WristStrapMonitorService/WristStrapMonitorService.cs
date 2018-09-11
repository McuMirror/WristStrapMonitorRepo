using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


using AsyncTaskWithEvent;
using LogFiles;
using SerialLib;



/*
 *      installutil.exe  MyNewService.exe  
 *      installutil.exe /u MyNewService.exe  
 * */


namespace WristStrapMonitorService
{
    public partial class WristStrapMonitorService : ServiceBase
    {
       // private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

       

        private void ForClassSerialCallBack(string inStr)
        {
            eventLog1.WriteEntry("ClassSerial:" + inStr);
        }

        public WristStrapMonitorService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("JabilWristStrapLog"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "JabilWristStrapLog", "JabilWristStrapNewLog");
            }
            eventLog1.Source = "JabilWristStrapLog";
            eventLog1.Log = "JabilWristStrapNewLog";
        }
        public CTest mtest;
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;

            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        //    System.Timers.Timer timer = new System.Timers.Timer();
        //    timer.Interval = 10000; // 60 seconds  
       //     timer.Elapsed += Timer_Elapsed; 
        //    timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart");

            CTest mtest = new CTest();
            mtest.AssignEventLog(ref eventLog1);
            mtest.Init();
            
        }

        private int eventId = 1;

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        protected override void OnStop()
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In onStop.");
        }


        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

    }
}
