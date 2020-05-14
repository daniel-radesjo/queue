using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace q
{
    class Program
    {
        private static string strQueueFile = "queue.txt";                       //queue for every processes, one row for one process
        private static string strActiveFile = "%computername%.active.txt";      //running process for every computer
        private static string strFinishedFile = "%computername%.finished.txt";  //finished processes for every computer
        private static string strOutputFile = "%computername%.output.txt";      //output from processes for every computer

        private static string strCmd = "";                                      //get process from queue to be executed
        private static string strComputerName = System.Environment.MachineName; //computer name

        static void Main(string[] args)
        {
            //if <strQueueFile> doesn't exist --> create it
            if (!File.Exists(strQueueFile))
            {
                Debug("queue.txt doesn't exists, creating...");
                FileStream f = File.Create(strQueueFile);
                f.Close();
                f.Dispose();
            }

            //add computername to all text files
            strActiveFile = strActiveFile.Replace("%computername%", strComputerName);
            strFinishedFile = strFinishedFile.Replace("%computername%", strComputerName);
            strOutputFile = strOutputFile.Replace("%computername%", strComputerName);

            Debug("Rows in " + strQueueFile + ": " + File.ReadAllLines(strQueueFile).Count());
            Debug("Computer name: " + strComputerName);

            while (File.ReadAllLines(strQueueFile).Count() > 0)
            {

                StreamReader sr = new StreamReader(strQueueFile);
                strCmd = sr.ReadLine();
                sr.Close();
                sr.Dispose();

                File.WriteAllLines(strQueueFile, File.ReadAllLines(strQueueFile).Skip(1)); //Remove first/top item from queue

                if (strCmd.Trim().Length > 0)
                {
                    Debug("Starting " + strCmd);
                    File.WriteAllText(strActiveFile, strCmd); //Write active item

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + strCmd + " >> " + strOutputFile);
                    process.Start();
                    process.WaitForExit();
                    process.Close();
                    process.Dispose();
                    Debug("Finished " + strCmd);
                    File.AppendAllText(strFinishedFile, strCmd + "\r\n"); //Add item to finished
                    File.Delete(strActiveFile); //Remove active
                }
                System.Threading.Thread.Sleep(1000);
            }

        }

        private static void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
