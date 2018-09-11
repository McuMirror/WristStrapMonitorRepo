using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.IO;

using System.Management; // need to add System.Management to your project references.


using Microsoft.Win32;


namespace JabilWristStrapMonitoring
{
    public class cWindowsIdentity
    {
        public sWindowsState mWindowsState;
        public cWindowsIdentity()
        {
            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            mWindowsState = sWindowsState.SessionUnknow;
        }
        public enum sWindowsState
        {
            SessionUnknow,
            SessionLock,
            SessionUnLock,
            SessionLogoff,
            SessionLogon,
        };
       
        public static bool IsAdministrator()
        {
            // Get Identity:
            WindowsIdentity user = WindowsIdentity.GetCurrent();

            // Set Principal
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            return isAdmin;
        }
        public static string GetCurrentLoginUserName()
        {
            return WindowsIdentity.GetCurrent().Name; 
        }
        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            string currentUserName = cWindowsIdentity.GetCurrentLoginUserName();
           
            List<string> lstring = new List<string>();

            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleDisconnect)
            { // lock
                mWindowsState = sWindowsState.SessionLock;
                lstring.Add(currentUserName + " lock");
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleConnect)
            { // unlock
                mWindowsState = sWindowsState.SessionUnLock;
                lstring.Add(currentUserName + " unlock");
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogoff)
            {  // logoff
                mWindowsState = sWindowsState.SessionLogoff;
                lstring.Add(currentUserName + " logOff");
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogon)
            {  // login
                mWindowsState = sWindowsState.SessionLogon;
                lstring.Add(currentUserName + " logIn");
            }
            ExportImportFile.StringArrayToFile(lstring, "log.txt", true);
        }

    }
}
