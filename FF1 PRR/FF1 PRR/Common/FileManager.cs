using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using FF1_PRR.Inventory;

namespace FF1_PRR.Common
{
    public class FileManager
    {
        private readonly string gameDirectory;

        public FileManager(string gameDirectory)
        {
            this.gameDirectory = gameDirectory;
        }

        public void RestoreVanillaFiles()
        {
            string[] dataMaster = {
                "ability.csv", "product.csv", "weapon.csv", "monster.csv", "monster_party.csv",
                "item.csv", "armor.csv", "foot_information.csv", "map.csv", 
                "character_status.csv", "growth_curve.csv"
            };
            
            string[] dataMessage = { "system_en.txt", "story_mes_en.txt" };

            string basePath = Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR");
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }

            CreateDirectoryStructure();
            CopyDataFiles(dataMaster, dataMessage);
            CopyCharacterSprites();
            CopyMapFiles();
        }

        public void ApplySelectedSprites(List<string> spriteSelections, string airshipSprite, string boatSprite)
        {
            foreach (var item in spriteSelections)
            {
                try
                {
                    string[] parts = item.Split(new[] { ": " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        string characterPart = parts[0];
                        string spritePart = parts[1];

                        bool includeJobUpgrade = characterPart.StartsWith("(+JU)");
                        string character = includeJobUpgrade ? characterPart.Substring(6) : characterPart;

                        string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                        SpriteUpdater.ReplaceSprite(baseFolder, character, spritePart, gameDirectory, includeJobUpgrade);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error processing sprite '{item}': {ex.Message}");
                }
            }

            ApplyVehicleSprites(airshipSprite, boatSprite);
        }



        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDirName}");

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        public string GetDataPath()
        {
            return Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", 
                "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master");
        }

        public string GetMessagePath()
        {
            return Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", 
                "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message");
        }

        public string GetStreamingAssetsPath()
        {
            return Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets");
        }

        private void CreateDirectoryStructure()
        {
            Directory.CreateDirectory(Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", 
                "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master"));
            Directory.CreateDirectory(Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", 
                "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message"));
        }

        private void CopyDataFiles(string[] dataMaster, string[] dataMessage)
        {
            string dataMasterPath = GetDataPath();
            string dataMessagePath = GetMessagePath();

            foreach (string file in dataMaster)
            {
                string outputPath = Path.Combine(dataMasterPath, file);
                string sourcePath = Path.Combine("data", "assets", file);
                File.Copy(sourcePath, outputPath, true);
            }

            foreach (string file in dataMessage)
            {
                string outputPath = Path.Combine(dataMessagePath, file);
                string sourcePath = Path.Combine("data", "assets", file);
                File.Copy(sourcePath, outputPath, true);
            }
        }

        private void CopyCharacterSprites()
        {
            string[] characterFolders = {
                "mo_ff1_p001_c00", "mo_ff1_p002_c00", "mo_ff1_p003_c00", "mo_ff1_p004_c00",
                "mo_ff1_p005_c00", "mo_ff1_p006_c00", "mo_ff1_p007_c00", "mo_ff1_p008_c00",
                "mo_ff1_p009_c00", "mo_ff1_p010_c00", "mo_ff1_p011_c00", "mo_ff1_p012_c00"
            };

            string spritesSourcePath = Path.Combine("data", "sprites");
            string spritesDestBasePath = Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR");

            foreach (string characterFolder in characterFolders)
            {
                string sourceSpritePath = Path.Combine(spritesSourcePath, characterFolder);
                string destSpritePath = Path.Combine(spritesDestBasePath, characterFolder);

                if (Directory.Exists(destSpritePath))
                    Directory.Delete(destSpritePath, true);

                if (Directory.Exists(sourceSpritePath))
                    DirectoryCopy(sourceSpritePath, destSpritePath, true);
            }
        }

        private void CopyMapFiles()
        {
            string resMapPath = GetStreamingAssetsPath();
            
            Updater.MemoriaToMagiciteCopy(resMapPath, Path.Combine("data", "master"), "MainData", "master");
            Updater.MemoriaToMagiciteCopy(resMapPath, Path.Combine("data", "messages"), "Message", "message");

            foreach (string mapDir in Directory.GetDirectories(Path.Combine("data", "maps")))
            {
                string packageJsonPath = Path.Combine(mapDir, "package.json");
                if (File.Exists(packageJsonPath))
                {
                    string destPackageJsonPath = Path.Combine(resMapPath, "Magicite", "FF1PRR", 
                        Path.GetFileName(mapDir), "Assets", "GameAssets", "Serial", "Res", "Map", 
                        Path.GetFileName(mapDir), "package.json");
                    Directory.CreateDirectory(Path.GetDirectoryName(destPackageJsonPath));
                    File.Copy(packageJsonPath, destPackageJsonPath, true);
                }

                foreach (string submapDir in Directory.GetDirectories(mapDir))
                {
                    string topKey = Path.GetFileName(mapDir);
                    string submapName = Path.GetFileName(submapDir);
                    RemoveCustomScripts(resMapPath, topKey, submapName);
                    Updater.MemoriaToMagiciteCopy(resMapPath, submapDir, "Map", topKey);
                }
            }
        }

        private void ApplyVehicleSprites(string airshipSprite, string boatSprite)
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;

            if (airshipSprite != "None")
            {
                try
                {
                    SpriteUpdater.ReplaceAirshipSprite(baseFolder, airshipSprite, gameDirectory);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error applying airship sprite '{airshipSprite}': {ex.Message}");
                }
            }

            if (boatSprite != "None")
            {
                try
                {
                    SpriteUpdater.ReplaceBoatSprite(baseFolder, boatSprite, gameDirectory);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error applying boat sprite '{boatSprite}': {ex.Message}");
                }
            }
        }

        private void RemoveCustomScripts(string resMapPath, string mapDirName, string submapDirName)
        {
            string mapRootPath = Path.Combine(resMapPath, "Magicite", "FF1PRR", mapDirName, 
                "Assets", "GameAssets", "Serial", "Res", "Map", mapDirName);
            string[] customScripts = { "sc_t_0099.json", "sc_t_0099_after.json" };

            foreach (string script in customScripts)
            {
                string scriptPath = Path.Combine(mapRootPath, submapDirName, script);
                if (File.Exists(scriptPath))
                    File.Delete(scriptPath);
            }
        }
    }
}