using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FF1_PRR.Common
{
    /// <summary>
    /// Provides comprehensive logging functionality for the randomizer with different log levels and diagnostic information
    /// </summary>
    public class RandomizerLogger
    {
        private readonly string logFilePath;
        private readonly LogLevel minimumLogLevel;
        private readonly object lockObject = new object();
        private readonly List<ILogHandler> logHandlers;

        public RandomizerLogger(string logDirectory = null, LogLevel minimumLogLevel = LogLevel.Info)
        {
            this.minimumLogLevel = minimumLogLevel;
            this.logHandlers = new List<ILogHandler>();

            // Set up default log file path
            if (string.IsNullOrEmpty(logDirectory))
            {
                logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            }

            Directory.CreateDirectory(logDirectory);
            logFilePath = Path.Combine(logDirectory, $"randomizer_{DateTime.Now:yyyyMMdd_HHmmss}.log");

            // Add default file handler
            AddLogHandler(new FileLogHandler(logFilePath));
        }

        /// <summary>
        /// Adds a custom log handler
        /// </summary>
        /// <param name="handler">The log handler to add</param>
        public void AddLogHandler(ILogHandler handler)
        {
            lock (lockObject)
            {
                logHandlers.Add(handler);
            }
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        public void Debug(string message, string context = null)
        {
            Log(LogLevel.Debug, message, context);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        public void Info(string message, string context = null)
        {
            Log(LogLevel.Info, message, context);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        public void Warning(string message, string context = null)
        {
            Log(LogLevel.Warning, message, context);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        /// <param name="exception">Optional exception details</param>
        public void Error(string message, string context = null, Exception exception = null)
        {
            var fullMessage = message;
            if (exception != null)
            {
                fullMessage += $"\nException: {exception.Message}\nStack Trace: {exception.StackTrace}";
            }
            Log(LogLevel.Error, fullMessage, context);
        }

        /// <summary>
        /// Logs a critical error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        /// <param name="exception">Optional exception details</param>
        public void Critical(string message, string context = null, Exception exception = null)
        {
            var fullMessage = message;
            if (exception != null)
            {
                fullMessage += $"\nException: {exception.Message}\nStack Trace: {exception.StackTrace}";
            }
            Log(LogLevel.Critical, fullMessage, context);
        }

        /// <summary>
        /// Logs validation results
        /// </summary>
        /// <param name="result">The validation result to log</param>
        /// <param name="context">Optional context information</param>
        public void LogValidation(ValidationResult result, string context = null)
        {
            if (result.IsValid)
            {
                Debug(result.SuccessMessage, context);
            }
            else
            {
                Error(result.ErrorMessage, context);
            }
        }

        /// <summary>
        /// Logs multiple validation results
        /// </summary>
        /// <param name="results">The validation results to log</param>
        /// <param name="context">Optional context information</param>
        public void LogValidations(IEnumerable<ValidationResult> results, string context = null)
        {
            foreach (var result in results)
            {
                LogValidation(result, context);
            }
        }

        /// <summary>
        /// Logs the start of an operation
        /// </summary>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="details">Optional operation details</param>
        /// <returns>A disposable operation tracker</returns>
        public OperationTracker StartOperation(string operationName, string details = null)
        {
            return new OperationTracker(this, operationName, details);
        }

        /// <summary>
        /// Logs diagnostic information about the system
        /// </summary>
        public void LogSystemDiagnostics()
        {
            Info("=== System Diagnostics ===");
            Info($"OS Version: {Environment.OSVersion}");
            Info($"CLR Version: {Environment.Version}");
            Info($"Working Directory: {Environment.CurrentDirectory}");
            Info($"Machine Name: {Environment.MachineName}");
            Info($"User Name: {Environment.UserName}");
            Info($"Processor Count: {Environment.ProcessorCount}");
            Info($"System Page Size: {Environment.SystemPageSize}");
            Info($"Available Memory: {GC.GetTotalMemory(false) / (1024 * 1024)} MB");
            Info("=== End System Diagnostics ===");
        }

        /// <summary>
        /// Logs randomizer configuration information
        /// </summary>
        /// <param name="config">The randomizer configuration</param>
        public void LogConfiguration(ConfigurationManager.RandomizerConfig config)
        {
            Info("=== Randomizer Configuration ===");
            Info($"Seed: {config.Seed}");
            Info($"FF1PR Folder: {config.FF1PRFolder}");
            Info($"Randomizer Flags: {config.RandoFlags}");
            Info("=== End Configuration ===");
        }

        /// <summary>
        /// Core logging method
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to log</param>
        /// <param name="context">Optional context information</param>
        private void Log(LogLevel level, string message, string context = null)
        {
            if (level < minimumLogLevel)
                return;

            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message,
                Context = context,
                ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId
            };

            lock (lockObject)
            {
                foreach (var handler in logHandlers)
                {
                    try
                    {
                        handler.WriteLog(logEntry);
                    }
                    catch (Exception ex)
                    {
                        // Avoid infinite recursion by not logging handler errors
                        Console.WriteLine($"Log handler error: {ex.Message}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents different log levels
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }

    /// <summary>
    /// Represents a log entry
    /// </summary>
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public string Context { get; set; }
        public int ThreadId { get; set; }

        public override string ToString()
        {
            var contextPart = !string.IsNullOrEmpty(Context) ? $" [{Context}]" : "";
            return $"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] [T{ThreadId}]{contextPart} {Message}";
        }
    }

    /// <summary>
    /// Interface for log handlers
    /// </summary>
    public interface ILogHandler
    {
        void WriteLog(LogEntry entry);
    }

    /// <summary>
    /// File-based log handler
    /// </summary>
    public class FileLogHandler : ILogHandler
    {
        private readonly string filePath;

        public FileLogHandler(string filePath)
        {
            this.filePath = filePath;
        }

        public void WriteLog(LogEntry entry)
        {
            File.AppendAllText(filePath, entry.ToString() + Environment.NewLine, Encoding.UTF8);
        }
    }

    /// <summary>
    /// Console-based log handler
    /// </summary>
    public class ConsoleLogHandler : ILogHandler
    {
        public void WriteLog(LogEntry entry)
        {
            var originalColor = Console.ForegroundColor;
            
            Console.ForegroundColor = entry.Level switch
            {
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Info => ConsoleColor.White,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Critical => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };

            Console.WriteLine(entry.ToString());
            Console.ForegroundColor = originalColor;
        }
    }

    /// <summary>
    /// Tracks the duration and outcome of operations
    /// </summary>
    public class OperationTracker : IDisposable
    {
        private readonly RandomizerLogger logger;
        private readonly string operationName;
        private readonly DateTime startTime;
        private bool disposed = false;

        public OperationTracker(RandomizerLogger logger, string operationName, string details = null)
        {
            this.logger = logger;
            this.operationName = operationName;
            this.startTime = DateTime.Now;

            var message = $"Starting operation: {operationName}";
            if (!string.IsNullOrEmpty(details))
            {
                message += $" - {details}";
            }
            logger.Info(message, "Operation");
        }

        public void Complete(string result = null)
        {
            if (!disposed)
            {
                var duration = DateTime.Now - startTime;
                var message = $"Completed operation: {operationName} in {duration.TotalMilliseconds:F2}ms";
                if (!string.IsNullOrEmpty(result))
                {
                    message += $" - {result}";
                }
                logger.Info(message, "Operation");
                disposed = true;
            }
        }

        public void Fail(string error, Exception exception = null)
        {
            if (!disposed)
            {
                var duration = DateTime.Now - startTime;
                var message = $"Failed operation: {operationName} after {duration.TotalMilliseconds:F2}ms - {error}";
                logger.Error(message, "Operation", exception);
                disposed = true;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Complete("Operation completed");
            }
        }
    }
}