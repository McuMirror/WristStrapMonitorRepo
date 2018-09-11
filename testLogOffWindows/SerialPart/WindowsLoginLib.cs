using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Win32;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;


namespace WindowsLogin
{
    class WindowsLoginLib
    {
        public string currentUserName = "null";
        public string GetCurrentLoginUserName()
        {
            return WindowsIdentity.GetCurrent().Name; ;
        }
        private eLoginState eloginState;

        public WindowsLoginLib()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }
        public void Init()
        {
            try
            {
                currentUserName = "null";
                currentUserName = this.GetCurrentLoginUserName();
                callback("CurrentUser:" + currentUserName + ";");
            }
            catch(Exception e)
            {
                callback("WindowsLoginLib:ExceptionHit:" + e.Message + ";");
            }

           
        }



        enum eLoginState
        {
            slock,
            sunlock,
            sunknown,
            sidle,
            slogOff,
            slogOn,
        };

        public delegate void TaskCompletedCallBack(string taskResult);
        private TaskCompletedCallBack callback;

        public void AssignCallBackFunction(TaskCompletedCallBack taskCompletedCallBack)
        {
            callback = taskCompletedCallBack;
        }


        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            currentUserName = GetCurrentLoginUserName();

            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleDisconnect)
            {
                callback("UserLock:"+currentUserName+";");
                eloginState = eLoginState.slock;
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleConnect)
            {
                callback("UserUnLock:" + currentUserName + ";");
                eloginState = eLoginState.sunlock;
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogoff)
            {
                callback("UserLogOff:" + currentUserName + ";");
                eloginState = eLoginState.slogOff;
                // msg("LogOff");
                //outRtb(ref msgRtb, "LogOff\n", true);
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogon)
            {
                callback("UserLogOn:" + currentUserName + ";");
                eloginState = eLoginState.slogOn;
                // msg("LogIn");
                //outRtb(ref msgRtb, "LogIn\n", true);
            }
        }
    }
}
