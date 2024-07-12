using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace FF1_PRR.Inventory
{
  public static class SpriteUpdater
  {
    public static void ReplaceSprite(string baseFolder, string characterSelection, string spriteSelection, string ff1prFolder, bool includeJobUpgrade)
    {
      string characterFolder = GetCharacterFolder(characterSelection);
      string sourceFolderPath = Path.Combine(baseFolder, "data", "mods", "sprites", spriteSelection);
      string destFolderPath = Path.Combine(ff1prFolder, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", characterFolder.ToLower());

      // Copy character-specific files to the destination folder
      DirectoryCopy(sourceFolderPath, destFolderPath, characterFolder, true);

      // Ensure the export.json exists and update it with the correct paths
      EnsureAndUpdateExportJson(destFolderPath, characterFolder);

      // Update .atlas and .spritedata files
      UpdateAtlasFiles(destFolderPath, characterFolder);
      UpdateSpritedataFiles(destFolderPath, characterFolder);

      // Rename subfolders and files in /Map to the correct character folder name
      RenameSubfoldersAndFiles(destFolderPath, characterFolder);

      if (includeJobUpgrade)
      {
        string upgradedCharacterFolder = GetUpgradedCharacterFolder(characterSelection);
        string upgradedDestFolderPath = Path.Combine(ff1prFolder, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", upgradedCharacterFolder.ToLower());

        // Copy character-specific files to the destination folder for the upgraded character
        DirectoryCopy(sourceFolderPath, upgradedDestFolderPath, upgradedCharacterFolder, true);

        // Ensure the export.json exists and update it with the correct paths for the upgraded character
        EnsureAndUpdateExportJson(upgradedDestFolderPath, upgradedCharacterFolder);

        // Update .atlas and .spritedata files for the upgraded character
        UpdateAtlasFiles(upgradedDestFolderPath, upgradedCharacterFolder);
        UpdateSpritedataFiles(upgradedDestFolderPath, upgradedCharacterFolder);

        // Rename subfolders and files in /Map to the correct character folder name for the upgraded character
        RenameSubfoldersAndFiles(upgradedDestFolderPath, upgradedCharacterFolder);
      }
    }


    public static void ReplaceAirshipSprite(string baseFolder, string airshipSelection, string ff1prFolder)
    {
      string sourceFolderPath = Path.Combine(baseFolder, "data", "mods", "Airships", airshipSelection);
      string destFolderPath = Path.Combine(ff1prFolder, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "mo_ff1_v001_c00");

      // Copy airship-specific files to the destination folder
      DirectoryCopySimple(sourceFolderPath, destFolderPath);
    }

    public static void ReplaceBoatSprite(string baseFolder, string boatSelection, string ff1prFolder)
    {
      string sourceFolderPath = Path.Combine(baseFolder, "data", "mods", "Boats", boatSelection);
      string destFolderPath = Path.Combine(ff1prFolder, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "mo_ff1_v008_c00");

      // Copy airship-specific files to the destination folder
      DirectoryCopySimple(sourceFolderPath, destFolderPath);
    }

    private static string GetCharacterFolder(string characterSelection)
    {
      return characterSelection switch
      {
        "Warrior" => "mo_ff1_p001_c00",
        "Thief" => "mo_ff1_p002_c00",
        "Monk" => "mo_ff1_p003_c00",
        "Red Mage" => "mo_ff1_p004_c00",
        "White Mage" => "mo_ff1_p005_c00",
        "Black Mage" => "mo_ff1_p006_c00",
        _ => throw new ArgumentException("Invalid character selection"),
      };
    }

    private static string GetUpgradedCharacterFolder(string characterSelection)
    {
      return characterSelection switch
      {
        "Warrior" => "mo_ff1_p007_c00",
        "Thief" => "mo_ff1_p008_c00",
        "Monk" => "mo_ff1_p009_c00",
        "Red Mage" => "mo_ff1_p010_c00",
        "White Mage" => "mo_ff1_p011_c00",
        "Black Mage" => "mo_ff1_p012_c00",
        _ => throw new ArgumentException("Invalid character selection"),
      };
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, string characterFolder, bool copySubDirs)
    {
      DirectoryInfo dir = new DirectoryInfo(sourceDirName);

      if (!dir.Exists)
      {
        throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
      }

      DirectoryInfo[] dirs = dir.GetDirectories();
      Directory.CreateDirectory(destDirName);

      foreach (FileInfo file in dir.GetFiles())
      {
        string tempPath = Path.Combine(destDirName, file.Name);
        if (file.Extension == ".atlas" || file.Extension == ".spritedata" || file.Extension == ".json")
        {
          string content = File.ReadAllText(file.FullName);
          content = ReplacePlaceholder(content, characterFolder);
          File.WriteAllText(tempPath, content);

          if (file.Extension == ".atlas")
          {
            string newAtlasFileName = ReplacePlaceholder(Path.GetFileName(file.FullName), characterFolder);
            string newAtlasFilePath = Path.Combine(Path.GetDirectoryName(tempPath), newAtlasFileName);
            if (File.Exists(newAtlasFilePath))
            {
              File.Delete(newAtlasFilePath);
            }
            File.Move(tempPath, newAtlasFilePath);
          }
        }
        else
        {
          file.CopyTo(tempPath, true);
        }
      }

      if (copySubDirs)
      {
        foreach (DirectoryInfo subdir in dirs)
        {
          string tempPath = Path.Combine(destDirName, ReplacePlaceholder(subdir.Name, characterFolder));
          DirectoryCopy(subdir.FullName, tempPath, characterFolder, copySubDirs);
        }
      }
    }

    private static void DirectoryCopySimple(string sourceDirName, string destDirName)
    {
      DirectoryInfo dir = new DirectoryInfo(sourceDirName);

      if (!dir.Exists)
      {
        throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
      }

      DirectoryInfo[] dirs = dir.GetDirectories();
      Directory.CreateDirectory(destDirName);

      foreach (FileInfo file in dir.GetFiles())
      {
        string tempPath = Path.Combine(destDirName, file.Name);
        file.CopyTo(tempPath, true);
      }

      foreach (DirectoryInfo subdir in dirs)
      {
        string tempPath = Path.Combine(destDirName, subdir.Name);
        DirectoryCopySimple(subdir.FullName, tempPath);
      }
    }

    private static void EnsureAndUpdateExportJson(string destFolderPath, string characterFolder)
    {
      string exportJsonPath = Path.Combine(destFolderPath, "keys", "export.json");
      if (!File.Exists(exportJsonPath))
      {
        throw new FileNotFoundException("export.json not found in template: " + exportJsonPath);
      }

      string jsonContent = File.ReadAllText(exportJsonPath);
      dynamic jsonObj = JsonConvert.DeserializeObject(jsonContent);

      UpdatePathsInJson(jsonObj, characterFolder);

      string updatedOutput = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
      File.WriteAllText(exportJsonPath, updatedOutput);
    }

    private static void UpdatePathsInJson(dynamic jsonObj, string characterFolder)
    {
      var keys = jsonObj.keys;
      var values = jsonObj.values;

      for (int i = 0; i < values.Count; i++)
      {
        string value = values[i].ToString();
        values[i] = ReplacePlaceholder(value, characterFolder);
      }

      jsonObj.values = values;
    }

    private static void UpdateAtlasFiles(string destPath, string characterFolder)
    {
      string[] atlasFiles = Directory.GetFiles(destPath, "*.atlas");

      foreach (string atlasFile in atlasFiles)
      {
        string content = File.ReadAllText(atlasFile);
        content = ReplacePlaceholder(content, characterFolder);
        string newAtlasFileName = ReplacePlaceholder(Path.GetFileName(atlasFile), characterFolder);
        string newAtlasFilePath = Path.Combine(Path.GetDirectoryName(atlasFile), newAtlasFileName);
        if (File.Exists(newAtlasFilePath))
        {
          File.Delete(newAtlasFilePath);
        }
        File.WriteAllText(atlasFile, content);
        File.Move(atlasFile, newAtlasFilePath);
      }
    }

    private static void UpdateSpritedataFiles(string destPath, string characterFolder)
    {
      string[] spritedataFiles = Directory.GetFiles(destPath, "*.spritedata");

      foreach (string spritedataFile in spritedataFiles)
      {
        string content = File.ReadAllText(spritedataFile);
        content = ReplaceFirstPlaceholder(content, characterFolder);
        content = content.Replace("Assets\\GameAssets\\Serial\\Res\\Chara\\Map\\MO_", $"Assets\\GameAssets\\Serial\\Res\\Chara\\Map\\{characterFolder.ToUpper()}_");
        File.WriteAllText(spritedataFile, content);
      }
    }

    private static string ReplacePlaceholder(string content, string characterFolder)
    {
      string upperCharacterFolder = characterFolder.ToUpper();
      string pattern = @"MO_[^_]+_[^_]+_C00";
      return Regex.Replace(content, pattern, upperCharacterFolder, RegexOptions.IgnoreCase);
    }

    private static string ReplaceFirstPlaceholder(string content, string characterFolder)
    {
      string upperCharacterFolder = characterFolder.ToUpper();
      string pattern = @"MO_[^_]+_[^_]+_C00";
      Match match = Regex.Match(content, pattern, RegexOptions.IgnoreCase);
      if (match.Success)
      {
        content = content.Substring(0, match.Index) + upperCharacterFolder + content.Substring(match.Index + match.Length);
      }
      return content;
    }

    private static void RenameSubfoldersAndFiles(string destFolderPath, string characterFolder)
    {
      string mapPath = Path.Combine(destFolderPath, "Assets", "GameAssets", "Serial", "Res", "Chara", "Map");
      DirectoryInfo mapDir = new DirectoryInfo(mapPath);

      foreach (var subDir in mapDir.GetDirectories())
      {
        if (subDir.Name.StartsWith("MO_", StringComparison.OrdinalIgnoreCase))
        {
          string newSubDirName = ReplacePlaceholder(subDir.Name, characterFolder);
          string newSubDirPath = Path.Combine(mapDir.FullName, newSubDirName);
          if (!string.Equals(subDir.FullName, newSubDirPath, StringComparison.OrdinalIgnoreCase))
          {
            Directory.Move(subDir.FullName, newSubDirPath);
          }

          // Rename .png files in the subdirectory
          foreach (var file in subDir.GetFiles("*.png"))
          {
            string newFileName = ReplacePlaceholder(file.Name, characterFolder);
            string newFilePath = Path.Combine(subDir.FullName, newFileName);
            if (File.Exists(newFilePath))
            {
              File.Delete(newFilePath);
            }

            // Skip move if source and destination are the same
            if (!string.Equals(file.FullName, newFilePath, StringComparison.OrdinalIgnoreCase))
            {
              // Retry logic to handle intermittent issues
              int retries = 3;
              bool success = false;
              while (retries > 0 && !success)
              {
                try
                {
                  file.MoveTo(newFilePath);
                  success = true;
                }
                catch (IOException)
                {
                  retries--;
                  Thread.Sleep(100); // Small delay before retrying
                }
              }

              if (!success)
              {
                throw new IOException($"Failed to move file: {file.FullName} to {newFilePath} after multiple attempts.");
              }
            }
          }
        }
      }
    }
  }
}
