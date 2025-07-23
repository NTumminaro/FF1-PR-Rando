using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FF1_PRR.Common
{
    public class UIController
    {
        private readonly Form form;
        private ConfigurationManager.RandomizerConfig config;
        private bool loading = true;

        public UIController(Form form)
        {
            this.form = form;
            this.config = ConfigurationManager.GetDefaultConfig();
        }

        public void InitializeForm()
        {
            LoadConfiguration();
            PopulateDropdowns();
            SetupEventHandlers();
            loading = false;
        }

        public void UpdateFlags(CheckBox[] flagGroup1, CheckBox[] flagGroup2, CheckBox[] flagGroup3, CheckBox[] flagGroup4,
            ComboBox modeShops, ComboBox modeXPBoost, ComboBox modeTreasure, ComboBox modeMagic,
            ComboBox modeMonsterStatAdjustment, ComboBox modeHeroStats, TrackBar chaosHpTrackBar,
            CheckBox flagJackInTheBox, TextBox randoFlags)
        {
            // Don't update flags if we're currently loading/applying settings to prevent circular updates
            if (loading) return;

            string flags = "";
            flags += ConfigurationManager.ConvertFlagsToString(flagGroup1);
            flags += ConfigurationManager.ConvertFlagsToString(flagGroup2);
            flags += ConfigurationManager.ConvertFlagsToString(flagGroup3);
            flags += ConfigurationManager.ConvertFlagsToString(flagGroup4);

            flags += ConfigurationManager.ConvertIntToChar(modeShops.SelectedIndex + (8 * modeXPBoost.SelectedIndex));
            flags += ConfigurationManager.ConvertIntToChar(modeTreasure.SelectedIndex + (8 * modeMagic.SelectedIndex));
            flags += ConfigurationManager.ConvertIntToChar(modeMonsterStatAdjustment.SelectedIndex + (16 * 0));
            flags += ConfigurationManager.ConvertIntToChar(modeHeroStats.SelectedIndex);

            flags += chaosHpTrackBar.Value.ToString();
            flags += flagJackInTheBox.Checked ? "1" : "0";

            randoFlags.Text = flags;
            config.RandoFlags = flags;
        }

        public void UpdateVisualFlags(ComboBox modeShuffleNPCs, ComboBox modeAirshipSprite, ComboBox modeBoatSprite,
            CheckBox flagShuffleBackgrounds, CheckBox flagShowBossNames, TextBox visualFlags)
        {
            // Don't update flags if we're currently loading/applying settings to prevent circular updates
            if (loading) return;

            string flags = "";
            flags += modeShuffleNPCs.SelectedIndex.ToString();
            flags += modeAirshipSprite.SelectedIndex.ToString("X");
            flags += modeBoatSprite.SelectedIndex.ToString("X");
            flags += flagShuffleBackgrounds.Checked ? "1" : "0";
            flags += flagShowBossNames.Checked ? "1" : "0";

            visualFlags.Text = flags;
            config.VisualFlags = flags;
        }

        public void ApplyFlags(string randoFlags, string visualFlags,
            CheckBox[] flagGroup1, CheckBox[] flagGroup2, CheckBox[] flagGroup3, CheckBox[] flagGroup4,
            ComboBox modeShops, ComboBox modeXPBoost, ComboBox modeTreasure, ComboBox modeMagic,
            ComboBox modeMonsterStatAdjustment, ComboBox modeHeroStats, TrackBar chaosHpTrackBar,
            CheckBox flagJackInTheBox, Label chaosHpLabel,
            ComboBox modeShuffleNPCs, ComboBox modeAirshipSprite, ComboBox modeBoatSprite,
            CheckBox flagShuffleBackgrounds, CheckBox flagShowBossNames)
        {
            if (randoFlags.Length < 10) return;
            if (visualFlags.Length < 4) return;

            loading = true;

            // Apply rando flags
            ConfigurationManager.ApplyFlagsToCheckboxes(randoFlags.Substring(0, 1), flagGroup1);
            ConfigurationManager.ApplyFlagsToCheckboxes(randoFlags.Substring(1, 1), flagGroup2);
            ConfigurationManager.ApplyFlagsToCheckboxes(randoFlags.Substring(2, 1), flagGroup3);
            ConfigurationManager.ApplyFlagsToCheckboxes(randoFlags.Substring(3, 1), flagGroup4);

            modeShops.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[4]) % 8;
            modeXPBoost.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[4]) / 8;
            modeTreasure.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[5]) % 8;
            modeMagic.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[5]) / 8;
            modeMonsterStatAdjustment.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[6]) % 16;
            modeHeroStats.SelectedIndex = ConfigurationManager.ConvertCharToInt(randoFlags[7]) % 8;

            // Set Chaos HP
            int chaosHpIndex = int.Parse(randoFlags[8].ToString());
            if (chaosHpIndex >= 0 && chaosHpIndex < chaosHpTrackBar.Maximum + 1)
            {
                chaosHpTrackBar.Value = chaosHpIndex;
                int[] hpValues = { 1, 9999, 20000, 30000 };
                int selectedHp = hpValues[chaosHpIndex];
                chaosHpLabel.Text = $"Chaos HP: {selectedHp}";
            }

            flagJackInTheBox.Checked = randoFlags[9] == '1';

            // Apply visual flags
            modeShuffleNPCs.SelectedIndex = int.Parse(visualFlags[0].ToString());

            if (visualFlags.Length > 1)
                modeAirshipSprite.SelectedIndex = int.Parse(visualFlags[1].ToString(), System.Globalization.NumberStyles.HexNumber);
            if (visualFlags.Length > 2)
                modeBoatSprite.SelectedIndex = int.Parse(visualFlags[2].ToString(), System.Globalization.NumberStyles.HexNumber);
            if (visualFlags.Length > 3)
                flagShuffleBackgrounds.Checked = visualFlags[3] == '1';
            if (visualFlags.Length > 4)
                flagShowBossNames.Checked = visualFlags[4] == '1';

            loading = false;
        }

        public void GenerateNewSeed(TextBox seedTextBox)
        {
            ConfigurationManager.GenerateNewSeed(config);
            seedTextBox.Text = config.Seed;
        }

        public void LoadFlags(string inputFlags, TextBox randoFlags, TextBox visualFlags)
        {
            if (string.IsNullOrEmpty(inputFlags)) return;

            if (inputFlags.Length >= 10)
            {
                randoFlags.Text = inputFlags.Substring(0, 10);
                config.RandoFlags = randoFlags.Text;
            }

            if (inputFlags.Length > 10)
            {
                visualFlags.Text = inputFlags.Substring(10);
                config.VisualFlags = visualFlags.Text;
            }
        }

        public void SaveConfiguration(string ff1prFolder, string seed, string randoFlags, string visualFlags, 
            List<string> spriteSelections)
        {
            config.FF1PRFolder = ff1prFolder;
            config.Seed = seed;
            config.RandoFlags = randoFlags;
            config.VisualFlags = visualFlags;
            config.SpriteSelections = spriteSelections;

            try
            {
                using (var writer = File.CreateText("lastFF1PRR.txt"))
                {
                    writer.WriteLine(config.FF1PRFolder);
                    writer.WriteLine(config.Seed);
                    writer.WriteLine(config.RandoFlags);
                    writer.WriteLine(config.VisualFlags);
                    writer.WriteLine(DateTime.Now.ToString());
                    
                    foreach (string selection in config.SpriteSelections)
                    {
                        writer.WriteLine(selection);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration: {ex.Message}");
            }
        }

        public ConfigurationManager.RandomizerConfig GetCurrentConfig()
        {
            return config;
        }

        public void SetLoadingState(bool isLoading)
        {
            loading = isLoading;
        }

        private void LoadConfiguration()
        {
            try
            {
                if (File.Exists("lastFF1PRR.txt"))
                {
                    using (var reader = File.OpenText("lastFF1PRR.txt"))
                    {
                        config.FF1PRFolder = reader.ReadLine() ?? "";
                        config.Seed = reader.ReadLine() ?? ConfigurationManager.GetDefaultConfig().Seed;
                        config.RandoFlags = reader.ReadLine() ?? ConfigurationManager.GetDefaultConfig().RandoFlags;
                        config.VisualFlags = reader.ReadLine() ?? ConfigurationManager.GetDefaultConfig().VisualFlags;
                        
                        // Skip the date line
                        reader.ReadLine();
                        
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            config.SpriteSelections.Add(line);
                        }
                    }
                }
            }
            catch
            {
                config = ConfigurationManager.GetDefaultConfig();
            }
        }

        private void PopulateDropdowns()
        {
            // This would populate airship and boat sprite dropdowns
            // Implementation would be moved from Form1.cs
        }

        private void SetupEventHandlers()
        {
            // This would set up event handlers for UI controls
            // Implementation would be moved from Form1.cs
        }
    }
}