using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncTaskWithEvent;
using System.Messaging;
using LogFiles;


using Msmq;

//https://www.c-sharpcorner.com/UploadFile/b9e011/introduction-to-msmq/

namespace UIPartConsole
{
  
    class CUIConsole
    {
        mAsyncTask mtask;
      //  MessageQueue billingQ;
        CLog log;

        private CMsmqIPC cmsmq;
     

        List<CMsmqIPC> lcmsmq;
        static bool ConsoleWriteLineEnable = true;
        static bool LogWriteFileEnable = true;

        private void Msg(string inStr)
        {
            if (LogWriteFileEnable)
            {
                log.WriteLog = inStr;
            }
            if (ConsoleWriteLineEnable)
            {
                Console.WriteLine(inStr);
            }
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
        public void StopMsmqReceive()
        {
            DestroyCmsmqObject();
        }
        public void StartMsmqReceive()
        {
            CreateCmsmqObject();
        }
        
        private void ForCMsmqCallBack(string instr)
        {
            Msg("CMsmq:"+instr);
        }


        public CUIConsole()
        {
            log = new CLog();
            log.EnableLog = true;

            mtask = new mAsyncTask();
            mtask.AssignCallBackFunction(ForMtaskCallBack);
            mtask.StartTask();

            lcmsmq = new List<CMsmqIPC>();
            CreateCmsmqObject();
           
         
        }
        void CreateCmsmqObject()
        {
            try
            {
                CMsmqIPC cmsmq2 = new CMsmqIPC();
                cmsmq2.AssignCallBackFunction(ForCMsmqCallBack);
                cmsmq2.SetRole(eRole.IamUI);
                 lcmsmq.Add(cmsmq2);
                cmsmq = lcmsmq[0];
            }
            catch (Exception e)
            {
                string m = e.Message;
            }
        
            
        }
        void DestroyCmsmqObject()
        {
            try
            {
                if (lcmsmq.Count > 0)
                {
                    cmsmq.Free();
                    cmsmq.Dispose();
                    cmsmq = null;

                    lcmsmq.RemoveAt(0);
                }
            }
            catch(Exception e)
            {
                string m = e.Message;
            }
         
        }

        private void ForMtaskCallBack(string instr)
        {
            System.Threading.Thread.Sleep(10);
        }


    }
}
