using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;
using System.Diagnostics;

namespace PublisherMulticast
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. establish the queue
            using (var helloQueue = new MessageQueue("FormatName:MULTICAST=234.1.1.1:8001"))
            {
                while (true)
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();

                    //2. create a message, that can be anything since it is a byte array
                    for (var i = 0; i < 100; i++)
                    {
                        SendMessage(helloQueue,
                            string.Format("{0}: msg:{1} hello world ", DateTime.UtcNow.Ticks, i));
                        Thread.Sleep(10);
                    }

                    stopWatch.Stop();
                    Console.ReadLine();

                    Console.WriteLine("====================================================");
                    Console.WriteLine("[MSMQ] done sending 1000 messages in " + stopWatch.ElapsedMilliseconds);
                    Console.WriteLine("[MSMQ] Sending reset counter to consumers.");

                    SendMessage(helloQueue, "reset");
                    Console.ReadLine();
                }
            }
        }
        private static void SendMessage(MessageQueue queue, string content)
        {
            //3. send the message
            queue.Send(content);
            Console.WriteLine(" [MSMQ] Sent {0}", content);
        }
    }
}
