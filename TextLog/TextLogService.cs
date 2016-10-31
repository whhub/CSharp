using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using ILog;

namespace TextLog
{
    [Export(typeof (ILogService))]
    public class TextLogService : ILogService
    {
        public void Log(string content)
        {
            Console.WriteLine("TextLog: " + content);
            Debug.WriteLine("TextLog: " + content);
        }
    }
}