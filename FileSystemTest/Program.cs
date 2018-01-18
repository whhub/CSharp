using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSystemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowDiskSpace();
        }

        public static void ShowDiskSpace()
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                Console.WriteLine("[{0}]\t[{1}]", driveInfo.Name, driveInfo.TotalFreeSpace/(1024*1024*1024));
            }
        }

        //public static long GetHardDiskFreeSpace(string driverName)
        //{
        //    long freeSpace = new long();
        //    var sDriverPath = driverName + ":\\";

        //    var driver = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == sDriverPath);

        //    freeSpace = driver.TotalFreeSpace / (1024 * 1024 * 1024);
        //    return freeSpace;
        //}

    }
}
