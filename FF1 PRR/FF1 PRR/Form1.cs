using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using CsvHelper;
using FF1_PRR.Inventory;
using FF1_PRR.Randomize;

namespace FF1_PRR
{
	public partial class FF1PRR : Form
	{
		bool loading = true;
		Random r1;
		DateTime lastGameAssets;
		const string defaultVisualFlags = "0";
		const string defaultFlags = "huCP900";

		public FF1PRR()
		{
			InitializeComponent();
			PopulateSpriteSelection();
		}

		private void restoreVanilla()
		{
			string vanillaPath = Path.Combine("vanilla", "FF1PRR");
			string modsPath = Path.Combine("mods", "FF1PRR");
			string targetPath = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR");

			// Clear the mods directory
			if (Directory.Exists(modsPath))
			{
				Directory.Delete(modsPath, true);
			}

			// Clear the target directory
			if (Directory.Exists(targetPath))
			{
				Directory.Delete(targetPath, true);
			}

			// Copy all files from vanilla to the mods directory
			CopyDirectory(vanillaPath, modsPath);

			// Copy all files from vanilla to the target directory
			CopyDirectory(vanillaPath, targetPath);
		}

		private void applyMods()
		{
			string modsPath = Path.Combine("mods", "FF1PRR");
			string targetPath = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR");

			// Clear the target directory
			if (Directory.Exists(targetPath))
			{
				Directory.Delete(targetPath, true);
			}

			// Copy all modded files to the game's directory
			CopyDirectory(modsPath, targetPath);
		}

		private void btnRestoreVanilla_Click(object sender, EventArgs e)
		{
			restoreVanilla();

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

		private void btnRandomize_Click(object sender, EventArgs e)
		{
			NewChecksum.Text = "Please wait...";
			this.Refresh();

			Updater.update(FF1PRFolder.Text);
			restoreVanilla();

			ApplySelectedSprites();

			// Apply database edits
			doDatabaseEdits("mods/FF1PRR");

			// Modify system_en.txt in the mods directory
			string seedNumber = RandoSeed.Text;
			ModifySystemMessage(seedNumber, Path.Combine("mods", "FF1PRR", "messages", "system_en.txt"));

			// Begin randomization on the mods folder
			r1 = new Random(Convert.ToInt32(RandoSeed.Text));
			if (modeMagic.SelectedIndex > 0) randomizeMagic("mods/FF1PRR");
			if (modeShops.SelectedIndex > 0) randomizeShops("mods/FF1PRR");
			if (flagKeyItems.Checked) randomizeKeyItems("mods/FF1PRR");
			if (modeTreasure.SelectedIndex > 0) randomizeTreasure("mods/FF1PRR");
			if (flagNoEscapeNES.Checked) noEscapeAdjustment("mods/FF1PRR");
			if (flagHeroStatsStandardize.Checked || modeHeroStats.SelectedIndex > 0) randomizeHeroStats("mods/FF1PRR");
			monsterBoost("mods/FF1PRR");

			applyMods(); // Copy modified files to the game's directory

			NewChecksum.Text = "COMPLETE";
		}

		// Helper method to copy directories
		private void CopyDirectory(string sourceDir, string destDir)
		{
			Directory.CreateDirectory(destDir);

			foreach (var file in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
			{
				string relativePath = file.Substring(sourceDir.Length + 1); // +1 to remove the leading directory separator
				string destPath = Path.Combine(destDir, relativePath);
				Directory.CreateDirectory(Path.GetDirectoryName(destPath));
				File.Copy(file, destPath, true);
			}
		}

		// This Modifies the main menu and the new game menu to show the seed number.
		private void ModifySystemMessage(string seedNumber, string systemMessagePath)
		{
			if (!File.Exists(systemMessagePath))
			{
				string errorMessage = $"The file {systemMessagePath} was not found.";
				throw new FileNotFoundException(errorMessage);
			}

			var lines = File.ReadAllLines(systemMessagePath).ToList();
			bool message183Found = false;
			bool message181Found = false;

			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i].StartsWith("MSG_SYSTEM_183\t"))
				{
					lines[i] = $"MSG_SYSTEM_183\tRandomizer Seed {seedNumber}";
					message183Found = true;
				}
				else if (lines[i].StartsWith("MSG_SYSTEM_181\t"))
				{
					lines[i] += $@"\nRandomizer Seed: {seedNumber}";
					message181Found = true;
				}
			}

