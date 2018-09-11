using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.IO;


namespace ReceiverMulticast
{
    class Subscriber
    {
        static void Main(string[] args)
        {
            try
            {
                int messagesReceived = 0;
                var messages = new Queue<string>(5000);
                var filePath = typeof(Subscriber).FullName + ".txt";
                var path = @".\private$\hello-queue";

                using (var helloQueue = new MessageQueue(path))
                {
                    
                    if (!MessageQueue.Exists(helloQueue.Path))
                    {
                        MessageQueue.Create(helloQueue.Path);
                    }
                

                    helloQueue.MulticastAddress = "234.1.1.1:8001";
                    while (true)
                    {
                        var message = helloQueue.Receive();
                        if (message == null)
                            return;

                        var reader = new StreamReader(message.BodyStream);
                        var body = reader.ReadToEnd();

                        messagesReceived += 1;

                        messages.Enqueue(body);
                        Console.WriteLine(" [MSMQ] {0} Received {1}", messagesReceived, body);

                        if (string.CompareOrdinal("reset", body) == 0)
                        {
                            messagesReceived = 0;
                            File.WriteAllText(filePath, body);
                            messages.Clear();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                string m = e.Message;
            }
         
        }
    }
}
