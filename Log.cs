using System;
using System.IO;

namespace TestTask
{
    public static class Logger
    {
        /*The Log method writes a message to a specified log file with a timestamp.
          Parameters: "logFilePath" full path to the log file where the message will be written; The "message" to be logged, which will be appended to the log file.*/
        public static void Log(string logFilePath, string message)
        {
            try
            {
                // Open the log file if the file does not exist, it will be created.
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    // Write the message to the file, prefixed with the timestamp.
                    sw.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, print an error message to the console.
                Console.WriteLine($"Error writing to log file: {ex}");
            }
        }
    }
}
