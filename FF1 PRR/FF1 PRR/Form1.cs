using FF1_PRR.Randomize;
using FF1_PRR.Inventory;
using Newtonsoft.Json;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace FF1_PRR
{
	public partial class FF1PRR : Form
	{
		bool loading = true;
		Random r1;
		DateTime lastGameAssets;
		const string defaultVisualFlags = "0";
		const string defaultFlags = "huCP90010";
		public FF1PRR()
		{
			InitializeComponent();
			PopulateSpriteSelection();
		}

		public void DetermineFlags(object sender, EventArgs e)
		{
			if (loading) return;

			flagNoEscapeRandomize.Enabled = flagNoEscapeNES.Checked;

			string flags = "";
			flags += convertIntToChar(checkboxesToNumber(new CheckBox[] { flagBossShuffle, flagKeyItems, flagShopsTrad, flagMagicShuffleShops, flagMagicKeepPermissions, flagReduceEncounterRate }));
			flags += convertIntToChar(checkboxesToNumber(new CheckBox[] { flagTreasureTrad, flagRebalanceBosses, flagFiendsDropRibbons, flagRebalancePrices, flagRestoreCritRating, flagWandsAddInt }));
			flags += convertIntToChar(checkboxesToNumber(new CheckBox[] { flagNoEscapeNES, flagNoEscapeRandomize, flagReduceChaosHP, flagHeroStatsStandardize, flagBoostPromoted, flagSecretChaos }));
			flags += convertIntToChar(modeShops.SelectedIndex + (8 * modeXPBoost.SelectedIndex));
			flags += convertIntToChar(modeTreasure.SelectedIndex + (8 * modeMagic.SelectedIndex));
			flags += convertIntToChar(modeMonsterStatAdjustment.SelectedIndex + (16 * 0));
			flags += convertIntToChar(modeHeroStats.SelectedIndex);

			// Add Chaos HP index to the flag string
			flags += chaosHpTrackBar.Value.ToString();

			// Add Jack in the Box flag to the flag string
			flags += flagJackInTheBox.Checked ? "1" : "0";

			// Add new settings for Shuffle NPCs
			flags += modeShuffleNPCs.SelectedIndex.ToString();

			RandoFlags.Text = flags;

			flags = "";
			flags += convertIntToChar(checkboxesToNumber(new CheckBox[] { CuteHats }));
			VisualFlags.Text = flags;

			// Handle the case where both Jack in the Box and Secret Chaos are selected
			if (flagJackInTheBox.Checked && flagSecretChaos.Checked)
			{
				Random random = new Random(Convert.ToInt32(RandoSeed.Text));
				if (random.Next(2) == 0)
				{
					flagJackInTheBox.Checked = false;
				}
				else
				{
					flagSecretChaos.Checked = false;
				}
			}
		}


		private void determineChecks(object sender, EventArgs e)
		{
			if (loading && RandoFlags.Text.Length < 10)  // Adjusted length check to include new settings
				RandoFlags.Text = defaultFlags;
			else if (RandoFlags.Text.Length < 10)
				return;

			if (loading && VisualFlags.Text.Length < 1)
				VisualFlags.Text = defaultVisualFlags;
			else if (VisualFlags.Text.Length < 1)
				return;

			loading = true;

			string flags = RandoFlags.Text;
			numberToCheckboxes(convertChartoInt(Convert.ToChar(flags.Substring(0, 1))), new CheckBox[] { flagBossShuffle, flagKeyItems, flagShopsTrad, flagMagicShuffleShops, flagMagicKeepPermissions, flagReduceEncounterRate });
			numberToCheckboxes(convertChartoInt(Convert.ToChar(flags.Substring(1, 1))), new CheckBox[] { flagTreasureTrad, flagRebalanceBosses, flagFiendsDropRibbons, flagRebalancePrices, flagRestoreCritRating, flagWandsAddInt });
			numberToCheckboxes(convertChartoInt(Convert.ToChar(flags.Substring(2, 1))), new CheckBox[] { flagNoEscapeNES, flagNoEscapeRandomize, flagReduceChaosHP, flagHeroStatsStandardize, flagBoostPromoted, flagSecretChaos });
			modeShops.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(3, 1))) % 8;
			modeXPBoost.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(3, 1))) / 8;
			modeTreasure.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(4, 1))) % 8;
			modeMagic.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(4, 1))) / 8;
			modeMonsterStatAdjustment.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(5, 1))) % 16;
			modeHeroStats.SelectedIndex = convertChartoInt(Convert.ToChar(flags.Substring(6, 1))) % 8;

			// Extract and set the Chaos HP value
			int chaosHpIndex = int.Parse(flags.Substring(7, 1));
			if (chaosHpIndex >= 0 && chaosHpIndex < chaosHpTrackBar.Maximum + 1)
			{
				chaosHpTrackBar.Value = chaosHpIndex;
				int[] hpValues = { 1, 9999, 20000, 30000 };
				int selectedHp = hpValues[chaosHpIndex];
				chaosHpLabel.Text = $"Chaos HP: {selectedHp}";
			}

			// Extract and set the Jack in the Box flag
			flagJackInTheBox.Checked = flags.Substring(8, 1) == "1";

			// Set new settings for Shuffle NPCs
			modeShuffleNPCs.SelectedIndex = int.Parse(flags.Substring(9, 1));

			flags = VisualFlags.Text;
			numberToCheckboxes(convertChartoInt(Convert.ToChar(flags.Substring(0, 1))), new CheckBox[] { CuteHats });

			flagNoEscapeRandomize.Enabled = flagNoEscapeNES.Checked;

			loading = false;
		}


		private int checkboxesToNumber(CheckBox[] boxes)
		{
			int number = 0;
			for (int lnI = 0; lnI < Math.Min(boxes.Length, 6); lnI++)
				number += boxes[lnI].Checked ? (int)Math.Pow(2, lnI) : 0;

			return number;
		}

		private int numberToCheckboxes(int number, CheckBox[] boxes)
		{
			for (int lnI = 0; lnI < Math.Min(boxes.Length, 6); lnI++)
				boxes[lnI].Checked = number % ((int)Math.Pow(2, lnI + 1)) >= (int)Math.Pow(2, lnI);

			return number;
		}

		private string convertIntToChar(int number)
		{
			if (number >= 0 && number <= 9)
				return number.ToString();
			if (number >= 10 && number <= 35)
				return Convert.ToChar(55 + number).ToString();
			if (number >= 36 && number <= 61)
				return Convert.ToChar(61 + number).ToString();
			if (number == 62) return "!";
			if (number == 63) return "@";
			return "";
		}

		private int convertChartoInt(char character)
		{
			if (character >= Convert.ToChar("0") && character <= Convert.ToChar("9"))
				return character - 48;
			if (character >= Convert.ToChar("A") && character <= Convert.ToChar("Z"))
				return character - 55;
			if (character >= Convert.ToChar("a") && character <= Convert.ToChar("z"))
				return character - 61;
			if (character == Convert.ToChar("!")) return 62;
			if (character == Convert.ToChar("@")) return 63;
			return 0;
		}

		private void FF1PRR_Load(object sender, EventArgs e)
		{
			RandoSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
			spriteSelection.SelectedIndex = 0;
			characterSelection.SelectedIndex = 0;

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

					//runChecksum();
					loading = false;
				}
			}
			catch
			{
				RandoFlags.Text = defaultFlags;
				VisualFlags.Text = defaultVisualFlags;
				// ignore error
				loading = false;
				determineChecks(null, null);
			}

		}

		private void NewSeed_Click(object sender, EventArgs e)
		{
			RandoSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
						"Source directory does not exist or could not be found: "
						+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the destination directory doesn't exist, create it.       
			Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string tempPath = Path.Combine(destDirName, file.Name);
				file.CopyTo(tempPath, true);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string tempPath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
				}
			}
		}

		// This Modifies the main menu and the new game menu to show the seed number.
		private void ModifySystemMessage(string seedNumber, string sourcePath, string destPath)
		{
			string systemMessagePath = Path.Combine(sourcePath, "system_en.txt");
			string modifiedMessagePath = Path.Combine(destPath, "system_en.txt");

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

			File.WriteAllLines(modifiedMessagePath, lines);
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

						string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
						SpriteUpdater.ReplaceSprite(baseFolder, character, spritePart, FF1PRFolder.Text, includeJobUpgrade);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error processing item '{item}': {ex.Message}");
				}
			}
		}



		// This brings us back to vanilla Magicite files.  This is NOT used for uninstallation.
		private void restoreVanilla()
		{
			string[] DATA_MASTER = {
				"ability.csv", // used by Magic randomization
				"product.csv", // used by Shop randomization
				"weapon.csv",  // used by balance flags
				"monster.csv", // used by xp boost & monster flags
				"monster_party.csv", // used by no escape flags
				"item.csv",    // used by price rebalance flag
				"armor.csv",   // used by price rebalance flag
				"foot_information.csv", // used by encounter rate flag
				"map.csv",      // used by encounter rate flag
				"character_status.csv", // used by hero stats flags
				"growth_curve.csv"      // used by hero stats flags
			};
			string[] DATA_MESSAGE =
			{
								"system_en.txt", // used by Key Item randomization
								"story_mes_en.txt" // used by jack in the box
            };

			Directory.CreateDirectory(Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master")); // <-- We'll be creating an Export.json soon
			Directory.CreateDirectory(Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message")); // <-- We'll be creating an Export.json soon

			string DATA_MASTER_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master");
			string DATA_MESSAGE_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message");
			string RES_MAP_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets"); // , "Assets", "GameAssets", "Serial", "Res", "Map"

			foreach (string i in DATA_MASTER)
			{
				string outputPath = Path.Combine(DATA_MASTER_PATH, i);
				string sourcePath = Path.Combine("data", "assets", i);
				File.Copy(sourcePath, outputPath, true);
			}
			foreach (string i in DATA_MESSAGE)
			{
				string outputPath = Path.Combine(DATA_MESSAGE_PATH, i);
				string sourcePath = Path.Combine("data", "assets", i);
				File.Copy(sourcePath, outputPath, true);
			}

			string[] characterFolders = {
														"mo_ff1_p001_c00",
														"mo_ff1_p002_c00",
														"mo_ff1_p003_c00",
														"mo_ff1_p004_c00",
														"mo_ff1_p005_c00",
														"mo_ff1_p006_c00",
														"mo_ff1_p007_c00",
														"mo_ff1_p008_c00",
														"mo_ff1_p009_c00",
														"mo_ff1_p010_c00",
														"mo_ff1_p011_c00",
														"mo_ff1_p012_c00",
										};

			string spritesSourcePath = Path.Combine("data", "sprites");
			string spritesDestBasePath = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR");

			foreach (string characterFolder in characterFolders)
			{
				string sourceSpritePath = Path.Combine(spritesSourcePath, characterFolder);
				string destSpritePath = Path.Combine(spritesDestBasePath, characterFolder);

				if (Directory.Exists(destSpritePath))
				{
					Directory.Delete(destSpritePath, true);
				}

				if (Directory.Exists(sourceSpritePath))
				{
					DirectoryCopy(sourceSpritePath, destSpritePath, true);
				}
			}


			// Iterate through the map directory and copy the files into the other map directory...
			Inventory.Updater.MemoriaToMagiciteCopy(RES_MAP_PATH, Path.Combine("data", "master"), "MainData", "master");
			Inventory.Updater.MemoriaToMagiciteCopy(RES_MAP_PATH, Path.Combine("data", "messages"), "Message", "message");

			// Iterate through the map directory and copy the files into the other map directory...
			foreach (string mapDir in Directory.GetDirectories(Path.Combine("data", "maps")))
			{
				// Copy the package.json file if it exists in the map directory
				string packageJsonPath = Path.Combine(mapDir, "package.json");
				if (File.Exists(packageJsonPath))
				{
					string destPackageJsonPath = Path.Combine(RES_MAP_PATH, "Magicite", "FF1PRR", Path.GetFileName(mapDir), "Assets", "GameAssets", "Serial", "Res", "Map", Path.GetFileName(mapDir), "package.json");
					Directory.CreateDirectory(Path.GetDirectoryName(destPackageJsonPath));
					File.Copy(packageJsonPath, destPackageJsonPath, true);
				}

				// Copy each submap directory
				foreach (string submapDir in Directory.GetDirectories(mapDir))
				{
					string topKey = Path.GetFileName(mapDir);
					string submapName = Path.GetFileName(submapDir);
					RemoveCustomScripts(RES_MAP_PATH, topKey, submapName);
					Inventory.Updater.MemoriaToMagiciteCopy(RES_MAP_PATH, submapDir, "Map", topKey);
				}
			}
		}

		private void RemoveCustomScripts(string resMapPath, string mapDirName, string submapDirName)
		{
			string mapRootPath = Path.Combine(resMapPath, "Magicite", "FF1PRR", mapDirName, "Assets", "GameAssets", "Serial", "Res", "Map", mapDirName);
			string[] customScripts = { "sc_t_0099.json", "sc_t_0099_after.json" };
			string logFilePath = Path.Combine(resMapPath, "log.txt");

			foreach (string script in customScripts)
			{
				string scriptPath = Path.Combine(mapRootPath, submapDirName, script);

				// Log the path being checked to a file
				using (StreamWriter sw = new StreamWriter(logFilePath, true))
				{
					sw.WriteLine($"Checking path: {scriptPath}");
				}

				if (File.Exists(scriptPath))
				{
					File.Delete(scriptPath);
				}
				else
				{
					// Log the missing file to the log file
					using (StreamWriter sw = new StreamWriter(logFilePath, true))
					{
						sw.WriteLine($"File not found: {scriptPath}");
					}
				}
			}
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

			// Copy over modded files
			string DATA_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master");
			string MESSAGE_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message");
			string MAP_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Assets", "GameAssets", "Serial", "Res", "Map");
			string RES_MAP_PATH = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets"); // , "Assets", "GameAssets", "Serial", "Res", "Map"

			File.Copy(Path.Combine("data", "mods", "system_en.txt"), Path.Combine(MESSAGE_PATH, "system_en.txt"), true);
			if (flagShopsTrad.Checked) File.Copy(Path.Combine("data", "mods", "productTraditional.csv"), Path.Combine(DATA_PATH, "product.csv"), true);
			else File.Copy(Path.Combine("data", "mods", "product.csv"), Path.Combine(DATA_PATH, "product.csv"), true);
			if (flagJackInTheBox.Checked) File.Copy(Path.Combine("data", "mods", "script.csv"), Path.Combine(DATA_PATH, "script.csv"), true);
			// Iterate through the map directory and copy the files into the other map directory...			
			if (flagJackInTheBox.Checked) File.Copy(Path.Combine("data", "mods", "story_mes_en.txt"), Path.Combine(MESSAGE_PATH, "story_mes_en.txt"), true);

			foreach (string jsonFile in Directory.GetDirectories(Path.Combine("data", "mods", "maps"), "*.*", SearchOption.AllDirectories))
			{
				if (jsonFile.Count(f => f == '\\') != 3) continue;

				Inventory.Updater.MemoriaToMagiciteCopy(RES_MAP_PATH, jsonFile, "Map", Path.GetFileName(jsonFile));
			}

			// string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
			// string selectedCharacter = characterSelection.SelectedItem.ToString();
			// string selectedSprite = spriteSelection.SelectedItem.ToString();
			// SpriteUpdater.ReplaceSprite(baseFolder, selectedCharacter, selectedSprite, FF1PRFolder.Text);

			// Begin randomization
			r1 = new Random(Convert.ToInt32(RandoSeed.Text));
			doDatabaseEdits();
			if (modeMagic.SelectedIndex > 0) randomizeMagic();
			if (modeShops.SelectedIndex > 0) randomizeShops();
			if (flagKeyItems.Checked) randomizeKeyItems();
			if (modeTreasure.SelectedIndex > 0) randomizeTreasure();
			if (flagNoEscapeNES.Checked) noEscapeAdjustment();
			if (flagHeroStatsStandardize.Checked || modeHeroStats.SelectedIndex > 0) randomizeHeroStats();
			monsterBoost();
			if (CuteHats.Checked)
			{
				// neongrey says: eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
				// Demerine says: eeeeeeeee
			}


			// string dataPath = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets");
			// bool hiddenChaos = false; // Set this based on your testing needs
			// bool shuffleAssetIds = false; // Set this based on your testing needs
			// bool allGarland = false;
			if (modeShuffleNPCs.SelectedIndex > 0 || flagSecretChaos.Checked)
				bool allGarland = false;
			{
				if (modeShuffleNPCs.SelectedIndex == 3) allGarland = true;
				NPCs npcs = new NPCs(r1, RES_MAP_PATH, flagSecretChaos.Checked, modeShuffleNPCs.SelectedIndex > 0, allGarland);
			}
			{
				NPCs npcs = new NPCs(r1, dataPath, hiddenChaos, shuffleAssetIds, allGarland);
				if (flagSecretChaos.Checked) File.Copy(Path.Combine("data", "mods", "script.csv"), Path.Combine(RES_MAP_PATH, "script.csv"), true);
				// Iterate through the map directory and copy the files into the other map directory...			
				if (flagSecretChaos.Checked) File.Copy(Path.Combine("data", "mods", "story_mes_en.txt"), Path.Combine(MESSAGE_PATH, "story_mes_en.txt"), true);

				// Modify the system message
				string seedNumber = RandoSeed.Text;
				ModifySystemMessage(seedNumber, Path.Combine("data", "mods"), MESSAGE_PATH);

				bool oopsAllGarland = false;
				string mapObjectFile = Path.Combine("data", "master", "mapobject.csv");
				new NPCAssets(r1, mapObjectFile, DATA_PATH, oopsAllGarland);

				NewChecksum.Text = "COMPLETE";
			}
		private class DatabaseEdit
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
		private List<DatabaseEdit> addEdits(string filename)
		{
			List<DatabaseEdit> edits;
			using (StreamReader reader = new StreamReader(Path.Combine("data", filename)))
			using (CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
			{
				edits = csv.GetRecords<DatabaseEdit>().ToList();
			}
			return edits;
		}
		private void doDatabaseEdits()
		{
			UpdateChaosHpCsv();

			List<DatabaseEdit> editsToMake = new List<DatabaseEdit>();
			string dataPath = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master");
			if (flagRebalancePrices.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[1]);
				editsToMake.AddRange(addEdits("dataRebalancePrices.csv"));
			}
			if (flagFiendsDropRibbons.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[2]);
				editsToMake.AddRange(addEdits("dataFiendsDropRibbons.csv"));
			}
			if (flagRebalanceBosses.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[4]);
				editsToMake.AddRange(addEdits("dataRebalanceBosses.csv"));
			}
			if (flagRestoreCritRating.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[8]);
				editsToMake.AddRange(addEdits("dataRestoreCritRating.csv"));
			}
			if (flagWandsAddInt.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[16]);
				editsToMake.AddRange(addEdits("dataWandsAddInt.csv"));
			}
			if (flagReduceEncounterRate.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[32]);
				editsToMake.AddRange(addEdits("dataReduceEncounterRate.csv"));
			}
			if (flagReduceChaosHP.Checked)
			{
				// Advance the RNG
				r1.NextBytes(new byte[64]);
				editsToMake.AddRange(addEdits("dataReduceChaosHP.csv"));
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
		private void randomizeShops()
		{
			Shops randoShops = new Shops(r1, modeShops.SelectedIndex,
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master", "product.csv"),
					flagShopsTrad.Checked);
		}

		private void randomizeMagic()
		{
			Magic magicData = new Magic(r1, modeMagic.SelectedIndex,
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master", "ability.csv"),
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master", "product.csv"),
					flagMagicShuffleShops.Checked, flagMagicKeepPermissions.Checked);
		}

		private void randomizeKeyItems()
		{
			KeyItems randoKeyItems = new KeyItems(r1,
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets")); // , "Assets", "GameAssets", "Serial", "Res", "Map"
		}
		private void randomizeTreasure()
		{
			Treasure randoChests = new Treasure(r1, modeTreasure.SelectedIndex,
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets"), // , "Assets", "GameAssets", "Serial", "Res", "Map"
					flagTreasureTrad.Checked, flagFiendsDropRibbons.Checked, flagJackInTheBox.Checked);
		}

		private void randomizeHeroStats()
		{
			Stats.RandomizeStats(modeHeroStats.SelectedIndex, flagHeroStatsStandardize.Checked, flagBoostPromoted.Checked, r1, Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master"),
					Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "message", "Assets", "GameAssets", "Serial", "Data", "Message"));
		}

		private void monsterBoost()
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

			string monsterFile = Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master", "monster.csv");

			Monster monsters = new Monster(r1, monsterFile, xp, 0, xp, 0, minStatAdjustment, maxStatAdjustment);
		}

		private void noEscapeAdjustment()
		{
			MonsterParty.mandatoryRandomEncounters(r1, Path.Combine(FF1PRFolder.Text, "FINAL FANTASY_Data", "StreamingAssets", "Magicite", "FF1PRR", "master", "Assets", "GameAssets", "Serial", "Data", "Master"), flagNoEscapeRandomize.Checked);
		}

		private void PopulateSpriteSelection()
		{
			string spriteDirectory = Path.Combine(Application.StartupPath, "data", "mods", "sprites");
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

		private void flagReduceChaosHP_CheckedChanged(object sender, EventArgs e)
		{
			chaosHpTrackBar.Enabled = !flagReduceChaosHP.Checked;
		}

		private void UpdateChaosHpCsv()
		{
			string filePath = Path.Combine("data", "dataReduceChaosHP.csv");
			var lines = File.ReadAllLines(filePath).ToList();

			int[] hpValues = { 1, 9999, 20000, 30000 };
			int selectedHp = hpValues[chaosHpTrackBar.Value];

			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i].StartsWith("monster.csv,Chaos,128,hp"))
				{
					var parts = lines[i].Split(',');
					parts[4] = selectedHp.ToString(); // Update the HP value
					lines[i] = string.Join(",", parts);
					break;
				}
			}

			File.WriteAllLines(filePath, lines);
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
				foreach (var item in currentSelectionsListBox.Items)
				{
					writer.WriteLine(item.ToString());
				}
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

		}
	}
}
