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
        }
    }
}
