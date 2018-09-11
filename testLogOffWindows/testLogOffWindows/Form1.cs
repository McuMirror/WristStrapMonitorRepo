using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.IO;

using System.Management; // need to add System.Management to your project references.


using Microsoft.Win32;

using Devices;




namespace testLogOffWindows
{
     
     
    public partial class Form1 : Form
    {
        CSerial vserial;
       // private CSerial.serialRxList[] sxlist;






        string finalComPortNumber;



        sUserState vuState;
        sTask vtask;
        DateTime startLogOffTime;
        int delayLogOffMilliSec = 20000;
        bool enableMonitoring = true;
        string toolTipStr = "Initializing Wrist Strap Monitor";
        bool ballonShowState = false;
        int ballonShowTime = 3;

        bool calibrationModeStatus;
        int calSampleSizeNeeded;
        int calSampleSizeCount;
        int calMin;
        int calMax;
        int calNom;
        int [] calDiff;

        string currentUserName = "null";

        private ManagementEventWatcher pluggedInWatcher;
        private ManagementEventWatcher pluggedOutWatcher;
        public string deviceID;

        bool comPortOpenedSuccess;

        string logFileNameByDay;
        DateTime logDate;


        enum sUserState
        {
            slock,
            sunlock,
            sunknown,
            sidle,
        };
        enum sTask
        {
            serialCommunicationError,
            initState,
            idle,
            logOff,
        };

