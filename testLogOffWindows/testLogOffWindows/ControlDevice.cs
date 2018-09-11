using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




using System;
using System.Runtime.InteropServices;
using System.Text;

//  https://docs.microsoft.com/en-us/windows-hardware/drivers/install/porting-from-setupapi-to-cfgmgr32#get-a-list-of-present-devices-and-retrieve-a-property-for-each-device
//  https://www.codeproject.com/Articles/6374/NET-Diving-into-System-Programming-Part




namespace testLogOffWindows
{
 /// &lt;summary>
 /// Summary description for Class.
 /// &lt;/summary>
 class DeviceClasses
 {
  /// &lt;summary>
  /// The main entry point for the application.
  /// &lt;/summary>
  public const int MAX_NAME_PORTS=7;
  public const int RegDisposition_OpenExisting=(0x00000001); 
    // open key only if exists
  public const int CM_REGISTRY_HARDWARE=(0x00000000);

  public const int CR_SUCCESS = (0x00000000);
  public const int CR_NO_SUCH_VALUE = (0x00000025);
  public const int CR_INVALID_DATA = (0x0000001F);
  public const int DIGCF_PRESENT = (0x00000002);
  public const int DIOCR_INSTALLER = (0x00000001);
// MaximumAllowed access type to Reg.
  public const int MAXIMUM_ALLOWED = (0x02000000);
[StructLayout(LayoutKind.Sequential)]

 public class SP_DEVINFO_DATA
 {
 public int cbSize;
 public Guid ClassGuid;
 public int DevInst; // DEVINST handle
 public ulong Reserved;
 };

    [DllImport("cfgmgr32.dll")]
    public static extern UInt32
    CM_Reenumerate_DevNode(UInt32 dnDevInst, UInt32 ulFlags);



  [DllImport("cfgmgr32.dll")]
  public static extern UInt32
  CM_Open_DevNode_Key(IntPtr dnDevNode, UInt32 samDesired, 
         UInt32 ulHardwareProfile,
         UInt32 Disposition,IntPtr phkDevice, UInt32 ulFlags);

  [DllImport("cfgmgr32.dll")]
  public static extern UInt32
  CM_Enumerate_Classes(UInt32 ClassIndex,ref Guid ClassGuid, UInt32 Params);

  [DllImport("setupapi.dll")]//
  public static extern Boolean
   SetupDiClassNameFromGuidA(ref Guid ClassGuid,
            StringBuilder ClassName, //char * ?
   UInt32 ClassNameSize, ref UInt32 RequiredSize);

  [DllImport("setupapi.dll")]
  public static extern IntPtr
   SetupDiGetClassDevsA(ref Guid ClassGuid, UInt32 Enumerator,
   IntPtr  hwndParent, UInt32 Flags);

  [DllImport("setupapi.dll")]
  public static extern Boolean
   SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex,
   ref SP_DEVINFO_DATA  DeviceInfoData);

  [DllImport("setupapi.dll")]
  public static extern Boolean
   SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

  [DllImport("setupapi.dll")]
  public static extern IntPtr
   SetupDiGetClassDevsA(ref Guid ClassGuid, UInt32 samDesired,
   UInt32 Flags, ref string hwndParent, IntPtr Reserved);

  [DllImport("setupapi.dll")]
  public static extern IntPtr
  SetupDiOpenClassRegKeyExA(
  ref Guid ClassGuid, UInt32 samDesired, int Flags, IntPtr MachineName,
  UInt32 Reserved);

  [DllImport("advapi32.dll")]
  public static extern UInt32
  RegQueryValueA(IntPtr KeyClass,UInt32 SubKey,
         StringBuilder ClassDescription,ref UInt32 sizeB);


  [DllImport("user32.dll")]
  public static extern Boolean
  CharToOem(String lpszSrc, StringBuilder lpszDst);

  public static int EnumerateClasses(UInt32 ClassIndex, 
   ref StringBuilder ClassName, StringBuilder ClassDescription, 
        ref bool DevicePresent)
  {
   Guid ClassGuid=Guid.Empty;
   IntPtr NewDeviceInfoSet;
   SP_DEVINFO_DATA DeviceInfoData;
   UInt32 result;
   StringBuilder name=new StringBuilder("");
   bool resNam=false;
   UInt32 RequiredSize=0;

   IntPtr ptr;

   result = CM_Enumerate_Classes(ClassIndex, ref ClassGuid,0);


    ClassName=new StringBuilder("");
    DevicePresent=false;
   //incorrect device class:
   if(result == CR_INVALID_DATA)
   {
    return -2;
   }
  //device class is absent
   if(result == CR_NO_SUCH_VALUE)
   {
    return -1;
   }
  //bad param. - fatal error
   if(result != CR_SUCCESS)
   {
    return -3;
   }


   name.Capacity=0;
   resNam=SetupDiClassNameFromGuidA(ref ClassGuid,name,RequiredSize,
         ref RequiredSize);
   if(RequiredSize > 0)
    {
    name.Capacity=(int)RequiredSize;
    resNam=SetupDiClassNameFromGuidA(ref ClassGuid,name,
           RequiredSize,ref RequiredSize);
    }

   NewDeviceInfoSet=SetupDiGetClassDevsA(
    ref ClassGuid,
    0,
    IntPtr.Zero,
    DIGCF_PRESENT);

   if(NewDeviceInfoSet.ToInt32() == -1)
    {  DevicePresent=false;
      ClassName=name;
      return 0;}

   IntPtr KeyClass=SetupDiOpenClassRegKeyExA(
    ref ClassGuid, MAXIMUM_ALLOWED, DIOCR_INSTALLER,IntPtr.Zero,0);
   if(KeyClass.ToInt32() == -1)
    {  DevicePresent=false;
      ClassName=name;
      return 0;}


   UInt32 sizeB=1000;
   String abcd="";
   StringBuilder CD=new StringBuilder("");
   ClassDescription.Capacity=1000;
  
   UInt32 res=RegQueryValueA(KeyClass,0,ClassDescription,ref sizeB);


   if(res != 0)ClassDescription=new StringBuilder("");
   SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
    ClassName=name;
    DevicePresent=true;

   return 0;

  }


     /*
  [STAThread]
  static void Main(string[] args)
  {
   StringBuilder classes=new StringBuilder("");
   StringBuilder classesDescr=new StringBuilder("");

   StringBuilder classesDescrOEM=new StringBuilder("");
   classesDescrOEM.Capacity=1000;
   Boolean DevExist=false;
   UInt32 i=0;
   while(true)
   {
   int res=EnumerateClasses(i,ref classes,classesDescr,ref DevExist);
   if(res == -1)break;
   ++i;
   if(res &lt; -1 || !DevExist)continue;
   Console.WriteLine("ClassName={0}, Description={1}",classes,classesDescr);
   }
   return;
  }
      * */
 }
    
}
