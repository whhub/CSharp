using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using ILog;

namespace FileLog
{
    [Export(typeof (ILogService))]
    public class FileLogService : ILogService
    {
        private readonly StreamWriter _file;

        public FileLogService()
        {
            var fileStream = new FileStream(@"C:\log.txt", FileMode.Append);
            _file = new StreamWriter(fileStream);
        }

        public void Log(string content)
        {
            _file.WriteLine(content);
            _file.Flush();
            Debug.WriteLine("FileLog: " + content);
        }
    }
}