        class USBDeviceInfo
        {
            public USBDeviceInfo(string Name, string deviceID, string pnpDeviceID, string description)
            {
                this.Name = Name;
                this.DeviceID = deviceID;
                this.PnpDeviceID = pnpDeviceID;
                this.Description = description;
            }
            public string Name { get; private set; }
            public string DeviceID { get; private set; }
            public string PnpDeviceID { get; private set; }
            public string Description { get; private set; }
        }
        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            //  using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
            //"Win32_SerialPort";


            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity WHERE Name LIKE '%(COM[0-9]%'"))
                //  using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_SerialPort"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                     (string)device.GetPropertyValue("Name"),
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }
        private string doGetUsb(string arrDeviceName)
        {

            string[] DevicesName = arrDeviceName.Split('|');
            int DevicesNameLen = DevicesName.Length;
            bool found = false;
            // use comport_out

            string detected_com_number = "null";
            var usbDevices = GetUSBDevices();
            string susb = "";

            foreach (var usbDevice in usbDevices)
            {
                susb = susb + "|" + usbDevice.Name + "|" + usbDevice.DeviceID + "|" + usbDevice.PnpDeviceID + "|" + usbDevice.Description + "\n";
                for (int c = 0; c < DevicesNameLen; c++)
                {
                    if (usbDevice.Name.Contains(DevicesName[c]) == true) 
                    {
                        string tmp = usbDevice.Name.ToString();
                        string deviceComPort = tmp.Substring(tmp.LastIndexOf("COM"));
                        deviceComPort = deviceComPort.Substring(0, deviceComPort.Length - 1);
                        detected_com_number = deviceComPort;
                        found = true;
                    }
                }

                //Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                //  usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }
            // string tstr = serial_no + "|" + dut_com_port_no;
            // return flasher_comport;
            if (found == true)
            {
                return detected_com_number;
            }
            else
            {
                return "Error";
            }


        }
        private string doGetUsb2(string arrDeviceName,string deviceId)
        {
            string[] deviceIdArr = deviceId.Split('|');

            string[] DevicesName = arrDeviceName.Split('|');
            int DevicesNameLen = DevicesName.Length;
            bool found = false;
            // use comport_out

            string detected_com_number = "null";
            var usbDevices = GetUSBDevices();
            string susb = "";

            foreach (var usbDevice in usbDevices)
            {
                susb = susb + "|" + usbDevice.Name + "|" + usbDevice.DeviceID + "|" + usbDevice.PnpDeviceID + "|" + usbDevice.Description + "\n";
                for (int c = 0; c < DevicesNameLen; c++)
                {
                    if ((usbDevice.Name.Contains(DevicesName[c]) == true) && (usbDevice.DeviceID.Contains(deviceIdArr[c]) == true))
                    {
                        string tmp = usbDevice.Name.ToString();
                        string deviceComPort = tmp.Substring(tmp.LastIndexOf("COM"));
                        deviceComPort = deviceComPort.Substring(0, deviceComPort.Length - 1);
                        detected_com_number = deviceComPort;
                        found = true;
                    }
                }

                //Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                //  usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }
            // string tstr = serial_no + "|" + dut_com_port_no;
            // return flasher_comport;
            if (found == true)
            {
                return detected_com_number;
            }
            else
            {
                return "Error";
            }


        }
        private string doGetUsbDeviceId(string arrDeviceName, string deviceId)
        {
            string[] deviceIdArr = deviceId.Split('|');

            string[] DevicesName = arrDeviceName.Split('|');
            int DevicesNameLen = DevicesName.Length;
            bool found = false;
            // use comport_out

            string detected_com_number = "null";
            string detectedInstaceId = "null";
            var usbDevices = GetUSBDevices();
            string susb = "";

            foreach (var usbDevice in usbDevices)
            {
                susb = susb + "|" + usbDevice.Name + "|" + usbDevice.DeviceID + "|" + usbDevice.PnpDeviceID + "|" + usbDevice.Description + "\n";
                for (int c = 0; c < DevicesNameLen; c++)
                {
                    if ((usbDevice.Name.Contains(DevicesName[c]) == true) && (usbDevice.DeviceID.Contains(deviceIdArr[c]) == true))
                    {
                        string tmp = usbDevice.Name.ToString();
                        string deviceComPort = tmp.Substring(tmp.LastIndexOf("COM"));
                        deviceComPort = deviceComPort.Substring(0, deviceComPort.Length - 1);
                        detected_com_number = deviceComPort;
                        detectedInstaceId = usbDevice.DeviceID;
                        found = true;
                    }
                }

                //Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                //  usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }
            // string tstr = serial_no + "|" + dut_com_port_no;
            // return flasher_comport;
            if (found == true)
            {
                return detectedInstaceId;
            }
            else
            {
                return "Error";
            }


        }

        private void InitializeComPort()
        {
            deviceID = "VID_04B4&PID_0002|VID_04D8&PID_000A|VID_0000&PID_0000";

            string ard1 = "USB Serial Port";
            string ard2 = "USB Serial Port";
            string ard3 = "def";
            string ard = ard1 + "|" + ard2 + "|" + ard3;
            // string ardComPortNo = "";
            finalComPortNumber = "null";
            finalComPortNumber = doGetUsb2(ard, deviceID);//,ref outDeviceName);

            string instanceId = doGetUsbDeviceId(ard, deviceID);

          //  bool resetResult = PortHelper.TryResetPortByName(finalComPortNumber);
            //TryResetPortByName

            if (finalComPortNumber.Equals("null") || finalComPortNumber.Equals("Error"))
            {
                msg("COM Port Error");
                vserial.setCommunicationError(true);
                comPortOpenedSuccess = false;

                getWristStrapStateBtn.Enabled = false;
                calibrationBtn.Enabled = false;
            }
            else
            {
                bool portSuccess = false;
                portSuccess = CSerial.IsPortAvailable(finalComPortNumber);
                int count = 0;
                bool resetResult = PortHelper.TryResetPortByInstanceId(instanceId);
                if(!resetResult)
                {
                    while (!portSuccess)
                    {
                        resetResult = PortHelper.TryResetPortByInstanceId(instanceId);
                        //   bool resetResult = PortHelper.TryResetPortByName(finalComPortNumber);
                        System.Threading.Thread.Sleep(5000);
                        portSuccess = CSerial.IsPortAvailable(finalComPortNumber);
                        count++;
                    }
                }
           
                int b = count;

                int err = vserial.OpenPort(finalComPortNumber);

                if (err > 0)
                {
                    vserial.setCommunicationError(true);
                    comPortOpenedSuccess = false;

                    getWristStrapStateBtn.Enabled = false;
                    calibrationBtn.Enabled = false;

                }
                else
                {
                    msg("COM Port Opened Success");
                    vserial.setCommunicationError(false);
                    comPortOpenedSuccess = true;
                    System.Threading.Thread.Sleep(500);


                    getWristStrapStateBtn.Enabled = true;
                    calibrationBtn.Enabled = true;

                    startStuffHere();
                }
            }








            if (comPortOpenedSuccess == false)
            {
                taskMessageLabel.Text = "Com Port Fail,Please call support";
                msg("COM Port Fail");

                enableMonitoring = true;
                vuState = sUserState.sunlock;
                vtask = sTask.logOff;
                wristStrapNotify.Visible = true;
                wristStrapNotify.BalloonTipText = "USB Communication Fail";
                wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                wristStrapNotify.ShowBalloonTip(ballonShowTime);
                ledOnCheckBox.AutoCheck = false;
                ledOffCheckBox.AutoCheck = false;
                wristStrapDisconnectCheckBox.AutoCheck = false;
                unCheckAllCheckBox();


                Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
                outLabel(ref taskMessageLabel, "USB Communication Fail", false);


                //  List<string> lstr = new List<string>();
                //   lstr.Add("0");
                //   lstr.Add("0");
                //   ExportImportFile.StringArrayToFile(lstr, "info.dat");
                List<string> lstr2 = new List<string>();
                lstr2 = ExportImportFile.FileToStringArray(mainFolder + "info.dat");
                int delayLogOffSec = 0;
                if (lstr2.Count > 0)
                {
                    delayLogOffSec = int.Parse(lstr2[0]);
                    delayLogOffMilliSec = delayLogOffSec * 1000;
                }
                else
                {
                    delayLogOffMilliSec = 10000;
                    delayLogOffSec = 10;
                }

                for (int a = 10; a < 100; a++)
                {
                    outComboBoxAddItem(ref timerComboBox, a.ToString());
                    // timerComboBox.Items.Add(a);
                }

                outComboBoxSelectItem(ref timerComboBox, delayLogOffSec - 10);
                LogOff();

                return;
            }
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
                        finalComPortNumber= "null";
                        finalComPortNumber = doGetUsb(ard);//,ref outDeviceName);
                        string instanceId = doGetUsbDeviceId(ard, deviceID);
                        // bool resetResult = PortHelper.TryResetPortByInstanceId(@"USB\VID_04D8&PID_000A\9&22F2C77B&0&4");

                        bool portSuccess = false;
                        while(!portSuccess)
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
                }
            }


            }
            catch (Exception g)
            {
                HandleCatchMessage(ref g);

            }
            msg("USB cable plug in detected");

        }
        private void DevicePluggedOutEventReceived(object sender, EventArrivedEventArgs e)
        {
            try
            {

          
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            if (vserial.communicationErrorBool == false)
            {
                string[] deviceArr = deviceID.Split('|');
                int deviceArrLen = deviceArr.Length;

                for (int a = 0; a < deviceArrLen; a++)
                {
                    if (instance.Properties["DeviceID"].Value.ToString().Contains(deviceArr[a]))
                    {

                       // getWristStrapStateBtn.Enabled = false;
                       // calibrationBtn.Enabled = false;


                        msg("USB cable unplug detected");
                        //OnDevicePluggedIn(EventArgs.Empty);
                        //  AppendToLogFile("USB_PLUG_OUT_DETECTED");
                        vserial.setCommunicationError(true);
                        vserial.ClosePort();
                        LogOff();
                    }
                }

                vserial.communicationErrorBool = true;
            }


            }
            catch (Exception g)
            {
                HandleCatchMessage(ref g);

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


        string appPath;
        string mainFolder;
        string logFolder;

        bool allowStartupCheckBox = false;

        private void HandleCatchMessage(ref Exception e)
        {
            msg("CatchMessage:"+e.Message);
        }

        public Form1()
        {
          
            try
            {
                InitializeComponent();












                getWristStrapStateBtn.Enabled = false;
                calibrationBtn.Enabled = false;


                allowStartupCheckBox = false;


                // Get Identity:
                WindowsIdentity user = WindowsIdentity.GetCurrent();

                // Set Principal
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                bool isAdmin =  principal.IsInRole(WindowsBuiltInRole.Administrator);

                if (isAdmin)
                {
                    //  RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    var obj = rk.GetValue("JabilWristStrapMonitor");
                    if (obj == null)
                    {
                        enableStartupCheckBox.Checked = false;
                    }
                    else
                    {
                        enableStartupCheckBox.Checked = true;
                    }
                }
                else
                {
                    enableStartupCheckBox.Enabled = false;
                }

           




                toolTipStr = "Initializing Wrist Strap Monitor";


                appPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                DirectoryInfo di = new DirectoryInfo(appPath);
                mainFolder = Path.Combine(di.FullName, @"");
                logFolder = Path.Combine(di.FullName, @"logFiles");

                timer1.Interval = 100;
                timer1.Start();


                 logDate = DateTime.Now;
                 DateTime tnow = DateTime.Now;
                 logFileNameByDay = logFolder+@"\wsmLog"+tnow.ToString("MMM-dd-yyyy")+@".txt";

                 bool exists = System.IO.File.Exists(logFileNameByDay);
                 if (!exists)
                 {
              
                 }

                comPortOpenedSuccess = false;

                currentUserName = GetCurrentLoginUserName();

                calibrationGroupBox.Visible = false;
                calDiff = new int[100];
                for(int a = 0;a<100;a++)
                {
                    calDiff[a] = 0;
                }

                Bitmap bm = new Bitmap(mainFolder+"jabil.png");
                pictureBox1.Image = bm;

                wristStrapNotify.BalloonTipClosed += wristStrapNotify_BalloonTipClosed;
                wristStrapNotify.BalloonTipShown += wristStrapNotify_BalloonTipShown;


                wristStrapNotify.BalloonTipText = "Starting";
                wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";


                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                vserial = new CSerial(false);
                vserial.serialReached += vserial_serialReached;
              //  int err = vserial.OpenPort("COM2");

                SetupPnPEventWatcher();
                pluggedInWatcher.Start();
                pluggedOutWatcher.Start();

         
                // SetupPnPEventWatcher();
                // pluggedInWatcher.Start();
                // pluggedOutWatcher.Start();

                InitializeComPort();

            }
            catch (Exception e)
            {
                HandleCatchMessage(ref e);
            }

          
        }


        void startStuffHere()
        {

            enableMonitoring = true;
            vuState = sUserState.sunknown;
            vtask = sTask.initState;
            wristStrapNotify.Visible = true;
            wristStrapNotify.BalloonTipText = "Running";
            wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
            wristStrapNotify.ShowBalloonTip(ballonShowTime);
            ledOnCheckBox.AutoCheck = false;
            ledOffCheckBox.AutoCheck = false;
            wristStrapDisconnectCheckBox.AutoCheck = false;
            unCheckAllCheckBox();

            
            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            outLabel(ref taskMessageLabel, "Task Idle", false);
           

            //  List<string> lstr = new List<string>();
            //   lstr.Add("0");
            //   lstr.Add("0");
            //   ExportImportFile.StringArrayToFile(lstr, "info.dat");
            List<string> lstr2 = new List<string>();
            lstr2 = ExportImportFile.FileToStringArray(mainFolder+"info.dat");
            int delayLogOffSec = 0;
            if (lstr2.Count > 0)
            {
                delayLogOffSec = int.Parse(lstr2[0]);
                delayLogOffMilliSec = delayLogOffSec * 1000;
            }
            else
            {
                delayLogOffMilliSec = 10000;
                delayLogOffSec = 10;
            }

            for (int a = 10; a < 100; a++)
            {
                outComboBoxAddItem(ref timerComboBox, a.ToString());
               // timerComboBox.Items.Add(a);
            }

            outComboBoxSelectItem(ref timerComboBox, delayLogOffSec - 10);
            

           // CheckWristStrapState();

          
            
        }

        void stopStuffHere()
        {
           

        }
        void wristStrapNotify_BalloonTipShown(object sender, EventArgs e)
        {
            ballonShowState = true;
        }
        void wristStrapNotify_BalloonTipClosed(object sender, EventArgs e)
        {
            ballonShowState = false;
        }


        private string GetCurrentLoginUserName()
        {
            return WindowsIdentity.GetCurrent().Name; ;
        }
    

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            currentUserName = GetCurrentLoginUserName();
          
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleDisconnect)
            {
                msg("Lock");
                
                if( vserial.communicationErrorBool == false)
                {
                    msg("Close COM Port");
                    vserial.ClosePort();
                    vserial.communicationErrorBool = true;
                }
               

               // outRtb(ref msgRtb, "Lock\n", true);
                vuState = sUserState.slock;
            }
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock || e.Reason == Microsoft.Win32.SessionSwitchReason.ConsoleConnect)
            {
                msg("Unlock");

                InitializeComPort();


                //outRtb(ref msgRtb, "Unlock\n", true);
                vuState = sUserState.sunlock;
                this.Focus();
            }
              if(e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogoff)
              {
                  msg("LogOff");
                  //outRtb(ref msgRtb, "LogOff\n", true);
              }
              if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLogon)
              {
                  msg("LogIn");
                  //outRtb(ref msgRtb, "LogIn\n", true);
              }
        }
        public void msg(string instr)
        {
            DateTime tnow = DateTime.Now;
            if (logDate.Day != tnow.Day)
            {
                logFileNameByDay = logFolder + @"\wsmLog" + tnow.ToString("MMM-dd-yyyy") + @".txt";
              //  logFileNameByDay = @"logFiles\wsmLog" + tnow.ToString("MMM-dd-yyyy");
                logDate = tnow;
            }

            
            string tstr = tnow.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            outRtb(ref msgRtb, tstr + "\t\t"+currentUserName+"\t\t" + " " + instr + "\n", true);

         


            if(enableMonitoring)
            {
                if (  (instr.Equals("WristOff"))  || (instr.Equals("COM Port Fail")) ) 
                {
                    Icon ired = new Icon(mainFolder+"red_light.ico");
                    wristStrapNotify.Icon = ired;
                    wristStrapNotify.BalloonTipText = "Wrist Strap Fail";
                    toolTipStr = "Wrist Strap Fail";
                    wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                    wristStrapNotify.ShowBalloonTip(5);
                }
                if (instr.Equals("WristOn"))
                {
                    Icon ired = new Icon(mainFolder + "green_light.ico");
                    wristStrapNotify.Icon = ired;
                    wristStrapNotify.BalloonTipText = "Wrist Strap Pass";
                    toolTipStr = "Wrist Strap Pass";
                    wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                    wristStrapNotify.ShowBalloonTip(ballonShowTime);
                }
            }
        }

        public void outButtonText(ref Button vrtb, string instr)
        {
            Button rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                        rtb.Text = instr;
                });
            }
            else
            {
                rtb.Text = instr;
            }
        }
        public void outRtb(ref RichTextBox vrtb, string instr, bool appendTrue)
        {

            RichTextBox rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                    if (appendTrue)
                    {
                        rtb.AppendText(instr);
                    }
                    else
                    {
                        rtb.Text = instr;
                    }

                    if (logFileNameByDay.Contains("logFiles") == true)
                    {
                        using (StreamWriter writer = new StreamWriter(logFileNameByDay, true))
                        {
                            writer.Write(instr);
                        }
                    }
                });
            }
            else
            {
                if (appendTrue)
                {
                    rtb.AppendText(instr);
                }
                else
                {
                    rtb.Text = instr;
                }
                if (logFileNameByDay.Contains("logFiles") == true)
                {
                    using (StreamWriter writer = new StreamWriter(logFileNameByDay, true))
                    {
                        writer.Write(instr);
                    }
                }
            }
        }


        public void outComboBoxSelectItem(ref ComboBox vrtb, int index)
        {
            ComboBox rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                    rtb.SelectedIndex = index ;
                    
                });
            }
            else
            {
                rtb.SelectedIndex = index;
            }
        }

        public void outComboBoxAddItem(ref ComboBox vrtb, string instr)
        {
            ComboBox rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                        rtb.Items.Add(instr);
                });
            }
            else
            {
                rtb.Items.Add(instr);
            }
        }

        public void outLabel(ref Label vrtb, string instr, bool appendTrue)
        {
            Label rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                    if (appendTrue)
                    {
                        rtb.Text = rtb.Text + instr;
                    }
                    else
                    {
                        rtb.Text = instr;
                    }
                });
            }
            else
            {
                if (appendTrue)
                {
                    rtb.Text = rtb.Text + instr;
                }
                else
                {
                    rtb.Text = instr;
                }
            }
        }
        public void outCheckBox(ref CheckBox vrtb,  bool checkTrue)
        {
            CheckBox rtb = vrtb;
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)delegate
                {
                    rtb.Checked = checkTrue;
                });
            }
            else
            {
                rtb.Checked = checkTrue;
            }
        }
        private void unCheckAllCheckBox()
        {
            outCheckBox(ref ledOffCheckBox, false);
            outCheckBox(ref ledOnCheckBox, false);
            outCheckBox(ref wristStrapDisconnectCheckBox, false);
        }
        void vserial_serialReached(object sender, serialEventArgs e)
        {
            string[] serialRx = e.serialData.serialRxStr;
            int size = e.serialData.size;
            for (int a = 0; a < size; a++)
            {
                if (serialRx[a].Count() > 0)
                {
                    msg(serialRx[a]);

                    //SerialRxTimeOut


                    if(serialRx[a].Equals("WristOff"))
                    {
                        Console.Beep();
                        unCheckAllCheckBox();
                        outCheckBox(ref ledOffCheckBox, true);
                        LogOff();
                     //  string[] b = { "a,b" };
                     //  mWindowsLogOff c = new mWindowsLogOff();
                     //  c.logOff(b);
                    }
                    if (serialRx[a].Equals("WristOn"))
                    {
                        Console.Beep();

                        unCheckAllCheckBox();
                        outCheckBox(ref ledOnCheckBox, true);
                        if(vtask == sTask.logOff  || vtask == sTask.initState)
                        {
                            vtask = sTask.idle;
                            outLabel(ref taskMessageLabel, "Task Idle", false);
                        }
                    }
                    if (serialRx[a].Equals("WristStrapDisconnect"))
                    {
                        LogOff();
                        unCheckAllCheckBox();
                        outCheckBox(ref wristStrapDisconnectCheckBox, true);                    
                    }
                    if (serialRx[a].Equals("WristStrapConnect"))
                    {
                        LogOff();
                        unCheckAllCheckBox();
                        outCheckBox(ref wristStrapDisconnectCheckBox, false);
                    }
                    if (serialRx[a].Equals("Calibration:On"))
                    {
                      calibrationModeStatus = true;
                      outButtonText(ref calibrationBtn, "Turn Off Calibration Mode");
                    }
                    if (serialRx[a].Equals("Calibration:Off"))
                    {
                      calibrationModeStatus = false;
                      outButtonText(ref calibrationBtn, "Turn On Calibration Mode");
                     
                    }
                    if (serialRx[a].Contains("Diff"))
                    {
                        if (calibrationModeStatus)
                        {
                            ProcessCalibration(serialRx[a]);
                        }
                    }
                   
                    if (serialRx[a].Contains("QueryLimitDone"))
                    {
                        string[] stemp = serialRx[a].Split(':');
                        outRtb(ref highLimitRtb, stemp[2], false);
                        outRtb(ref lowLimitRtb, stemp[4], false);
                    }

                }
            }
        }

        private void ProcessCalibration(string instr)
        {
            string[] stemp = instr.Split(':');
            string sdiff = stemp[1];
            int idiff = int.Parse(sdiff);

            double nom = 0;
            if (calSampleSizeCount < calSampleSizeNeeded)
            {
                calDiff[calSampleSizeCount] = idiff;
                calSampleSizeCount++;
            }
            if (calSampleSizeCount >= calSampleSizeNeeded)
            {
               
                double sumNom = 0;
                for (int a = 0; a < 100; a++)
                {
                    sumNom = sumNom + calDiff[a];
                    if (calMin > calDiff[a])
                    {
                        calMin = calDiff[a];
                    }
                    if (calMax < calDiff[a])
                    {
                        calMax = calDiff[a];
                    }
                }
                nom = sumNom / 100;
                calSampleSizeCount = 0;
                outLabel(ref maxLabel, calMax.ToString(), false);
                outLabel(ref minLabel, calMin.ToString(), false);
                outLabel(ref nominalLabel, nom.ToString(), false);

                for (int a = 0; a < 100; a++)
                {
                    calDiff[a] = 0;
                }
                calMax = 0;
                calMin = 5000;
                 

            }

            outLabel(ref sampleSizeLabel, calSampleSizeCount.ToString(), false);

          

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string[] b = { "a,b" };
            mWindowsLogOff a = new mWindowsLogOff();
            a.logOff(b);
        }
        private void LogOff()
        {
            if (enableMonitoring == true)
            {
                vtask = sTask.logOff;
                startLogOffTime = DateTime.Now;
            }
            if (enableMonitoring == false)
            {
                outLabel(ref taskMessageLabel, "Monitoring Disable, Task Idle", false);
            }
        }

        private void getWristStrapStateBtn_Click(object sender, EventArgs e)
        {
            byte[] g = { 57, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //  vserial.sendBytes(g,1);

            List<string> s = new List<string>();
            s.Add("WristOff");
            s.Add("WristOn");
            s.Add("WristStrapDisconnect");
            s.Add("WristStrapConnect");
            s.Add("Calibration:On");
            s.Add("Calibration:Off");
            vserial.serialQuery(g, g.Length, s, 1000);
        
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if(vuState == sUserState.sunlock)
                {
                    msg("Check wrist strap state");
                    CheckWristStrapState();
                    vuState = sUserState.sidle;
                }
                // handle task here
                if(vtask == sTask.serialCommunicationError)
                {
                    InitializeComPort();
                    vtask = sTask.initState;
                }
                if (vtask == sTask.initState)
                {
                    CheckWristStrapState();
                }
                if(vtask == sTask.logOff)
                {
                         Double elapsedMillisecs = ((TimeSpan)(DateTime.Now - startLogOffTime)).TotalMilliseconds;
                         if (elapsedMillisecs > delayLogOffMilliSec)
                         {
                             string[] b = { "a,b" };
                             mWindowsLogOff a = new mWindowsLogOff();
                             a.logOff(b);
                             vtask = sTask.idle;
                             outLabel(ref taskMessageLabel, "Task Idle", false);
                         }
                         else
                         {
                             double elapsedDouble = ((delayLogOffMilliSec - elapsedMillisecs) / 1000);
                             int elapsedSecond = Convert.ToInt16(Math.Ceiling(elapsedDouble));

                             outLabel(ref taskMessageLabel, "Process to lock, in " + elapsedSecond.ToString() + " second",false);
                         }
                }
                if (enableMonitoring == false)
                {
                    if (vtask == sTask.logOff)
                    {
                        vtask = sTask.idle;
                        outLabel(ref taskMessageLabel, "Monitoring Disable, Task Idle", false);
                    }
                }
            }
            catch (Exception ee)
            {
                HandleCatchMessage(ref ee);
                vtask = sTask.serialCommunicationError;
            }

        }

        private void CheckWristStrapState()
        {
            byte[] g = { 57, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //  vserial.sendBytes(g,1);

            List<string> s = new List<string>();
            s.Add("WristOff");
            s.Add("WristOn");
            s.Add("WristStrapDisconnect");
            s.Add("WristStrapConnect");
            s.Add("Calibration:On");
            s.Add("Calibration:Off");
            vserial.serialQuery(g, g.Length, s, 1000);
        }
        private void disableMonitoringBtn_Click(object sender, EventArgs e)
        {
            if (enableMonitoring == true)
            {
                AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();

                if (aform.successAuthenticate == true)
                {
                    enableMonitoring = false;
                    outLabel(ref taskMessageLabel, "Monitoring Disable, Task Idle", false);
                    msg("Monitoring Disable");

                    disableMonitoringBtn.Text = "Enable Monitoring";

                    Icon ired = new Icon(mainFolder + "yellow_light.ico");
                    wristStrapNotify.Icon = ired;
                    wristStrapNotify.BalloonTipText = "Wrist Strap Stop Monitoring";
                    toolTipStr = "Wrist Strap Stop Monitoring"; 
                    wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                    wristStrapNotify.ShowBalloonTip(ballonShowTime);
                }
                else
                {
                    MessageBox.Show("Wrong Password", "Error");
                }
                return;
            }
            if(enableMonitoring == false)
            {
                enableMonitoring = true;
                disableMonitoringBtn.Text = "Disable Monitoring";
                outLabel(ref taskMessageLabel, "Monitoring Enable, Task Idle", false);
                msg("Monitoring Enable");
                CheckWristStrapState();
                return;
            }
        }

        private void msgRtb_TextChanged(object sender, EventArgs e)
        {
            
            msgRtb.SelectionStart = msgRtb.Text.Length;
            // scroll it automatically
            msgRtb.ScrollToCaret();

            if (msgRtb.Text.Length > 2000)
            {
                int len = msgRtb.Text.Length ;
                string strim = msgRtb.Text.Substring(200, len - 200);
                msgRtb.Text = strim;
            }
             
        }
        private void wristStrapNotify_DoubleClick(object sender, EventArgs e)
        {
        }
        private void wristStrapNotify_MouseMove(object sender, MouseEventArgs e)
        {
          //  wristStrapNotify.Icon = ired;
            if (ballonShowState == false)
            {
                wristStrapNotify.BalloonTipText = toolTipStr;
                wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                wristStrapNotify.ShowBalloonTip(ballonShowTime);
            }
        
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

        private void timerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
                    int a = Convert.ToUInt16(timerComboBox.SelectedItem.ToString());
                    if(delayLogOffMilliSec != a*1000)
                    {
                        AuthenticateForm aform = new AuthenticateForm();
                        aform.ShowDialog();
                        if (aform.successAuthenticate == true)
                        {
                            delayLogOffMilliSec = a * 1000;
                            List<string> lstr = new List<string>();
                            lstr.Add(timerComboBox.SelectedItem.ToString());
                            ExportImportFile.StringArrayToFile(lstr, "info.dat");
                            msg("Timer setting changed = " + a.ToString() + " seconds");
                        }
                    }
         

        }

        private void calibrationBtn_Click(object sender, EventArgs e)
        {
            if (calibrationModeStatus == true)
            {
                calibrationGroupBox.Visible = false;
                byte[] g = { 70, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                List<string> s = new List<string>();
                s.Add("WristOff");
                s.Add("WristOn");
                s.Add("WristStrapDisconnect");
                s.Add("WristStrapConnect");
                s.Add("Calibration:On");
                s.Add("Calibration:Off");
                vserial.serialQuery(g, 10, s, 1000);


                enableMonitoring = true;
                disableMonitoringBtn.Text = "Disable Monitoring";
                outLabel(ref taskMessageLabel, "Monitoring Enable, Task Idle", false);
                msg("Monitoring Enable");
                CheckWristStrapState();
            


            }
            if (calibrationModeStatus == false)
            {
                AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();

                if (aform.successAuthenticate == true)
                {
                    calibrationGroupBox.Visible = true;
                    sampleSizeLabel.Text = "";
                    maxLabel.Text = "";
                    nominalLabel.Text = "";
                    minLabel.Text = "";
                    calSampleSizeNeeded = 100;
                    calSampleSizeCount = 0;
                    calMin = 5000;
                    calMax = 0;
                    calNom = 0;
                    for (int a = 0; a < 100; a++)
                    {
                        calDiff[a] = 0;
                    }
                    byte[] g = { 60, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    List<string> s = new List<string>();
                    s.Add("WristOff");
                    s.Add("WristOn");
                    s.Add("WristStrapDisconnect");
                    s.Add("WristStrapConnect");
                    s.Add("Calibration:On");
                    s.Add("Calibration:Off");
                    vserial.serialQuery(g, 10, s, 1000);




                    enableMonitoring = false;
                    outLabel(ref taskMessageLabel, "Monitoring Disable, Task Idle", false);
                    msg("Monitoring Disable");

                    disableMonitoringBtn.Text = "Enable Monitoring";

                    Icon ired = new Icon(mainFolder + "yellow_light.ico");
                    wristStrapNotify.Icon = ired;
                    wristStrapNotify.BalloonTipText = "Wrist Strap Stop Monitoring";
                    toolTipStr = "Wrist Strap Stop Monitoring";
                    wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                    wristStrapNotify.ShowBalloonTip(ballonShowTime);




                }
                else
                {
                    MessageBox.Show("Error, Wrong Password", "Error");
                }
              
            }

            
            //  vserial.sendBytes(g,1);

        
        }

        private bool IsInt(string sVal)
        {
            foreach (char c in sVal)
            {
                int iN = (int)c;
                if ((iN > 57) || (iN < 48))
                    return false;
            }
            return true;
        }
        private void setNewLimitBtn_Click(object sender, EventArgs e)
        {
            if( IsInt(highLimitRtb.Text) && IsInt(lowLimitRtb.Text))
            {
                Int16 hlimit = Int16.Parse(highLimitRtb.Text);
                byte[] byteArrayHLimit = BitConverter.GetBytes(hlimit);
                Int16 llimit = Int16.Parse(lowLimitRtb.Text);
                byte[] byteArrayLLimit = BitConverter.GetBytes(llimit);

                byte[] g = { 71, byteArrayHLimit[0], byteArrayHLimit[1], byteArrayLLimit[0], byteArrayLLimit[1], 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                List<string> s = new List<string>();
                s.Add("WristOff");
                s.Add("WristOn");
                s.Add("WristStrapDisconnect");
                s.Add("WristStrapConnect");
                s.Add("WriteLimitError");
                s.Add("WriteLimitDone");
                vserial.serialQuery(g, 10, s, 1000);
            }
            else
            {

            }
        }

        private void queryLimitBtn_Click(object sender, EventArgs e)
        {
            byte[] g = { 72, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            List<string> s = new List<string>();
            s.Add("WristOff");
            s.Add("WristOn");
            s.Add("WristStrapDisconnect");
            s.Add("WristStrapConnect");
            s.Add("Calibration:On");
            s.Add("QueryLimitDone");
            vserial.serialQuery(g, 10, s, 1000);
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
               AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();

                if (aform.successAuthenticate == true)
                {
                    if (calibrationModeStatus == true)
                    {
                        byte[] g = { 70, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                        List<string> s = new List<string>();
                        s.Add("WristOff");
                        s.Add("WristOn");
                        s.Add("WristStrapDisconnect");
                        s.Add("WristStrapConnect");
                        s.Add("Calibration:On");
                        s.Add("Calibration:Off");
                        vserial.serialQuery(g, 10, s, 1000);
                    }
                    msg("FormClosing");
                }
                else
                {
                    MessageBox.Show("You are not authorize to close this app", "Error");
                    e.Cancel = true;
                }






         
           

            System.Threading.Thread.Sleep(1000);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
                AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();
                if (aform.successAuthenticate == true)
                {
                    enableMonitoring = false;
                    outLabel(ref taskMessageLabel, "Monitoring Disable, Task Idle", false);
                    msg("Monitoring Disable");

                    disableMonitoringBtn.Text = "Enable Monitoring";

                    Icon ired = new Icon(mainFolder + "yellow_light.ico");
                    wristStrapNotify.Icon = ired;
                    wristStrapNotify.BalloonTipText = "Wrist Strap Stop Monitoring";
                    toolTipStr = "Wrist Strap Stop Monitoring";
                    wristStrapNotify.BalloonTipTitle = "WristStrap Monitoring";
                    wristStrapNotify.ShowBalloonTip(ballonShowTime);


                    LogFileViewerForm logform = new LogFileViewerForm(logFolder);
                    logform.ShowDialog();


                    enableMonitoring = true;
                    disableMonitoringBtn.Text = "Disable Monitoring";
                    outLabel(ref taskMessageLabel, "Monitoring Enable, Task Idle", false);
                    msg("Monitoring Enable");
                    CheckWristStrapState();





                }

           
        }

        private void enableStartupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

         

            if (allowStartupCheckBox)
            {
           
             //   RegistryKey rk = Registry.CurrentUser.OpenSubKey("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
      
                AuthenticateForm aform = new AuthenticateForm();
                aform.ShowDialog();

                if (aform.successAuthenticate == true)
                {
                    if (enableStartupCheckBox.Checked == true)
                    {
                        rk.SetValue("JabilWristStrapMonitor", Application.ExecutablePath.ToString());  // enable auto startup

                    }
                    else
                    {
                        rk.DeleteValue("JabilWristStrapMonitor", false);   // disable auto startup   
                    }
                }
                else
                {
                    var obj = rk.GetValue("JabilWristStrapMonitor");
                    if (obj == null)
                    {
                        enableStartupCheckBox.Checked = false;
                    }
                    else
                    {
                        enableStartupCheckBox.Checked = true;
                    }
                }
                allowStartupCheckBox = false;
            }

            }
            catch (Exception eee)
            {
                string f = eee.Message;
            }

        }

        private void enableStartupCheckBox_MouseEnter(object sender, EventArgs e)
        {
          
        }

        private void enableStartupCheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            allowStartupCheckBox = true;
        }

    }



}
