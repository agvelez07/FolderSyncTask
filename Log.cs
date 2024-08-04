using TestTask;

namespace TestTask {

    public static class Logger
    {
        private static string logFilePath;
        public static void logInit(string path)
        {
            logFilePath = path;
            
            Console.WriteLine(logFilePath);
        }
    }
}