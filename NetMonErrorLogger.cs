using System;
using System.IO;

namespace NetworkMonitor
{
    class NetMonErrorLogger
    {
        public static void ErrorLogger(Exception ex, string path, string filename)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText($@"{path}\{filename}.log", $"Message: {ex.Message}\n\nStack: {ex.StackTrace}\n");

            Console.WriteLine($"\n\n\tError log can be found at '{path}'.");

            System.Threading.Thread.Sleep(5000);
        }
    }
}
