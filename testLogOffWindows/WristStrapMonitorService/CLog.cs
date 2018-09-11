using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LogFiles
{
    class CLog
    {
        static string appPath;
        static string LogFolder;
        static string LogFileName;
        static DateTime currentLogDate;
        public bool EnableLog
        {
            get;
            set;
        }
        public string GetLogFileNamePath()
        {
            return LogFileName;
        }

        public CLog()
        {
            currentLogDate = DateTime.Now;
            EnableLog = true;
            appPath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            DirectoryInfo di = new DirectoryInfo(appPath);
            LogFolder = Path.Combine(di.FullName, @"LogFolder\");
            bool exists = System.IO.Directory.Exists(LogFolder);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(LogFolder);
            }
            LogFileName = GetNewFileNameDate(LogFolder);

            if (File.Exists(LogFileName))
            {

            }
        }

        private object threadSafer = new object();
        public string WriteLog
        {
            set
            {
                // LogWrite(WriteLog);
                lock (threadSafer)
                {
                    //  WriteToFile(value).Wait();
                    LogWriteAsync(value).Wait();
                }
            }
            get
            {
                return "";
            }

        }

        static Task WriteToFile(string instr)
        {
            return LogWriteAsync(instr);
        }



        static async Task LogWriteAsync(string inStr)
        {
            try
            {
                TimeSpan ts = DateTime.Now - currentLogDate;
                if (ts.Days > 0)
                {
                    LogFileName = GetNewFileNameDate(LogFolder);
                }
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                bool Append = true;
                using (StreamWriter sw = new StreamWriter(LogFileName, Append))
                {
                    // sw.WriteLine("{0}", timeStamp + " " + inStr);
                    await sw.WriteLineAsync(timeStamp + " " + inStr);
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception e)
            {
                string m = e.Message + " " + inStr;
            }
            finally
            {

            }


        }

        private void LogWrite(string inStr)
        {
            if (EnableLog)
            {
                TimeSpan ts = DateTime.Now - currentLogDate;
                if (ts.Days > 0)
                {
                    LogFileName = GetNewFileNameDate(LogFolder);
                }
                string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                bool Append = true;
                using (StreamWriter sw = new StreamWriter(LogFileName, Append))
                {
                    sw.WriteLine("{0}", timeStamp + " " + inStr);
                }
            }
        }
        public static string GetNewFileNameDate(string directory)
        {
            string filename = String.Format("{0:yyyy-MMM-dd}__{1}", DateTime.Now, "Log");
            string path = Path.Combine(directory, filename);
            return path;

        }
    }
}