			if (!message183Found)
			{
				string errorMessage = "MSG_SYSTEM_183 not found in system_en.txt.";
				throw new Exception(errorMessage);
			}

			if (!message181Found)
			{
				string errorMessage = "MSG_SYSTEM_181 not found in system_en.txt.";
				throw new Exception(errorMessage);
			}

			File.WriteAllLines(systemMessagePath, lines);
		}

		private void ApplySelectedSprites()
		{
			foreach (var item in currentSelectionsListBox.Items)
			{
				try
				{
					string[] parts = item.ToString().Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						string characterPart = parts[0];
						string spritePart = parts[1];

						bool includeJobUpgrade = characterPart.StartsWith("(+JU)");
						string character = includeJobUpgrade ? characterPart.Substring(6) : characterPart;

						string baseFolder = Path.Combine("sprites");
						SpriteUpdater.ReplaceSprite(baseFolder, character, spritePart, FF1PRFolder.Text, includeJobUpgrade);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error processing item '{item}': {ex.Message}");
				}
			}
		}

		private void doDatabaseEdits(string baseDir)
		{
			List<DatabaseEdit> editsToMake = new List<DatabaseEdit>();
			string dataPath = Path.Combine(baseDir, "master");
			string modsPath = Path.Combine("csvmods");
			if (flagRebalancePrices.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[1]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataRebalancePrices.csv")));
			}
			if (flagFiendsDropRibbons.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[2]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataFiendsDropRibbons.csv")));
			}
			if (flagRebalanceBosses.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[4]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataRebalanceBosses.csv")));
			}
			if (flagRestoreCritRating.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[8]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataRestoreCritRating.csv")));
			}
			if (flagWandsAddInt.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[16]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataWandsAddInt.csv")));
			}
			if (flagReduceEncounterRate.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[32]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataReduceEncounterRate.csv")));
			}
			if (flagReduceChaosHP.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[64]);
				editsToMake.AddRange(addEdits(Path.Combine(modsPath, "dataReduceChaosHP.csv")));
			}

