using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;



// https://www.c-sharpcorner.com/UploadFile/b9e011/introduction-to-msmq/


namespace UIPartConsole
{
    class Program
    {
       
        static void Main(string[] args)
        {
            try
            {
                CUIConsole cuiconsole = new CUIConsole();
                int a = 0;
                while (true)
                {
                    //  cuiconsole.sendData2Queue();
                    //  cuiconsole.SendMsg("UItoSerial:"+a.ToString()+";");

                    string ck =  Console.ReadLine();
                    switch(ck.Trim())
                    {
                        case "A":                            
                            cuiconsole.SendMsmqMsg("RequestWristState;");
                            break;
                        case "B":
                            //stop
                            cuiconsole.StopMsmqReceive();
                                break;
                        case "C":
                            //start
                            cuiconsole.StartMsmqReceive();
                                break;

                    }



                   
                    a++;
                }
            }
            catch(Exception e)
            {
                string m = e.Message;
            }
           
        }

      
    }
}
