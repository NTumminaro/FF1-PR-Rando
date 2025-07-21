using FF1_PRR.Randomize;
using FF1_PRR.Inventory;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FF1_PRR.Common
{
    public class RandomizerEngine
    {
        private Random r1;
        private string gameDirectory;
        private ConfigurationManager.RandomizerConfig config;
        private FileManager fileManager;
        private RandomizerLogger logger;

        public RandomizerEngine(ConfigurationManager.RandomizerConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.gameDirectory = config.FF1PRFolder;
            this.r1 = new Random(Convert.ToInt32(config.Seed));
            this.fileManager = new FileManager(gameDirectory);
            
            // Initialize logger
            this.logger = new RandomizerLogger();
            this.logger.LogSystemDiagnostics();
            this.logger.LogConfiguration(config);
            
            // Validate initial configuration
            ValidateInitialConfiguration();
        }

        /// <summary>
        /// Validates the initial configuration and system requirements
        /// </summary>
        private void ValidateInitialConfiguration()
        {
            using var operation = logger.StartOperation("Initial Configuration Validation");
            
            try
            {
                var validationResults = new List<ValidationResult>();

                // Validate game directory
                validationResults.Add(ValidationUtility.ValidateDirectoryExists(gameDirectory, "FF1PR Game Directory"));
                
                // Validate required data directories
                var requiredDirectories = new[]
                {
                    "data",
                    Path.Combine("data", "assets"),
                    Path.Combine("data", "mods"),
                    Path.Combine("data", "maps")
                };
                
                foreach (var dir in requiredDirectories)
                {
                    validationResults.Add(ValidationUtility.ValidateDirectoryExists(dir, $"Required Directory: {dir}"));
                }

                // Validate required data files
                var requiredFiles = new[]
                {
                    Path.Combine("data", "assets", "ability.csv"),
                    Path.Combine("data", "assets", "product.csv"),
                    Path.Combine("data", "assets", "monster.csv"),
                    Path.Combine("data", "mods", "system_en.txt")
                };
                
                validationResults.Add(ValidationUtility.ValidateMultipleFilesExist(requiredFiles, "Required Data Files"));

                // Validate disk space (estimate 500MB needed)
                validationResults.Add(ValidationUtility.ValidateDiskSpace(gameDirectory, 500 * 1024 * 1024, "Game Directory"));

                // Validate directory permissions
                validationResults.Add(ValidationUtility.ValidateDirectoryPermissions(gameDirectory, true, "Game Directory Write Access"));

                // Log all validation results
                logger.LogValidations(validationResults, "Configuration Validation");

                // Check for any failures
                var failures = validationResults.Where(r => !r.IsValid).ToList();
                if (failures.Any())
                {
                    var errorMessage = $"Configuration validation failed:\n{string.Join("\n", failures.Select(f => f.ErrorMessage))}";
                    operation.Fail(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                operation.Complete("Configuration validation passed");
            }
            catch (Exception ex)
            {
                operation.Fail("Configuration validation error", ex);
                throw;
            }
        }

        public class RandomizationOptions
        {
            public bool KeyItems { get; set; }
            public bool ShopsTrad { get; set; }
            public bool MagicShuffleShops { get; set; }
            public bool MagicKeepPermissions { get; set; }
            public bool MagicRandomizeClassPermissions { get; set; }
            public bool ReduceEncounterRate { get; set; }
            public bool TreasureTrad { get; set; }
            public bool RebalanceBosses { get; set; }
            public bool FiendsDropRibbons { get; set; }
            public bool RebalancePrices { get; set; }
            public bool RestoreCritRating { get; set; }
            public bool WandsAddInt { get; set; }
            public bool NoEscapeNES { get; set; }
            public bool NoEscapeRandomize { get; set; }
            public bool ReduceChaosHP { get; set; }
            public bool HeroStatsStandardize { get; set; }
            public bool BoostPromoted { get; set; }
            public bool SecretChaos { get; set; }
            public bool DockAnywhere { get; set; }
            public bool ShuffleCanoe { get; set; }
            public bool IncludeAllBosses { get; set; }
            public bool JackInTheBox { get; set; }
            public bool BossShuffle { get; set; }
            public bool ShuffleBackgrounds { get; set; }
            public bool ShuffleMonsterEncounters { get; set; }

            public int ModeShops { get; set; }
            public int ModeXPBoost { get; set; }
            public int ModeTreasure { get; set; }
            public int ModeMagic { get; set; }
            public int ModeMonsterStatAdjustment { get; set; }
            public int ModeHeroStats { get; set; }
            public int ChaosHpValue { get; set; }
            public int ModeShuffleNPCs { get; set; }
            public int ModeAirshipSprite { get; set; }
            public int ModeBoatSprite { get; set; }
        }

        public void ExecuteRandomization(RandomizationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            using var operation = logger.StartOperation("Full Randomization Process");
            
            try
            {
                logger.Info("Starting randomization process", "Randomization");
                
                PrepareGameFiles(options);
                ApplyDatabaseEdits(options);
                ExecuteRandomizationSteps(options);
                FinalizeRandomization(options);
                
                operation.Complete("Randomization completed successfully");
                logger.Info("Randomization process completed successfully", "Randomization");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Randomization failed: {ex.Message}";
                operation.Fail(errorMessage, ex);
                logger.Critical(errorMessage, "Randomization", ex);
                throw new RandomizationException(errorMessage, ex);
            }
        }

        private void PrepareGameFiles(RandomizationOptions options)
        {
            using var operation = logger.StartOperation("Prepare Game Files");
            
            try
            {
                logger.Info("Updating game installation", "File Preparation");
                Updater.update(gameDirectory);
                
                logger.Info("Restoring vanilla files", "File Preparation");
                fileManager.RestoreVanillaFiles();
                
                logger.Info("Applying mod files", "File Preparation");
                ApplyModFiles(options);
                
                operation.Complete("Game files prepared successfully");
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to prepare game files", ex);
                throw new FileOperationException("game files", "Failed to prepare game files", ex);
            }
        }

        private void UpdateChaosHpCsv(RandomizationOptions options)
        {
            if (!options.ReduceChaosHP) return;

            using var operation = logger.StartOperation("Update Chaos HP CSV");
            
            try
            {
                int[] hpValues = { 1, 9999, 20000, 30000 };
                int selectedHp = hpValues[Math.Min(options.ChaosHpValue, hpValues.Length - 1)];

                string chaosHpCsvPath = Path.Combine("data", "dataReduceChaosHP.csv");
                
                // Validate file exists
                var fileValidation = ValidationUtility.ValidateFileExists(chaosHpCsvPath, "Chaos HP CSV");
                if (!fileValidation.IsValid)
                {
                    logger.Warning($"Chaos HP CSV not found, skipping: {chaosHpCsvPath}", "Database Edits");
                    operation.Complete("Skipped - file not found");
                    return;
                }

                logger.Info($"Updating Chaos HP to {selectedHp}", "Database Edits");
                
                var lines = File.ReadAllLines(chaosHpCsvPath).ToList();
                bool updated = false;
                
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("monster.csv") && lines[i].Contains("Chaos"))
                    {
                        var parts = lines[i].Split(',');
                        if (parts.Length > 4)
                        {
                            parts[4] = selectedHp.ToString(); // Update HP value
                            lines[i] = string.Join(",", parts);
                            updated = true;
                        }
                    }
                }

                if (updated)
                {
                    File.WriteAllLines(chaosHpCsvPath, lines);
                    operation.Complete($"Updated Chaos HP to {selectedHp}");
                }
                else
                {
                    logger.Warning("No Chaos entries found to update in HP CSV", "Database Edits");
                    operation.Complete("No updates needed");
                }
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to update Chaos HP CSV", ex);
                throw new FileOperationException(Path.Combine("data", "dataReduceChaosHP.csv"), "Failed to update Chaos HP", ex);
            }
        }

        private void ApplyDatabaseEdits(RandomizationOptions options)
        {
            using var operation = logger.StartOperation("Apply Database Edits");
            
            try
            {
                UpdateChaosHpCsv(options);

                var editsToMake = new List<DatabaseEdit>();
                string dataPath = GetDataPath();

                // Validate data path exists
                var dataPathValidation = ValidationUtility.ValidateDirectoryExists(dataPath, "Data Path");
                if (!dataPathValidation.IsValid)
                {
                    throw new DirectoryNotFoundException($"Data path not found: {dataPath}");
                }

                logger.Info("Loading database edit configurations", "Database Edits");

                if (options.RebalancePrices)
                {
                    r1.NextBytes(new byte[1]);
                    editsToMake.AddRange(LoadEdits("dataRebalancePrices.csv"));
                    logger.Debug("Loaded rebalance prices edits", "Database Edits");
                }
                if (options.FiendsDropRibbons)
                {
                    r1.NextBytes(new byte[2]);
                    editsToMake.AddRange(LoadEdits("dataFiendsDropRibbons.csv"));
                    logger.Debug("Loaded fiends drop ribbons edits", "Database Edits");
                }
                if (options.RebalanceBosses)
                {
                    r1.NextBytes(new byte[4]);
                    editsToMake.AddRange(LoadEdits("dataRebalanceBosses.csv"));
                    logger.Debug("Loaded rebalance bosses edits", "Database Edits");
                }
                if (options.RestoreCritRating)
                {
                    r1.NextBytes(new byte[8]);
                    editsToMake.AddRange(LoadEdits("dataRestoreCritRating.csv"));
                    logger.Debug("Loaded restore crit rating edits", "Database Edits");
                }
                if (options.WandsAddInt)
                {
                    r1.NextBytes(new byte[16]);
                    editsToMake.AddRange(LoadEdits("dataWandsAddInt.csv"));
                    logger.Debug("Loaded wands add int edits", "Database Edits");
                }
                if (options.ReduceEncounterRate)
                {
                    r1.NextBytes(new byte[32]);
                    editsToMake.AddRange(LoadEdits("dataReduceEncounterRate.csv"));
                    logger.Debug("Loaded reduce encounter rate edits", "Database Edits");
                }
                if (options.ReduceChaosHP)
                {
                    r1.NextBytes(new byte[64]);
                    editsToMake.AddRange(LoadEdits("dataReduceChaosHP.csv"));
                    logger.Debug("Loaded reduce chaos HP edits", "Database Edits");
                }

                logger.Info($"Applying {editsToMake.Count} database edits", "Database Edits");
                ApplyEditsToFiles(editsToMake, dataPath);
                
                operation.Complete($"Applied {editsToMake.Count} database edits successfully");
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to apply database edits", ex);
                throw new RandomizationException("Failed to apply database edits", ex);
            }
        }

        private void ExecuteRandomizationSteps(RandomizationOptions options)
        {
            if (options.ModeMagic > 0)
                RandomizeMagic(options);

            if (options.ModeShops > 0)
                RandomizeShops(options);

            if (options.KeyItems)
                RandomizeKeyItems(options);

            if (options.ModeTreasure > 0)
                RandomizeTreasure(options);

            if (options.NoEscapeNES)
                ApplyNoEscapeAdjustment(options);

            if (options.HeroStatsStandardize || options.ModeHeroStats > 0)
                RandomizeHeroStats(options);

            if (options.ShuffleBackgrounds)
                ApplyCosmetics(options);

            if (options.BossShuffle)
                ShuffleBosses(options);

            if (options.ShuffleMonsterEncounters)
                ShuffleMonsterEncounters(options);

            ApplyMonsterBoost(options);
        }

        private void RandomizeMagic(RandomizationOptions options)
        {
            var magic = new Magic(r1, options.ModeMagic,
                Path.Combine(GetDataPath(), "ability.csv"),
                Path.Combine(GetDataPath(), "product.csv"),
                options.MagicShuffleShops, options.MagicKeepPermissions, options.MagicRandomizeClassPermissions);
        }

        private void RandomizeShops(RandomizationOptions options)
        {
            var shops = new Shops(r1, options.ModeShops,
                Path.Combine(GetDataPath(), "product.csv"),
                options.ShopsTrad);
        }

        private void RandomizeKeyItems(RandomizationOptions options)
        {
            var keyItems = new KeyItems(r1,
                Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets"),
                options.DockAnywhere, options.ShuffleCanoe);
        }

        private void RandomizeTreasure(RandomizationOptions options)
        {
            bool useJackInTheBox = DetermineSpecialFeatureUsage(options.JackInTheBox, options.SecretChaos, 0);
            var treasure = new Treasure(r1, options.ModeTreasure,
                Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets"),
                options.TreasureTrad, options.FiendsDropRibbons, useJackInTheBox);
        }

        private bool DetermineSpecialFeatureUsage(bool feature, bool secretChaos, int randomChoice)
        {
            if (feature && secretChaos)
            {
                return r1.Next(2) == randomChoice;
            }
            return feature;
        }

        private void ApplyNoEscapeAdjustment(RandomizationOptions options)
        {
            MonsterParty.mandatoryRandomEncounters(r1, fileManager.GetDataPath(), options.NoEscapeRandomize);
        }

        private void RandomizeHeroStats(RandomizationOptions options)
        {
            Stats.RandomizeStats(options.ModeHeroStats, options.HeroStatsStandardize, options.BoostPromoted, 
                r1, fileManager.GetDataPath(), fileManager.GetMessagePath());
        }

        private void ApplyCosmetics(RandomizationOptions options)
        {
            var cosmetics = new Cosmetics(r1, Path.Combine("data", "mods"), 
                fileManager.GetDataPath(), options.ShuffleBackgrounds);
        }

        private void ShuffleBosses(RandomizationOptions options)
        {
            var bossShuffle = new ShuffleBosses(r1, 
                Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR"),
                options.BossShuffle, options.IncludeAllBosses);
        }

        private void ShuffleMonsterEncounters(RandomizationOptions options)
        {
            var monsterEncounters = new MonsterEncounters(r1, GetDataPath(), options.ShuffleMonsterEncounters);
        }

        private void ApplyMonsterBoost(RandomizationOptions options)
        {
            double xp = GetXPMultiplier(options.ModeXPBoost);
            var (minStat, maxStat) = GetStatAdjustmentRange(options.ModeMonsterStatAdjustment);

            string monsterFile = Path.Combine(fileManager.GetDataPath(), "monster.csv");
            var monsters = new Monster(r1, monsterFile, xp, 0, xp, 0, minStat, maxStat);
        }

        private double GetXPMultiplier(int mode)
        {
            return mode switch
            {
                0 => 0.5,
                1 => 1.0,
                2 => 1.5,
                3 => 2.0,
                4 => 3.0,
                5 => 4.0,
                6 => 5.0,
                _ => 10.0
            };
        }

        private (double min, double max) GetStatAdjustmentRange(int mode)
        {
            return mode switch
            {
                1 => (0.6666667, 1.5),
                2 => (0.5, 2.0),
                3 => (0.3333333, 3.0),
                4 => (0.25, 4.0),
                5 => (0.2, 5.0),
                6 => (1.0, 1.25),
                7 => (1.0, 1.5),
                8 => (1.0, 2.0),
                9 => (1.0, 3.0),
                10 => (1.0, 4.0),
                11 => (1.0, 5.0),
                _ => (1.0, 1.0)
            };
        }

        private void FinalizeRandomization(RandomizationOptions options)
        {
            ApplyFinalFiles(options);
            ModifySystemMessage();
            
            // Always apply randomizer title logo at the end
            string resMapPath = fileManager.GetStreamingAssetsPath();
            ApplyTitleLogo(resMapPath);
        }

        private void ModifySystemMessage()
        {
            using var operation = logger.StartOperation("Modify System Messages");
            
            try
            {
                string messagePath = GetMessagePath();
                string systemMessagePath = Path.Combine("data", "mods", "system_en.txt");
                string modifiedMessagePath = Path.Combine(messagePath, "system_en.txt");

                if (!File.Exists(systemMessagePath))
                {
                    operation.Fail($"System message file not found: {systemMessagePath}");
                    throw new FileNotFoundException($"The file {systemMessagePath} was not found.");
                }

                var lines = File.ReadAllLines(systemMessagePath).ToList();
                bool message183Found = false;
                bool message181Found = false;

                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("MSG_SYSTEM_CS_0_047\t"))
                    {
                        // Replace the entire message to avoid formatting issues with existing \n
                        lines[i] = $"MSG_SYSTEM_CS_0_047\t© SQUARE ENIX\\nLOGO & IMAGE ILLUSTRATION: © YOSHITAKA AMANO\\nSeed: {config.Seed} - Flags: {config.RandoFlags}";
                        message181Found = true;
                        logger.Debug($"Updated MSG_SYSTEM_CS_0_047 with seed and flags", "System Messages");
                    }
                    if (lines[i].StartsWith("MSG_SYSTEM_183\t"))
                    {
                        lines[i] = $"MSG_SYSTEM_183\tSeed: {config.Seed} - Flags: {config.RandoFlags}";
                        message183Found = true;
                        logger.Debug($"Updated MSG_SYSTEM_183 with seed and flags", "System Messages");
                    }
                }

                if (!message183Found || !message181Found)
                {
                    var missingMessages = new List<string>();
                    if (!message183Found) missingMessages.Add("MSG_SYSTEM_183");
                    if (!message181Found) missingMessages.Add("MSG_SYSTEM_181");
                    
                    operation.Fail($"Required system messages not found: {string.Join(", ", missingMessages)}");
                    throw new Exception($"Required system messages not found in system_en.txt: {string.Join(", ", missingMessages)}");
                }

                File.WriteAllLines(modifiedMessagePath, lines);
                
                operation.Complete($"Updated system messages with seed {config.Seed} and flags {config.RandoFlags}");
                logger.Info($"System messages updated successfully", "System Messages");
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to modify system messages", ex);
                logger.Error("Failed to modify system messages", "System Messages", ex);
                throw new RandomizationException("Failed to modify system messages", ex);
            }
        }



        private void ApplyModFiles(RandomizationOptions options)
        {
            string dataPath = GetDataPath();
            string messagePath = GetMessagePath();
            string resMapPath = fileManager.GetStreamingAssetsPath();

            File.Copy(Path.Combine("data", "mods", "system_en.txt"), 
                Path.Combine(messagePath, "system_en.txt"), true);

            if (options.ShopsTrad)
                File.Copy(Path.Combine("data", "mods", "productTraditional.csv"), 
                    Path.Combine(dataPath, "product.csv"), true);
            else
                File.Copy(Path.Combine("data", "mods", "product.csv"), 
                    Path.Combine(dataPath, "product.csv"), true);

            bool useJackInTheBox = DetermineSpecialFeatureUsage(options.JackInTheBox, options.SecretChaos, 0);
            bool useSecretChaos = DetermineSpecialFeatureUsage(options.SecretChaos, options.JackInTheBox, 1);

            if (useJackInTheBox)
            {
                CopySpecialFiles(dataPath, messagePath);
            }

            // Copy map files
            foreach (string jsonFile in Directory.GetDirectories(Path.Combine("data", "mods", "maps"), "*.*", SearchOption.AllDirectories))
            {
                if (jsonFile.Count(f => f == '\\') != 3) continue;
                Updater.MemoriaToMagiciteCopy(resMapPath, jsonFile, "Map", Path.GetFileName(jsonFile));
            }

            // Apply NPC modifications
            ApplyNPCModifications(options, useSecretChaos);
        }

        private void CopySpecialFiles(string dataPath, string messagePath)
        {
            File.Copy(Path.Combine("data", "mods", "script.csv"), 
                Path.Combine(dataPath, "script.csv"), true);
            File.Copy(Path.Combine("data", "mods", "story_mes_en.txt"), 
                Path.Combine(messagePath, "story_mes_en.txt"), true);
        }

        private void ApplyNPCModifications(RandomizationOptions options, bool useSecretChaos)
        {
            string dataPath = GetDataPath();
            string messagePath = GetMessagePath();
            string resMapPath = fileManager.GetStreamingAssetsPath();
            string modsDir = Path.Combine("data", "mods");

            if (useSecretChaos && options.ModeShuffleNPCs == 3)
            {
                new NPCs(r1, resMapPath, useSecretChaos, true);
            }
            else if (useSecretChaos && options.ModeShuffleNPCs != 3)
            {
                new NPCs(r1, resMapPath, useSecretChaos, false);
            }

            if (options.ModeShuffleNPCs == 0)
            {
                new NPCAssets(r1, modsDir, dataPath, false, false);
            }
            else if (options.ModeShuffleNPCs == 1)
            {
                new NPCAssets(r1, modsDir, dataPath, false, true);
            }
            else if (options.ModeShuffleNPCs == 2)
            {
                new NPCAssets(r1, modsDir, dataPath, true, false);
            }

            if (useSecretChaos)
            {
                File.Copy(Path.Combine("data", "mods", "script.csv"), Path.Combine(resMapPath, "script.csv"), true);
                CopySpecialFiles(dataPath, messagePath);
            }
        }

        private void ApplyTitleLogo(string resMapPath)
        {
            using var operation = logger.StartOperation("Apply Randomizer Title Logo");
            
            try
            {
                string sourceLogo = Path.Combine("data", "mods", "title", "TitleLogoImage_EN.png");
                
                // Validate source file exists
                var sourceValidation = ValidationUtility.ValidateFileExists(sourceLogo, "Randomizer Title Logo");
                if (!sourceValidation.IsValid)
                {
                    operation.Fail($"Title logo source file not found: {sourceLogo}");
                    throw new FileNotFoundException($"Title logo source file not found: {sourceLogo}");
                }

                // Create the proper Magicite structure with keys and Assets
                string topKey = "common_title";
                string topValue = Path.Combine("Assets", "GameAssets", "Serial", "Res", "UI", "Common", "Title", "Sprite");
                
                // Create directories
                string assetsDir = Path.Combine(resMapPath, "Magicite", "FF1PRR", topKey, topValue);
                string keysDir = Path.Combine(resMapPath, "Magicite", "FF1PRR", topKey, "keys");
                Directory.CreateDirectory(assetsDir);
                Directory.CreateDirectory(keysDir);

                // Copy the title logo PNG file
                string destLogoPath = Path.Combine(assetsDir, "TitleLogoImage_EN.png");
                File.Copy(sourceLogo, destLogoPath, true);

                // Also copy the spritedata file if it exists
                string sourceSpriteData = Path.Combine("data", "mods", "title", "TitleLogoImage_EN.spritedata");
                if (File.Exists(sourceSpriteData))
                {
                    string destSpriteDataPath = Path.Combine(assetsDir, "TitleLogoImage_EN.spritedata");
                    File.Copy(sourceSpriteData, destSpriteDataPath, true);
                    logger.Debug("Copied TitleLogoImage_EN.spritedata companion file", "Title Logo");
                }
                else
                {
                    logger.Warning("TitleLogoImage_EN.spritedata not found - image may not render properly", "Title Logo");
                }

                // Create the Export.json file for Magicite
                var importJson = new Updater.ImportData();
                importJson.keys.Add("TitleLogoImage_EN");
                importJson.values.Add(topValue.Replace('\\', '/') + "/TitleLogoImage_EN");

                string exportJsonPath = Path.Combine(keysDir, "Export.json");
                using (var writer = new StreamWriter(exportJsonPath))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, importJson);
                }
                
                operation.Complete("Randomizer title logo applied successfully with proper Magicite structure");
                logger.Info("Applied randomizer title logo with keys and Export.json", "Title Logo");
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to apply title logo", ex);
                logger.Error("Failed to apply randomizer title logo", "Title Logo", ex);
                throw new FileOperationException("TitleLogoImage_EN.png", "Failed to apply title logo", ex);
            }
        }

        private void ApplyFinalFiles(RandomizationOptions options)
        {
            if (options.DockAnywhere)
            {
                string resMapPath = Path.Combine(gameDirectory, "FINAL FANTASY_Data", "StreamingAssets");
                File.Copy(Path.Combine("data", "mods", "landing_group.csv"),
                    Path.Combine(resMapPath, "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master", "landing_group.csv"), true);
            }
        }



        private string GetDataPath()
        {
            return fileManager.GetDataPath();
        }

        private string GetMessagePath()
        {
            return fileManager.GetMessagePath();
        }

        private List<DatabaseEdit> LoadEdits(string filename)
        {
            using var operation = logger.StartOperation($"Load Database Edits: {filename}");
            
            try
            {
                string filePath = Path.Combine("data", filename);
                
                // Validate file exists and has correct format
                var fileValidation = ValidationUtility.ValidateFileExists(filePath, $"Database Edit File: {filename}");
                if (!fileValidation.IsValid)
                {
                    operation.Fail($"Database edit file not found: {filename}");
                    throw new FileNotFoundException($"Database edit file not found: {filePath}");
                }

                var csvValidation = ValidationUtility.ValidateCsvFormat(filePath, new[] { "file", "id", "field", "value" }, $"Database Edit CSV: {filename}");
                if (!csvValidation.IsValid)
                {
                    operation.Fail($"Invalid CSV format: {filename}");
                    throw new InvalidDataException($"Invalid CSV format in {filePath}: {csvValidation.ErrorMessage}");
                }

                List<DatabaseEdit> edits;
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    edits = csv.GetRecords<DatabaseEdit>().ToList();
                }

                operation.Complete($"Loaded {edits.Count} database edits from {filename}");
                return edits;
            }
            catch (Exception ex)
            {
                operation.Fail($"Failed to load database edits from {filename}", ex);
                throw new FileOperationException(Path.Combine("data", filename), $"Failed to load database edits", ex);
            }
        }

        private void ApplyEditsToFiles(List<DatabaseEdit> edits, string dataPath)
        {
            using var operation = logger.StartOperation("Apply Edits to Files");
            
            try
            {
                var fileGroups = edits.GroupBy(x => x.file).ToList();
                logger.Info($"Applying edits to {fileGroups.Count} files", "Database Edits");

                foreach (var editsByFile in fileGroups)
                {
                    using var fileOperation = logger.StartOperation($"Apply Edits to {editsByFile.Key}");
                    
                    try
                    {
                        string filePath = Path.Combine(dataPath, editsByFile.Key);
                        
                        // Validate target file exists
                        var fileValidation = ValidationUtility.ValidateFileExists(filePath, $"Target CSV File: {editsByFile.Key}");
                        if (!fileValidation.IsValid)
                        {
                            fileOperation.Fail($"Target file not found: {editsByFile.Key}");
                            throw new FileNotFoundException($"Target file not found: {filePath}");
                        }

                        // Validate CSV format
                        var csvValidation = ValidationUtility.ValidateCsvFormat(filePath, null, $"Target CSV: {editsByFile.Key}");
                        if (!csvValidation.IsValid)
                        {
                            fileOperation.Fail($"Invalid CSV format: {editsByFile.Key}");
                            throw new InvalidDataException($"Invalid CSV format in {filePath}: {csvValidation.ErrorMessage}");
                        }

                        // Validate file permissions
                        var permissionValidation = ValidationUtility.ValidateFilePermissions(filePath, true, $"CSV File: {editsByFile.Key}");
                        if (!permissionValidation.IsValid)
                        {
                            fileOperation.Fail($"Insufficient permissions: {editsByFile.Key}");
                            throw new UnauthorizedAccessException($"Insufficient permissions for {filePath}: {permissionValidation.ErrorMessage}");
                        }

                        List<dynamic> fileToEdit;
                        using (var reader = new StreamReader(filePath))
                        using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            fileToEdit = csv.GetRecords<dynamic>().ToList();
                        }

                        int appliedEdits = 0;
                        var failedEdits = new List<string>();

                        foreach (var edit in editsByFile)
                        {
                            try
                            {
                                var itemDict = fileToEdit.Find(x => x.id == edit.id) as IDictionary<string, object>;
                                if (itemDict != null)
                                {
                                    if (itemDict.ContainsKey(edit.field))
                                    {
                                        itemDict[edit.field] = edit.value;
                                        appliedEdits++;
                                        logger.Debug($"Applied edit: {edit.id}.{edit.field} = {edit.value}", "Database Edits");
                                    }
                                    else
                                    {
                                        failedEdits.Add($"Field '{edit.field}' not found in record '{edit.id}'");
                                        logger.Warning($"Field '{edit.field}' not found in record '{edit.id}' for file {editsByFile.Key}", "Database Edits");
                                    }
                                }
                                else
                                {
                                    failedEdits.Add($"Record '{edit.id}' not found");
                                    logger.Warning($"Record '{edit.id}' not found in file {editsByFile.Key}", "Database Edits");
                                }
                            }
                            catch (Exception ex)
                            {
                                failedEdits.Add($"Error applying edit {edit.id}.{edit.field}: {ex.Message}");
                                logger.Error($"Error applying edit {edit.id}.{edit.field} in file {editsByFile.Key}: {ex.Message}", "Database Edits");
                            }
                        }

                        // Write the modified file
                        using (var writer = new StreamWriter(filePath))
                        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(fileToEdit);
                        }

                        if (failedEdits.Any())
                        {
                            logger.Warning($"Some edits failed for {editsByFile.Key}: {string.Join(", ", failedEdits)}", "Database Edits");
                        }

                        fileOperation.Complete($"Applied {appliedEdits}/{editsByFile.Count()} edits to {editsByFile.Key}");
                    }
                    catch (Exception ex)
                    {
                        fileOperation.Fail($"Failed to apply edits to {editsByFile.Key}", ex);
                        throw new FileOperationException(Path.Combine(dataPath, editsByFile.Key), $"Failed to apply database edits", ex);
                    }
                }

                operation.Complete($"Applied edits to {fileGroups.Count} files successfully");
            }
            catch (Exception ex)
            {
                operation.Fail("Failed to apply edits to files", ex);
                throw;
            }
        }

        private class DatabaseEdit
        {
            public string file { get; set; }
            public string name { get; set; }
            public string id { get; set; }
            public string field { get; set; }
            public string value { get; set; }
            public string comment { get; set; }
        }
    }
}