			// Now apply the edits
			foreach (var editsByFile in editsToMake.GroupBy(x => x.file))
			{
				List<dynamic> fileToEdit;
				using (StreamReader reader = new StreamReader(Path.Combine(dataPath, editsByFile.Key)))
				using (CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
				{
					fileToEdit = csv.GetRecords<dynamic>().ToList();
					foreach (var edit in editsByFile)
					{
						var itemDict = fileToEdit.Find(x => x.id == edit.id) as IDictionary<string, object>;
						itemDict[edit.field] = edit.value;
					}
				}
				using (StreamWriter writer = new StreamWriter(Path.Combine(dataPath, editsByFile.Key)))
				using (CsvWriter csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
				{
					csv.WriteRecords(fileToEdit);
				}
			}
		}

		private List<DatabaseEdit> addEdits(string filename)
		{
			List<DatabaseEdit> edits;
			using (StreamReader reader = new StreamReader(filename))
			using (CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
			{
				edits = csv.GetRecords<DatabaseEdit>().ToList();
			}
			return edits;
		}

		private void randomizeShops(string baseDir)
		{
			Shops randoShops = new Shops(r1, modeShops.SelectedIndex,
					Path.Combine(baseDir, "master", "product.csv"),
					flagShopsTrad.Checked);
		}

		private void randomizeMagic(string baseDir)
		{
			Magic magicData = new Magic(
					r1,
					modeMagic.SelectedIndex,
					Path.Combine(baseDir, "master", "ability.csv"),
					Path.Combine(baseDir, "master", "product.csv"),
					flagMagicShuffleShops.Checked,
					flagMagicKeepPermissions.Checked
			);
		}

		private void randomizeKeyItems(string baseDir)
		{
			KeyItems randoKeyItems = new KeyItems(r1, baseDir);
		}

		private void randomizeTreasure(string baseDir)
		{
			Treasure randoChests = new Treasure(r1, modeTreasure.SelectedIndex, baseDir, flagTreasureTrad.Checked, flagFiendsDropRibbons.Checked);
		}

		private void randomizeHeroStats(string baseDir)
		{
			Stats.RandomizeStats(modeHeroStats.SelectedIndex, flagHeroStatsStandardize.Checked, flagBoostPromoted.Checked, r1, Path.Combine(baseDir, "master", "character_status.csv"),
					Path.Combine(baseDir, "messages", "system_en.txt"));
		}

		private void monsterBoost(string baseDir)
		{
			double xp = modeXPBoost.SelectedIndex == 0 ? 0.5 :
					modeXPBoost.SelectedIndex == 1 ? 1.0 :
					modeXPBoost.SelectedIndex == 2 ? 1.5 :
					modeXPBoost.SelectedIndex == 3 ? 2.0 :
					modeXPBoost.SelectedIndex == 4 ? 3.0 :
					modeXPBoost.SelectedIndex == 5 ? 4.0 :
					modeXPBoost.SelectedIndex == 6 ? 5.0 : 10;

			double minStatAdjustment = modeMonsterStatAdjustment.SelectedIndex == 1 ? 0.6666667 :
					modeMonsterStatAdjustment.SelectedIndex == 2 ? 0.5 :
					modeMonsterStatAdjustment.SelectedIndex == 3 ? 0.3333333 :
					modeMonsterStatAdjustment.SelectedIndex == 4 ? 0.25 :
					modeMonsterStatAdjustment.SelectedIndex == 5 ? 0.2 : 1;

			double maxStatAdjustment = modeMonsterStatAdjustment.SelectedIndex == 6 ? 1.25 :
					modeMonsterStatAdjustment.SelectedIndex == 1 || modeMonsterStatAdjustment.SelectedIndex == 7 ? 1.5 :
					modeMonsterStatAdjustment.SelectedIndex == 2 || modeMonsterStatAdjustment.SelectedIndex == 8 ? 2 :
					modeMonsterStatAdjustment.SelectedIndex == 3 || modeMonsterStatAdjustment.SelectedIndex == 9 ? 3 :
					modeMonsterStatAdjustment.SelectedIndex == 4 || modeMonsterStatAdjustment.SelectedIndex == 10 ? 4 :
					modeMonsterStatAdjustment.SelectedIndex == 5 || modeMonsterStatAdjustment.SelectedIndex == 11 ? 5 : 1;

			string monsterFile = Path.Combine(baseDir, "master", "monster.csv");

			Monster monsters = new Monster(r1, monsterFile, xp, 0, xp, 0, minStatAdjustment, maxStatAdjustment);
		}

		private void noEscapeAdjustment(string baseDir)
		{
			MonsterParty.mandatoryRandomEncounters(r1, Path.Combine(baseDir, "master"), flagNoEscapeRandomize.Checked);
		}

		private void PopulateSpriteSelection()
		{
			string spriteDirectory = Path.Combine("sprites");
			if (Directory.Exists(spriteDirectory))
			{
				var spriteFolders = Directory.GetDirectories(spriteDirectory);
				foreach (var folder in spriteFolders)
				{
					string folderName = Path.GetFileName(folder);
					spriteSelection.Items.Add(folderName);
				}
			}
			else
			{
				MessageBox.Show("Sprite directory not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


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

		private void LoadPresetButton_Click(object sender, EventArgs e)
		{
			string selectedPreset = presetDropdown.SelectedItem.ToString();
			string filePath = "lastFF1PRR.txt";

			if (File.Exists(filePath))
			{
				var lines = File.ReadAllLines(filePath).ToList();
				int presetIndex = lines.IndexOf("[Presets]");
				if (presetIndex >= 0)
				{
					int startIndex = lines.IndexOf($"[{selectedPreset}]", presetIndex);
					if (startIndex >= 0)
					{
						var presetLines = lines.Skip(startIndex + 1).TakeWhile(line => !line.StartsWith("[") || line == "[Presets]").ToList();
						if (presetLines.Count > 0)
						{
							RandoFlags.Text = presetLines.FirstOrDefault(line => line.StartsWith("RandoFlags="))?.Split('=')[1] ?? defaultFlags;
							// Load other settings similarly...
						}
					}
				}
			}
		}

		private void SavePresetButton_Click(object sender, EventArgs e)
		{
			string newPresetName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the new preset:", "Save Preset", "");
			if (string.IsNullOrWhiteSpace(newPresetName)) return;

			string filePath = "lastFF1PRR.txt";
			var lines = File.Exists(filePath) ? File.ReadAllLines(filePath).ToList() : new List<string>();

			int presetIndex = lines.IndexOf("[Presets]");
			if (presetIndex == -1)
			{
				lines.Add("[Presets]");
				presetIndex = lines.Count - 1;
			}

			int existingPresetIndex = lines.IndexOf($"[{newPresetName}]", presetIndex);
			if (existingPresetIndex >= 0)
			{
				lines.RemoveAt(existingPresetIndex); // Remove old preset
			}

			lines.InsertRange(presetIndex + 1, new List<string>
		{
				$"[{newPresetName}]",
				$"RandoFlags={RandoFlags.Text}",
        // Save other settings similarly...
    });

			File.WriteAllLines(filePath, lines);
			presetDropdown.Items.Add(newPresetName); // Add to dropdown if not already present
		}

		private void SavePresetButton_Click(object sender, EventArgs e)
		{
			string newPresetName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the new preset:", "Save Preset", "");
			if (string.IsNullOrWhiteSpace(newPresetName)) return;

			string filePath = "lastFF1PRR.txt";
			var lines = File.Exists(filePath) ? File.ReadAllLines(filePath).ToList() : new List<string>();

			int presetIndex = lines.IndexOf("[Presets]");
			if (presetIndex == -1)
			{
				lines.Add("[Presets]");
				presetIndex = lines.Count - 1;
			}

			int existingPresetIndex = lines.IndexOf($"[{newPresetName}]", presetIndex);
			if (existingPresetIndex >= 0)
			{
				lines.RemoveAt(existingPresetIndex); // Remove old preset
			}

			lines.InsertRange(presetIndex + 1, new List<string>
		{
				$"[{newPresetName}]",
				$"RandoFlags={RandoFlags.Text}",
        // Save other settings similarly...
    });

			File.WriteAllLines(filePath, lines);
			presetDropdown.Items.Add(newPresetName); // Add to dropdown if not already present
		}


		private void frmFF1PRR_FormClosing(object sender, FormClosingEventArgs e)
		{
			using (StreamWriter writer = File.CreateText("lastFF1PRR.txt"))
			{
				writer.WriteLine(FF1PRFolder.Text);
				writer.WriteLine(RandoSeed.Text);
				writer.WriteLine(RandoFlags.Text);
				writer.WriteLine(VisualFlags.Text);
				writer.WriteLine(lastGameAssets);
				writer.WriteLine($"LastPreset={presetDropdown.SelectedItem?.ToString()}");
				foreach (var item in currentSelectionsListBox.Items)
				{
					writer.WriteLine(item.ToString());
				}
				// Append any existing presets at the end
				writer.WriteLine("[Presets]");
				// Add code to write existing presets if not already saved
			}
		}


		private void btnBrowse_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();

				if (result == OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
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
	}

	public class DatabaseEdit
	{
		public string file { get; set; }
		public string name { get; set; }
		public string id { get; set; }
		public string field { get; set; }
		public string value { get; set; }
		public string comment { get; set; }

		public int CompareTo(DatabaseEdit edit)
		{
			// A null value means that this object is greater.
			if (edit == null)
				return 1;

			else
				return this.file.CompareTo(edit.file);
		}
	}
}
