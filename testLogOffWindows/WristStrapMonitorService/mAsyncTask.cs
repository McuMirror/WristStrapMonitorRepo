

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Threading;




// aka function pointer 
//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/using-delegates


// task async
//https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming 

//Callback Operation By Delegate Or Interface
//http://www.c-sharpcorner.com/UploadFile/1c8574/delegate-used-for-callback-operation/

namespace AsyncTaskWithEvent
{
     class mAsyncTask
    {
        CancellationTokenSource _cts;
        Task[] _task;

        //http://www.c-sharpcorner.com/UploadFile/1c8574/delegate-used-for-callback-operation/
        public delegate void TaskCompletedCallBack(string taskResult);
        private TaskCompletedCallBack callback;

        public void AssignCallBackFunction(TaskCompletedCallBack taskCompletedCallBack)
        {
            callback = taskCompletedCallBack;
        }
        public mAsyncTask()
        {
            taskStopped = false;
        }
        public void StartTask()
        {
            taskStopped = false;
            _cts = new CancellationTokenSource();
            _task = new Task[1];
            _task[0] = DoWorkAsync(_cts.Token);
        }
        private bool taskStopped;
        public bool TaskIsCompleted()
        {
            return taskStopped;
           
        }
        public void StopTask()
        {
            try
            {
                _cts.Cancel();
                // show the dialog for at least 2 secs
                //   await Task.WhenAll(_task[0], Task.Delay(2000));
                // Task.WhenAll(_task[0], Task.Delay(2000));
            }
            catch (Exception ex)
            {
                while (ex is AggregateException)
                    ex = ex.InnerException;
                if (!(ex is OperationCanceledException))
                    throw;
            }
            //dialog.Close();
            //    };
        }

        async Task DoWorkAsync(CancellationToken ct)
        {
            var i = 0;
            while (true)
            {
                try
                {
                    if (ct.IsCancellationRequested)
                    {
                        taskStopped = true;
                        callback("mAsyncTask:TaskStopped;");
                        ct.ThrowIfCancellationRequested();
                    }

                    await Task.Run(() =>
                    {
                        callback("");
                    }, ct);
                }
                catch (Exception e)
                {
                    string m = e.Message;
                    callback("mAsyncTask:ExceptionHit:" + m);
                }
            }
        }


    }

}
