using System.IO;

namespace ReadWriteSample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = "data.txt";
            var fileStream = File.Exists(path)
                ? File.OpenWrite(path)
                : File.Create(path);

            var stream = new StreamWriter(fileStream);
            stream.WriteLine("a b c");

            stream.Close();
        }
    }
}
