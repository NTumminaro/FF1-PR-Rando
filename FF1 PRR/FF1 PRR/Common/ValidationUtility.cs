using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;

namespace FF1_PRR.Common
{
    /// <summary>
    /// Provides comprehensive validation utilities for file existence, JSON structure, and CSV format validation
    /// </summary>
    public static class ValidationUtility
    {
        /// <summary>
        /// Validates that a file exists at the specified path
        /// </summary>
        /// <param name="filePath">Path to the file to validate</param>
        /// <param name="description">Optional description of the file for error messages</param>
        /// <returns>ValidationResult indicating success or failure with details</returns>
        public static ValidationResult ValidateFileExists(string filePath, string description = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return ValidationResult.Failure($"File path is null or empty{(description != null ? $" for {description}" : "")}");
                }

                if (!File.Exists(filePath))
                {
                    return ValidationResult.Failure($"File not found: {filePath}{(description != null ? $" ({description})" : "")}");
                }

                return ValidationResult.Success($"File exists: {filePath}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating file existence for {filePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates that a directory exists at the specified path
        /// </summary>
        /// <param name="directoryPath">Path to the directory to validate</param>
        /// <param name="description">Optional description of the directory for error messages</param>
        /// <returns>ValidationResult indicating success or failure with details</returns>
        public static ValidationResult ValidateDirectoryExists(string directoryPath, string description = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(directoryPath))
                {
                    return ValidationResult.Failure($"Directory path is null or empty{(description != null ? $" for {description}" : "")}");
                }

                if (!Directory.Exists(directoryPath))
                {
                    return ValidationResult.Failure($"Directory not found: {directoryPath}{(description != null ? $" ({description})" : "")}");
                }

                return ValidationResult.Success($"Directory exists: {directoryPath}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating directory existence for {directoryPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates that multiple files exist
        /// </summary>
        /// <param name="filePaths">Collection of file paths to validate</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult with details about all file validations</returns>
        public static ValidationResult ValidateMultipleFilesExist(IEnumerable<string> filePaths, string description = null)
        {
            var results = new List<ValidationResult>();
            var failures = new List<string>();

            foreach (var filePath in filePaths)
            {
                var result = ValidateFileExists(filePath, description);
                results.Add(result);
                if (!result.IsValid)
                {
                    failures.Add(result.ErrorMessage);
                }
            }

            if (failures.Any())
            {
                return ValidationResult.Failure($"File validation failures{(description != null ? $" for {description}" : "")}:\n{string.Join("\n", failures)}");
            }

            return ValidationResult.Success($"All {results.Count} files validated successfully{(description != null ? $" for {description}" : "")}");
        }

        /// <summary>
        /// Validates JSON file structure and content
        /// </summary>
        /// <param name="jsonFilePath">Path to the JSON file to validate</param>
        /// <param name="expectedProperties">Optional list of required properties</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating success or failure with details</returns>
        public static ValidationResult ValidateJsonStructure(string jsonFilePath, IEnumerable<string> expectedProperties = null, string description = null)
        {
            try
            {
                // First validate file exists
                var fileExistsResult = ValidateFileExists(jsonFilePath, description);
                if (!fileExistsResult.IsValid)
                {
                    return fileExistsResult;
                }

                // Read and parse JSON
                string jsonContent = File.ReadAllText(jsonFilePath);
                
                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    return ValidationResult.Failure($"JSON file is empty: {jsonFilePath}{(description != null ? $" ({description})" : "")}");
                }

                // Validate JSON syntax
                JsonConvert.DeserializeObject(jsonContent);

                // If expected properties are specified, validate them
                if (expectedProperties != null && expectedProperties.Any())
                {
                    var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                    var missingProperties = expectedProperties.Where(prop => !jsonObject.ContainsKey(prop)).ToList();
                    
                    if (missingProperties.Any())
                    {
                        return ValidationResult.Failure($"JSON file missing required properties: {string.Join(", ", missingProperties)} in {jsonFilePath}{(description != null ? $" ({description})" : "")}");
                    }
                }

                return ValidationResult.Success($"JSON structure valid: {jsonFilePath}");
            }
            catch (JsonException ex)
            {
                return ValidationResult.Failure($"Invalid JSON format in {jsonFilePath}: {ex.Message}{(description != null ? $" ({description})" : "")}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating JSON structure for {jsonFilePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates CSV file format and structure
        /// </summary>
        /// <param name="csvFilePath">Path to the CSV file to validate</param>
        /// <param name="expectedHeaders">Optional list of required column headers</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating success or failure with details</returns>
        public static ValidationResult ValidateCsvFormat(string csvFilePath, IEnumerable<string> expectedHeaders = null, string description = null)
        {
            try
            {
                // First validate file exists
                var fileExistsResult = ValidateFileExists(csvFilePath, description);
                if (!fileExistsResult.IsValid)
                {
                    return fileExistsResult;
                }

                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                // Read header to validate CSV structure
                csv.Read();
                csv.ReadHeader();
                
                if (csv.HeaderRecord == null || csv.HeaderRecord.Length == 0)
                {
                    return ValidationResult.Failure($"CSV file has no headers: {csvFilePath}{(description != null ? $" ({description})" : "")}");
                }

                // Validate expected headers if provided
                if (expectedHeaders != null && expectedHeaders.Any())
                {
                    var actualHeaders = csv.HeaderRecord.ToList();
                    var missingHeaders = expectedHeaders.Where(header => !actualHeaders.Contains(header, StringComparer.OrdinalIgnoreCase)).ToList();
                    
                    if (missingHeaders.Any())
                    {
                        return ValidationResult.Failure($"CSV file missing required headers: {string.Join(", ", missingHeaders)} in {csvFilePath}{(description != null ? $" ({description})" : "")}");
                    }
                }

                // Try to read at least one record to validate format
                var recordCount = 0;
                while (csv.Read() && recordCount < 10) // Check first 10 records for performance
                {
                    recordCount++;
                    // Just reading to validate format - CsvReader will throw if format is invalid
                }

                return ValidationResult.Success($"CSV format valid: {csvFilePath} (Headers: {string.Join(", ", csv.HeaderRecord)})");
            }
            catch (CsvHelperException ex)
            {
                return ValidationResult.Failure($"Invalid CSV format in {csvFilePath}: {ex.Message}{(description != null ? $" ({description})" : "")}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating CSV format for {csvFilePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates that required CSV fields contain valid data
        /// </summary>
        /// <param name="csvFilePath">Path to the CSV file to validate</param>
        /// <param name="requiredFields">Dictionary of field names and their validation rules</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating success or failure with details</returns>
        public static ValidationResult ValidateCsvData(string csvFilePath, Dictionary<string, Func<string, bool>> requiredFields = null, string description = null)
        {
            try
            {
                // First validate CSV format
                var formatResult = ValidateCsvFormat(csvFilePath, requiredFields?.Keys, description);
                if (!formatResult.IsValid)
                {
                    return formatResult;
                }

                if (requiredFields == null || !requiredFields.Any())
                {
                    return ValidationResult.Success($"CSV data validation skipped (no validation rules provided): {csvFilePath}");
                }

                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                
                var records = csv.GetRecords<dynamic>().ToList();
                var validationErrors = new List<string>();
                
                for (int i = 0; i < records.Count; i++)
                {
                    var record = records[i] as IDictionary<string, object>;
                    
                    foreach (var field in requiredFields)
                    {
                        if (record.TryGetValue(field.Key, out var value))
                        {
                            var stringValue = value?.ToString() ?? "";
                            if (!field.Value(stringValue))
                            {
                                validationErrors.Add($"Row {i + 2}: Invalid value '{stringValue}' for field '{field.Key}'");
                            }
                        }
                    }
                }

                if (validationErrors.Any())
                {
                    return ValidationResult.Failure($"CSV data validation failures in {csvFilePath}:\n{string.Join("\n", validationErrors)}");
                }

                return ValidationResult.Success($"CSV data validation passed: {csvFilePath} ({records.Count} records validated)");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating CSV data for {csvFilePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates disk space availability for file operations
        /// </summary>
        /// <param name="directoryPath">Directory to check for available space</param>
        /// <param name="requiredSpaceBytes">Required space in bytes</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating if sufficient space is available</returns>
        public static ValidationResult ValidateDiskSpace(string directoryPath, long requiredSpaceBytes, string description = null)
        {
            try
            {
                var drive = new DriveInfo(Path.GetPathRoot(directoryPath));
                
                if (drive.AvailableFreeSpace < requiredSpaceBytes)
                {
                    var availableMB = drive.AvailableFreeSpace / (1024 * 1024);
                    var requiredMB = requiredSpaceBytes / (1024 * 1024);
                    return ValidationResult.Failure($"Insufficient disk space. Available: {availableMB}MB, Required: {requiredMB}MB{(description != null ? $" ({description})" : "")}");
                }

                return ValidationResult.Success($"Sufficient disk space available: {drive.AvailableFreeSpace / (1024 * 1024)}MB");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error checking disk space for {directoryPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates file permissions for read/write operations
        /// </summary>
        /// <param name="filePath">Path to the file to check permissions</param>
        /// <param name="requireWrite">Whether write permission is required</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating if permissions are sufficient</returns>
        public static ValidationResult ValidateFilePermissions(string filePath, bool requireWrite = false, string description = null)
        {
            try
            {
                // Check if file exists first
                if (!File.Exists(filePath))
                {
                    // If file doesn't exist, check directory permissions
                    var directory = Path.GetDirectoryName(filePath);
                    return ValidateDirectoryPermissions(directory, requireWrite, description);
                }

                // Test read permission
                try
                {
                    using var stream = File.OpenRead(filePath);
                }
                catch (UnauthorizedAccessException)
                {
                    return ValidationResult.Failure($"No read permission for file: {filePath}{(description != null ? $" ({description})" : "")}");
                }

                // Test write permission if required
                if (requireWrite)
                {
                    try
                    {
                        using var stream = File.OpenWrite(filePath);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return ValidationResult.Failure($"No write permission for file: {filePath}{(description != null ? $" ({description})" : "")}");
                    }
                }

                return ValidationResult.Success($"File permissions validated: {filePath}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating file permissions for {filePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates directory permissions for read/write operations
        /// </summary>
        /// <param name="directoryPath">Path to the directory to check permissions</param>
        /// <param name="requireWrite">Whether write permission is required</param>
        /// <param name="description">Optional description for error messages</param>
        /// <returns>ValidationResult indicating if permissions are sufficient</returns>
        public static ValidationResult ValidateDirectoryPermissions(string directoryPath, bool requireWrite = false, string description = null)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    return ValidationResult.Failure($"Directory does not exist: {directoryPath}{(description != null ? $" ({description})" : "")}");
                }

                // Test read permission
                try
                {
                    Directory.GetFiles(directoryPath);
                }
                catch (UnauthorizedAccessException)
                {
                    return ValidationResult.Failure($"No read permission for directory: {directoryPath}{(description != null ? $" ({description})" : "")}");
                }

                // Test write permission if required
                if (requireWrite)
                {
                    var testFile = Path.Combine(directoryPath, $"test_write_{Guid.NewGuid()}.tmp");
                    try
                    {
                        File.WriteAllText(testFile, "test");
                        File.Delete(testFile);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return ValidationResult.Failure($"No write permission for directory: {directoryPath}{(description != null ? $" ({description})" : "")}");
                    }
                    catch (Exception ex)
                    {
                        return ValidationResult.Failure($"Error testing write permission for directory {directoryPath}: {ex.Message}");
                    }
                }

                return ValidationResult.Success($"Directory permissions validated: {directoryPath}");
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error validating directory permissions for {directoryPath}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Represents the result of a validation operation
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string Message { get; private set; }
        public string ErrorMessage => IsValid ? null : Message;
        public string SuccessMessage => IsValid ? Message : null;

        private ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public static ValidationResult Success(string message = "Validation passed")
        {
            return new ValidationResult(true, message);
        }

        public static ValidationResult Failure(string errorMessage)
        {
            return new ValidationResult(false, errorMessage);
        }

        public override string ToString()
        {
            return $"{(IsValid ? "SUCCESS" : "FAILURE")}: {Message}";
        }
    }
}