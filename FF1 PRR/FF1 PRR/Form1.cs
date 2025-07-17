using FF1_PRR.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FF1_PRR
{
	public partial class FF1PRR : Form
	{
		private UIController uiController;
		private RandomizerEngine randomizerEngine;
		private DateTime lastGameAssets;
		private bool loading = true;

		public FF1PRR()
		{
			InitializeComponent();
			uiController = new UIController(this);
			PopulateSpriteSelection();
		}

		public class NearestNeighborPictureBox : PictureBox
		{
			protected override void OnPaint(PaintEventArgs pe)
			{
				if (Image != null)
				{
					pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

					// Calculate aspect ratio
					float imageAspectRatio = (float)Image.Width / Image.Height;
					int drawHeight = Height;
					int drawWidth = (int)(Height * imageAspectRatio);

					// Calculate drawing rectangle to center horizontally
					Rectangle drawRect = new Rectangle((Width - drawWidth) / 2, 0, drawWidth, drawHeight);

					pe.Graphics.DrawImage(Image, drawRect);
				}
				else
				{
					base.OnPaint(pe);
				}
			}
		}



		public void DetermineFlags(object sender, EventArgs e)
		{
			flagNoEscapeRandomize.Enabled = flagNoEscapeNES.Checked;
			chaosHpTrackBar.Enabled = flagReduceChaosHP.Checked;
			flagIncludeAllBosses.Enabled = flagBossShuffle.Checked;

			uiController.UpdateFlags(
				new CheckBox[] { flagBossShuffle, flagKeyItems, flagShopsTrad, flagMagicShuffleShops, flagMagicKeepPermissions, flagMagicRandomizeClassPermissions, flagReduceEncounterRate },
				new CheckBox[] { flagTreasureTrad, flagRebalanceBosses, flagFiendsDropRibbons, flagRebalancePrices, flagRestoreCritRating, flagWandsAddInt },
				new CheckBox[] { flagNoEscapeNES, flagNoEscapeRandomize, flagReduceChaosHP, flagHeroStatsStandardize, flagBoostPromoted, flagSecretChaos },
				new CheckBox[] { flagDockAnywhere, flagShuffleCanoe, flagIncludeAllBosses, flagShuffleMonsterEncounters },
				modeShops, modeXPBoost, modeTreasure, modeMagic, modeMonsterStatAdjustment, modeHeroStats,
				chaosHpTrackBar, flagJackInTheBox, RandoFlags);

			uiController.UpdateVisualFlags(modeShuffleNPCs, modeAirshipSprite, modeBoatSprite, flagShuffleBackgrounds, VisualFlags);
		}

		private void determineChecks(object sender, EventArgs e)
		{
			var config = uiController.GetCurrentConfig();
			
			if (RandoFlags.Text.Length < 10)
				RandoFlags.Text = config.RandoFlags;
			if (VisualFlags.Text.Length < 4)
				VisualFlags.Text = config.VisualFlags;

			uiController.ApplyFlags(RandoFlags.Text, VisualFlags.Text,
				new CheckBox[] { flagBossShuffle, flagKeyItems, flagShopsTrad, flagMagicShuffleShops, flagMagicKeepPermissions, flagMagicRandomizeClassPermissions, flagReduceEncounterRate },
				new CheckBox[] { flagTreasureTrad, flagRebalanceBosses, flagFiendsDropRibbons, flagRebalancePrices, flagRestoreCritRating, flagWandsAddInt },
				new CheckBox[] { flagNoEscapeNES, flagNoEscapeRandomize, flagReduceChaosHP, flagHeroStatsStandardize, flagBoostPromoted, flagSecretChaos },
				new CheckBox[] { flagDockAnywhere, flagShuffleCanoe, flagIncludeAllBosses, flagShuffleMonsterEncounters },
				modeShops, modeXPBoost, modeTreasure, modeMagic, modeMonsterStatAdjustment, modeHeroStats,
				chaosHpTrackBar, flagJackInTheBox, chaosHpLabel,
				modeShuffleNPCs, modeAirshipSprite, modeBoatSprite, flagShuffleBackgrounds);

			flagNoEscapeRandomize.Enabled = flagNoEscapeNES.Checked;
			chaosHpTrackBar.Enabled = flagReduceChaosHP.Checked;
			flagIncludeAllBosses.Enabled = flagBossShuffle.Checked;
		}

		// These methods are now handled by ConfigurationManager

		private void btnLoadFlags_Click(object sender, EventArgs e)
		{
			uiController.LoadFlags(RandoFlags.Text, RandoFlags, VisualFlags);
			determineChecks(null, null);
		}


		private void FF1PRR_Load(object sender, EventArgs e)
		{
			RandoSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
			spriteSelection.SelectedIndex = 0;
			characterSelection.SelectedIndex = 0;

			string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
			string airshipSpritesPath = Path.Combine(baseFolder, "data", "mods", "Airships");
			var airshipSprites = Directory.GetDirectories(airshipSpritesPath).Select(Path.GetFileName).ToList();
			airshipSprites.Insert(0, "None"); // Add a "None" option at the beginning
			modeAirshipSprite.DataSource = airshipSprites;

			string boatSpritesPath = Path.Combine(baseFolder, "data", "mods", "Boats");
			var boatSprites = Directory.GetDirectories(boatSpritesPath).Select(Path.GetFileName).ToList();
			boatSprites.Insert(0, "None"); // Add a "None" option at the beginning
			modeBoatSprite.DataSource = boatSprites;

			try
			{
				using (TextReader reader = File.OpenText("lastFF1PRR.txt"))
				{
					FF1PRFolder.Text = reader.ReadLine();
					RandoSeed.Text = reader.ReadLine();
					RandoFlags.Text = reader.ReadLine();
					VisualFlags.Text = reader.ReadLine();
					lastGameAssets = DateTime.Parse(reader.ReadLine());
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						currentSelectionsListBox.Items.Add(line);
					}

					determineChecks(null, null);

					loading = false;
				}
			}
			catch
			{
				var defaultConfig = ConfigurationManager.GetDefaultConfig();
				RandoFlags.Text = defaultConfig.RandoFlags;
				VisualFlags.Text = defaultConfig.VisualFlags;
				loading = false;
				determineChecks(null, null);
			}
		}


		private void NewSeed_Click(object sender, EventArgs e)
		{
			uiController.GenerateNewSeed(RandoSeed);
		}

		private void btnRestoreVanilla_Click(object sender, EventArgs e)
		{
			try
			{
				var fileManager = new FileManager(FF1PRFolder.Text);
				fileManager.RestoreVanillaFiles();
				MessageBox.Show("Restoration to vanilla complete!");
				currentSelectionsListBox.Items.Clear();

				// Clear the list of sprites in the lastFF1PRR.txt file
				using (StreamWriter writer = File.CreateText("lastFF1PRR.txt"))
				{
					writer.WriteLine(FF1PRFolder.Text);
					writer.WriteLine(RandoSeed.Text);
					writer.WriteLine(RandoFlags.Text);
					writer.WriteLine(VisualFlags.Text);
					writer.WriteLine(""); // Clear the lastGameAssets content
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error restoring vanilla files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnRandomize_Click(object sender, EventArgs e)
		{
			try
			{
				NewChecksum.Text = "Please wait...";
				this.Refresh();

				// Create configuration
				var config = uiController.GetCurrentConfig();
				config.FF1PRFolder = FF1PRFolder.Text;
				config.Seed = RandoSeed.Text;

				// Initialize randomizer engine
				randomizerEngine = new RandomizerEngine(config);

				// Create randomization options from UI state
				var options = CreateRandomizationOptions();

				// Apply selected sprites
				var fileManager = new FileManager(FF1PRFolder.Text);
				var spriteSelections = currentSelectionsListBox.Items.Cast<string>().ToList();
				string airshipSprite = modeAirshipSprite.SelectedItem?.ToString() ?? "None";
				string boatSprite = modeBoatSprite.SelectedItem?.ToString() ?? "None";
				fileManager.ApplySelectedSprites(spriteSelections, airshipSprite, boatSprite);

				// Execute randomization
				randomizerEngine.ExecuteRandomization(options);

				NewChecksum.Text = "COMPLETE";
			}
			catch (Exception ex)
			{
				NewChecksum.Text = "ERROR";
				MessageBox.Show($"Randomization failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private RandomizerEngine.RandomizationOptions CreateRandomizationOptions()
		{
			return new RandomizerEngine.RandomizationOptions
			{
				KeyItems = flagKeyItems.Checked,
				ShopsTrad = flagShopsTrad.Checked,
				MagicShuffleShops = flagMagicShuffleShops.Checked,
				MagicKeepPermissions = flagMagicKeepPermissions.Checked,
				MagicRandomizeClassPermissions = flagMagicRandomizeClassPermissions.Checked,
				ReduceEncounterRate = flagReduceEncounterRate.Checked,
				TreasureTrad = flagTreasureTrad.Checked,
				RebalanceBosses = flagRebalanceBosses.Checked,
				FiendsDropRibbons = flagFiendsDropRibbons.Checked,
				RebalancePrices = flagRebalancePrices.Checked,
				RestoreCritRating = flagRestoreCritRating.Checked,
				WandsAddInt = flagWandsAddInt.Checked,
				NoEscapeNES = flagNoEscapeNES.Checked,
				NoEscapeRandomize = flagNoEscapeRandomize.Checked,
				ReduceChaosHP = flagReduceChaosHP.Checked,
				HeroStatsStandardize = flagHeroStatsStandardize.Checked,
				BoostPromoted = flagBoostPromoted.Checked,
				SecretChaos = flagSecretChaos.Checked,
				DockAnywhere = flagDockAnywhere.Checked,
				ShuffleCanoe = flagShuffleCanoe.Checked,
				IncludeAllBosses = flagIncludeAllBosses.Checked,
				JackInTheBox = flagJackInTheBox.Checked,
				BossShuffle = flagBossShuffle.Checked,
				ShuffleBackgrounds = flagShuffleBackgrounds.Checked,
				ShuffleMonsterEncounters = flagShuffleMonsterEncounters.Checked,
				ModeShops = modeShops.SelectedIndex,
				ModeXPBoost = modeXPBoost.SelectedIndex,
				ModeTreasure = modeTreasure.SelectedIndex,
				ModeMagic = modeMagic.SelectedIndex,
				ModeMonsterStatAdjustment = modeMonsterStatAdjustment.SelectedIndex,
				ModeHeroStats = modeHeroStats.SelectedIndex,
				ChaosHpValue = chaosHpTrackBar.Value,
				ModeShuffleNPCs = modeShuffleNPCs.SelectedIndex,
				ModeAirshipSprite = modeAirshipSprite.SelectedIndex,
				ModeBoatSprite = modeBoatSprite.SelectedIndex
			};
		}


		private void PopulateSpriteSelection()
		{
			string spriteDirectory = Path.Combine(Application.StartupPath, "data", "mods", "sprites");
			if (Directory.Exists(spriteDirectory))
			{
				var spriteFolders = Directory.GetDirectories(spriteDirectory);
				var sortedFolders = spriteFolders
						.Select(folder => new
						{
							FullPath = folder,
							FolderName = Path.GetFileName(folder),
							Game = Path.GetFileName(folder).Split('-').Last().Trim()
						})
						.OrderBy(folder => folder.Game) // Sort by the game part
						.ToList();

				spriteSelection.Items.Clear();
				foreach (var folder in sortedFolders)
				{
					spriteSelection.Items.Add(folder.FolderName);
				}
			}
			else
			{
				MessageBox.Show("Sprite directory not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void SpriteSelection_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Get the selected sprite name
			string selectedSprite = spriteSelection.SelectedItem.ToString();

			// Build the path to the sprite image directory
			string spriteDirectory = Path.Combine(Application.StartupPath, "data", "mods", "sprites", selectedSprite);

			// Find the PNG file in the nested directories
			string[] imageFiles = Directory.GetFiles(spriteDirectory, "*.png", SearchOption.AllDirectories);

			if (imageFiles.Length > 0)
			{
				// Load the first PNG file into the PictureBox
				pictureBoxSprite.Image = Image.FromFile(imageFiles[0]);
			}
			else
			{
				pictureBoxSprite.Image = null; // Clear the PictureBox if no image is found
				MessageBox.Show($"No PNG file found in the directory for {selectedSprite}.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		// Event handler for Apply button click
		private void ApplyButton_Click(object sender, EventArgs e)
		{
			string character = characterSelection.SelectedItem.ToString();
			string sprite = spriteSelection.SelectedItem.ToString();
			bool includeJobUpgrade = includeJobUpgradeCheckBox.Checked;
			string newEntry = $"{(includeJobUpgrade ? "(+JU) " : "")}{character}: {sprite}";

			// Check if the character is already in the list
			bool characterExists = false;
			for (int i = 0; i < currentSelectionsListBox.Items.Count; i++)
			{
				string existingEntry = currentSelectionsListBox.Items[i].ToString();
				// Extract the character part from the existing entry using a regular expression
				string pattern = @"(\(\+JU\)\s)?(.+):";
				var match = Regex.Match(existingEntry, pattern);
				if (match.Success)
				{
					string existingCharacter = match.Groups[2].Value.Trim();
					if (existingCharacter == character)
					{
						// Update the existing entry
						currentSelectionsListBox.Items[i] = newEntry;
						characterExists = true;
						break;
					}
				}
			}

			// If character does not exist in the list, add the new entry
			if (!characterExists)
			{
				currentSelectionsListBox.Items.Add(newEntry);
			}
		}

		private void ChaosHpTrackBar_Scroll(object sender, EventArgs e)
		{
			int[] hpValues = { 1, 9999, 20000, 30000 };
			int selectedIndex = chaosHpTrackBar.Value;
			int selectedHp = hpValues[selectedIndex];
			chaosHpLabel.Text = $"Chaos HP: {selectedHp}";

			DetermineFlags(sender, e);
		}

		private void frmFF1PRR_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				using (StreamWriter writer = File.CreateText("lastFF1PRR.txt"))
				{
					writer.WriteLine(FF1PRFolder.Text);
					writer.WriteLine(RandoSeed.Text);
					writer.WriteLine(RandoFlags.Text);
					writer.WriteLine(VisualFlags.Text);
					writer.WriteLine(lastGameAssets.ToString());
					foreach (var item in currentSelectionsListBox.Items)
					{
						writer.WriteLine(item.ToString());
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					FF1PRFolder.Text = fbd.SelectedPath;
			}
		}

		private void statExplanation_Click(object sender, EventArgs e)
		{
			MessageBox.Show("None - Keep stats at vanilla\r\n" +
																			"Shuffle - Each character will get the same stats at level up, but when they earn them are shuffled around\r\n" +
																			"Standard - Each character will get a percentage chance of a strong level up consistent to their class\r\n" +
																			"Silly - Stat growth is randomized, but will be approx. similar to the stat gains in the base game\r\n" +
																			"Wild - Randomized stat growth.  Similar to base game stats, but can vary wildly\r\n" +
																			"Chaos - Randomized stat growth.  Characters can have any stat gain\r\n" +
																			"Standardized - For None, Shuffle, and Standard, make stat gains consistent for each play for the seed.  Great for races!\r\n" +
																			"Boost promoted classes - 25% chance of higher stats on shuffle and standard, 25%, +/-, higher stats on silly, wild, and chaos for promoted classes\r\n" +
																			"NOTE:  Accuracy and magic defense is only randomized in silly, wild, or chaos settings");
		}

		private void RandoSeed_TextChanged(object sender, EventArgs e)
		{
			// Event handler for when the randomization seed text changes
			// This can be used to validate the seed or trigger other actions
		}

		private void currentSelectionsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Event handler for when the selection in the current selections list box changes
			// This can be used to update UI or perform other actions based on the selection
		}

		private void groupBox9_Enter(object sender, EventArgs e)
		{
			// Event handler for when the group box is entered
			// This is typically used for focus-related actions
		}
	}
}