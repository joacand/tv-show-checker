using System;
using System.IO;
using TVShowChecker.Core.Interfaces;

namespace TVShowChecker.Infrastructure.Services
{
    public class Logger : ILogger
    {
        private string TodaysLogFile => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToShortDateString() + ".log");

        public void LogError(string msg)
        {
            var logFile = GetLogFile();

            using StreamWriter sw = File.AppendText(logFile);
            sw.WriteLine($"{DateTime.Now} - ERROR: {msg}");
        }

        private string GetLogFile()
        {
            var logFile = TodaysLogFile;

            if (!File.Exists(logFile))
            {
                using (File.Create(logFile)) { };
            }

            return logFile;
        }
    }
}
