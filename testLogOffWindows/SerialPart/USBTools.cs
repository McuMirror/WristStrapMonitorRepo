using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Principal;
using System.IO;

using System.Management; // need to add System.Management to your project references.
using Microsoft.Win32;




namespace USBUtility
{
    public static  class USBTools
    {
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
        public static string doGetUsb(string arrDeviceName)
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
        public static string doGetUsb2(string arrDeviceName, string deviceId)
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
        public static string doGetUsbDeviceId(string arrDeviceName, string deviceId)
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
    }
}
