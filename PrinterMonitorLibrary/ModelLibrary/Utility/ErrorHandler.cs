using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PrinterMonitorLibrary
{
    public class ErrorHandler
    {
        public static void WriteError(Exception Error)
        {
            string directoryPath = System.Configuration.ConfigurationManager.AppSettings.Get("LogFilePath");

            string filename = string.Format("errorLog_{0}{1}{2}", System.DateTime.Now.Day.ToString(), System.DateTime.Now.Month.ToString(), System.DateTime.Now.Year.ToString());

            string filepath = directoryPath + "\\" + filename + ".txt";


            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filepath))
            {
                FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                StreamWriter file = new StreamWriter(fs);
                file.WriteLine("[Time Stamp: " + String.Format("{0:ddd, MMM d, yyyy}", DateTime.Now) + "]\n");
                file.WriteLine("[Error:]\n");
                file.WriteLine(Error + "\n\n");
                file.WriteLine("");
                file.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                StreamWriter file = new StreamWriter(fs);
                file.WriteLine("[Time Stamp: " + String.Format("{0:ddd, MMM d, yyyy}", DateTime.Now) + "]\n");
                file.WriteLine("[Error:]\n");
                file.WriteLine(Error + "\n\n");
                file.WriteLine("");
                file.Close();
                fs.Close();
            }
        }
    }
}
