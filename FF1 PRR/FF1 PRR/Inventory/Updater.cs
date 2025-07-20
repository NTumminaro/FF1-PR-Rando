using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FF1_PRR.Common;

namespace FF1_PRR.Inventory
{
	public static class Updater
	{


		public static void update(string mainDirectory)
		{
			if (!Path.Exists(Path.Combine(mainDirectory, "BepInEx")))
			{
				ZipFile.ExtractToDirectory("install.zip", mainDirectory, true);
			}
		}

		public static void MemoriaToMagiciteCopy(string mainDirectory, string origDirectory, string type, string topKey, bool merge = false)
		{
			// Validate input parameters
			if (string.IsNullOrWhiteSpace(mainDirectory))
			{
				throw new ArgumentException("Main directory cannot be null or empty", nameof(mainDirectory));
			}

			if (string.IsNullOrWhiteSpace(origDirectory))
			{
				throw new ArgumentException("Origin directory cannot be null or empty", nameof(origDirectory));
			}

			if (string.IsNullOrWhiteSpace(type))
			{
				throw new ArgumentException("Type cannot be null or empty", nameof(type));
			}

			// Validate directories exist
			var mainDirValidation = ValidationUtility.ValidateDirectoryExists(mainDirectory, "Main Directory");
			if (!mainDirValidation.IsValid)
			{
				throw new DirectoryNotFoundException($"Main directory not found: {mainDirectory}");
			}

			var origDirValidation = ValidationUtility.ValidateDirectoryExists(origDirectory, "Origin Directory");
			if (!origDirValidation.IsValid)
			{
				throw new DirectoryNotFoundException($"Origin directory not found: {origDirectory}");
			}

			string topDirectory;
			string topValue;

			string originalTopKey = topKey; // Keep original casing for internal paths
			
			switch (type)
			{
				case "Message":
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Data", "Message");
					break;
				case "MainData":
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Data", "Master");
					break;

				case "Map":
					if (string.IsNullOrWhiteSpace(topKey))
					{
						throw new ArgumentException("TopKey is required for Map type", nameof(topKey));
					}
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Res", "Map", originalTopKey); // Use original casing
					break;
				default:
					throw new ArgumentException($"Invalid type parameter: {type}. Valid types are: Message, MainData, Map");
			}

			topKey = topKey.ToLower(); // Convert to lowercase only for parent directory
			topDirectory = Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, topValue);
			Directory.CreateDirectory(topDirectory); // <-- We'll be creating an Export.json soon
			Directory.CreateDirectory(Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, "keys")); // <-- We'll be creating an Export.json soon
			ImportData importJson = new ImportData();
			if (merge && File.Exists(Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, "keys", "Export.json")))
			{
				using (StreamReader sr = new StreamReader(Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, "keys", "Export.json")))
				using (JsonTextReader reader = new JsonTextReader(sr))
				{
					JsonSerializer deserializer = new JsonSerializer();
					importJson = deserializer.Deserialize<ImportData>(reader);
				}
			}

			// Get files to export
			string[] filesToExport = Directory.GetFiles(origDirectory, "*.*", SearchOption.AllDirectories);

			foreach (string file in filesToExport)
			{
				// Strip first three directories from the file
				string finalFile = file.Substring(file.IndexOf('\\') + 1);
				finalFile = finalFile.Substring(finalFile.IndexOf('\\') + 1);
				if (file.Count(f => f == '\\') > 3)
					finalFile = finalFile.Substring(finalFile.IndexOf('\\') + 1);
				if (file.Contains("mods"))
					finalFile = finalFile.Substring(finalFile.IndexOf('\\') + 1);
				if (!Directory.Exists(Path.Combine(topDirectory, Path.GetDirectoryName(finalFile))))
					Directory.CreateDirectory(Path.Combine(topDirectory, Path.GetDirectoryName(finalFile)));

				File.Copy(file, MemoriaToMagiciteFile(mainDirectory, file), true);

				if (type == "Map" && (file.Count(f => f == '\\') == 3))
					finalFile = finalFile.Substring(finalFile.IndexOf('\\') + 1);
				string keyName = finalFile;
				importJson.keys.Add(keyName.Substring(0, keyName.IndexOf('.')).Replace('\\', '/'));
				importJson.values.Add(topValue.Replace('\\', '/') + "/" + finalFile.Substring(0, finalFile.IndexOf('.')).Replace('\\', '/'));
			}

			JsonSerializer serializer = new JsonSerializer();

			using (StreamWriter sw = new StreamWriter(Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, "keys", "Export.json")))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				serializer.Serialize(writer, importJson);
			}
		}

		public static string MemoriaToMagiciteFile(string mainDirectory, string type, string fileToUse, string topKey = null)
		{
			string topValue;

			switch (type)
			{
				case "Message":
					topKey = "message";
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Data", "Message");
					break;
				case "MainData":
					topKey = "master";
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Data", "Master");
					break;

				case "Map":
					if (topKey == null) throw new Exception("Map type has no topKey parameter value");
					topValue = Path.Combine("Assets", "GameAssets", "Serial", "Res", "Map", topKey);
					break;

				default:
					throw new Exception("Invalid type parameter in MemoriaToMagicite");
			}

			return Path.Combine(mainDirectory, "Magicite", "FF1PRR", topKey, topValue, fileToUse);
		}

		public static string MemoriaToMagiciteFile(string mainDirectory, string fileToUse)
		{
			// Validate input parameters
			if (string.IsNullOrWhiteSpace(mainDirectory))
			{
				throw new ArgumentException("Main directory cannot be null or empty", nameof(mainDirectory));
			}

			if (string.IsNullOrWhiteSpace(fileToUse))
			{
				throw new ArgumentException("File path cannot be null or empty", nameof(fileToUse));
			}

			// Validate main directory exists
			var directoryValidation = ValidationUtility.ValidateDirectoryExists(mainDirectory, "Main Directory");
			if (!directoryValidation.IsValid)
			{
				throw new DirectoryNotFoundException($"Main directory not found: {mainDirectory}");
			}

			string finalFile = fileToUse;
			
			// Remove leading backslashes
			while (finalFile.StartsWith(@"\"))
				finalFile = finalFile[1..];
			if (finalFile.StartsWith(@"altscenarios\", StringComparison.OrdinalIgnoreCase))
			{
				// Treat AltScenarios as consisting only of Map_XXXXX dirs
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				string topKey = finalFile.Substring(0, finalFile.IndexOf('\\'));
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				return MemoriaToMagiciteFile(mainDirectory, "Map", finalFile, topKey);
			}
			while (finalFile.StartsWith(@"res\", StringComparison.OrdinalIgnoreCase) || finalFile.StartsWith(@"data\", StringComparison.OrdinalIgnoreCase))
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
			if (finalFile.StartsWith(@"battle\", StringComparison.OrdinalIgnoreCase))
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
			if (finalFile.StartsWith(@"mods\", StringComparison.OrdinalIgnoreCase))
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
			if (finalFile.StartsWith(@"maps\", StringComparison.OrdinalIgnoreCase) || finalFile.StartsWith(@"map\", StringComparison.OrdinalIgnoreCase))
			{
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				string topKey = finalFile.Substring(0, finalFile.IndexOf('\\'));
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				return MemoriaToMagiciteFile(mainDirectory, "Map", finalFile, topKey);
			}

			else if (finalFile.StartsWith(@"message", StringComparison.OrdinalIgnoreCase))
			{
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				return MemoriaToMagiciteFile(mainDirectory, "Message", finalFile);
			}
			else if (finalFile.StartsWith(@"master", StringComparison.OrdinalIgnoreCase) || finalFile.StartsWith(@"maindata", StringComparison.OrdinalIgnoreCase))
			{
				finalFile = finalFile[(finalFile.IndexOf('\\') + 1)..];
				return MemoriaToMagiciteFile(mainDirectory, "MainData", finalFile);
			}

			throw new Exception("Invalid fileToUse parameter:  " + fileToUse);
		}

		public class ImportData
		{
			public List<string> keys { get; set; }
			public List<string> values { get; set; }

			public ImportData()
			{
				keys = new List<string>();
				values = new List<string>();
			}
		}
	}
}
