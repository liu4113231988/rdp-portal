using System;
using System.IO;
using System.Text;

namespace RDP_Portal
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string LogFileName = "rdp-portal.log";
        private static string LogPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName);
        private static readonly int MaxLogSizeBytes = 1024 * 1024;

        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }

        public static void Warning(string message)
        {
            WriteLog("WARN", message);
        }

        public static void Error(string message, Exception? ex = null)
        {
            var sb = new StringBuilder();
            sb.Append(message);
            if (ex != null)
            {
                sb.AppendLine();
                sb.Append($"Exception: {ex.GetType().Name}");
                sb.AppendLine();
                sb.Append($"Message: {ex.Message}");
                sb.AppendLine();
                sb.Append($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    sb.AppendLine();
                    sb.Append($"InnerException: {ex.InnerException.GetType().Name}");
                    sb.AppendLine();
                    sb.Append($"InnerMessage: {ex.InnerException.Message}");
                    sb.AppendLine();
                    sb.Append($"InnerStackTrace: {ex.InnerException.StackTrace}");
                }
            }
            WriteLog("ERROR", sb.ToString());
        }

        public static void Debug(string message)
        {
#if DEBUG
            WriteLog("DEBUG", message);
#endif
        }

        private static void WriteLog(string level, string message)
        {
            try
            {
                lock (_lock)
                {
                    CheckLogSize();
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var logLine = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";
                    File.AppendAllText(LogPath, logLine, Encoding.UTF8);
                }
            }
            catch
            {
            }
        }

        private static void CheckLogSize()
        {
            try
            {
                if (File.Exists(LogPath))
                {
                    var fileInfo = new FileInfo(LogPath);
                    if (fileInfo.Length > MaxLogSizeBytes)
                    {
                        var backupPath = LogPath + ".bak";
                        if (File.Exists(backupPath))
                        {
                            File.Delete(backupPath);
                        }
                        File.Move(LogPath, backupPath);
                    }
                }
            }
            catch
            {
            }
        }

        public static void Clear()
        {
            try
            {
                lock (_lock)
                {
                    if (File.Exists(LogPath))
                    {
                        File.Delete(LogPath);
                    }
                }
            }
            catch
            {
            }
        }

        public static string GetLogPath()
        {
            return LogPath;
        }
    }
}
