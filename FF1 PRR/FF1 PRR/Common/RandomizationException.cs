using System;

namespace FF1_PRR.Common
{
    /// <summary>
    /// Exception thrown when randomization operations fail
    /// </summary>
    public class RandomizationException : Exception
    {
        public RandomizationException() : base()
        {
        }

        public RandomizationException(string message) : base(message)
        {
        }

        public RandomizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when validation operations fail
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when file operations fail
    /// </summary>
    public class FileOperationException : Exception
    {
        public string FilePath { get; }

        public FileOperationException(string filePath) : base($"File operation failed for: {filePath}")
        {
            FilePath = filePath;
        }

        public FileOperationException(string filePath, string message) : base($"File operation failed for {filePath}: {message}")
        {
            FilePath = filePath;
        }

        public FileOperationException(string filePath, string message, Exception innerException) : base($"File operation failed for {filePath}: {message}", innerException)
        {
            FilePath = filePath;
        }
    }
}