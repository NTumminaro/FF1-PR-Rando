using FF1_PRR.Inventory;
using FF1_PRR.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1_PRR.Randomize
{
	public class KeyItems
	{
		private readonly RandomizerLogger logger;
		private readonly ValidationUtility validator;

		// 24 flags
		enum flags
		{
			lute = 5,
			crown = 10,
			crystalEye = 11,
			joltTonic = 12,
			mysticKey = 13,
			nitroPowder = 14,
			canal = 15,
			starRuby = 17,
			rod = 19,
			earthCrystal = 21,
			canoe = 22,
			fireCrystal = 23,
			floater = 24,
			airship = 25, // Also SysCall
			cube = 26,
			oxyale = 29,
			slab = 31,
			learnLufuin = 32,
			chime = 33,
			waterCrystal = 34,
			airCrystal = 35,
			ratTail = 45,
			adamantite = 47,
			excalibur = 48
		}

		// 26 locations
		enum locations
		{
			sarah = 1,
			coroniaKing = 2,
			pirate = 3,
			marsh = 4,
			astos = 5,
			matoya = 6,
			elfPrince = 7,
			coroniaTreasury = 8,
			dwarf = 9,
			excal = 10,
			vampire = 11,
			sage = 12,
			lich = 13,
			lukahn = 14,
			marilith = 15,
			iceCave = 16,
			airship = 17,
			gaia = 18,
			waterfall = 19,
			shrine5F = 20,
			kraken = 21,
			unne = 22,
			lefein = 23,
			ordeals = 24,
			adamantite = 25,
			tiamat = 26,
		}

		private class locationData
		{
			public int keyItem { get; set; }
			public int ff1Event { get; set; }

			public locationData(int ki, int ev)
			{
				keyItem = ki;
				ff1Event = ev;
			}
		}

		public KeyItems(Random r1, string directory, bool flagDockAnywhere, bool flagShuffleCanoe)
		{
			// Initialize logging and validation
			logger = new RandomizerLogger();
			
			using var operation = logger.StartOperation("Key Items Randomization");
			
			try
			{
				// Validate input parameters
				ValidateInputParameters(r1, directory, operation);
				
				List<locationData> ld = new List<locationData>();
				List<int> complete = new List<int>();

				// Validate clingo files exist
				string keyItemDataFile = flagDockAnywhere ? "KeyItemDataShipDockAnywhere.lp" : "KeyItemDataShip.lp";
				string keyItemSolvingFile = flagShuffleCanoe ? "KeyItemSolvingShipShuffleCanoe.lp" : "KeyItemSolvingShip.lp";
				
				ValidateClingoFiles(keyItemDataFile, keyItemSolvingFile);

				Process p = new Process();

			p.StartInfo = new ProcessStartInfo(Path.Combine("clingo", "clingo"), Path.Combine("clingo", keyItemSolvingFile) + " " + Path.Combine("clingo", keyItemDataFile) + " --sign-def=3 --seed=" + r1.Next().ToString().Trim() + " --outf=2")
			{
				RedirectStandardOutput = true,
				UseShellExecute = false
			};
			p.Start();
			p.WaitForExit();

			//The output of the shell command will be in the outPut variable after the 
			//following line is executed
			var clingoJSON = p.StandardOutput.ReadToEnd();
			File.WriteAllText(@"spoiler.log", clingoJSON);

			ClingoKeyItem events = JsonConvert.DeserializeObject<ClingoKeyItem>(clingoJSON);
			foreach (string pairValue in events.Call[0].Witnesses[0].Value)
			{
				string[] values = pairValue.Replace("pair(", "").Replace(")", "").Split(",");
				int keyItem = -1;
				int location = -1;

				switch (values[0])
				{
					case "lute": keyItem = (int)flags.lute; break;
					case "crown": keyItem = (int)flags.crown; break;
					case "crystal": keyItem = (int)flags.crystalEye; break;
					case "jolt_tonic": keyItem = (int)flags.joltTonic; break;
					case "mystic_key": keyItem = (int)flags.mysticKey; break;
					case "nitro_powder": keyItem = (int)flags.nitroPowder; break;
					case "canal": keyItem = (int)flags.canal; break;
					case "star_ruby": keyItem = (int)flags.starRuby; break;
					case "rod": keyItem = (int)flags.rod; break;
					case "levistone": keyItem = (int)flags.floater; break;
					//case "gear": keyItem = gear; break;
					case "rats_tail": keyItem = (int)flags.ratTail; break;
					case "oxyale": keyItem = (int)flags.oxyale; break;
					case "rosetta_stone": keyItem = (int)flags.slab; break;
					case "chime": keyItem = (int)flags.chime; break;
					case "warp_cube": keyItem = (int)flags.cube; break;
					case "adamantite": keyItem = (int)flags.adamantite; break;
					case "excalibur": keyItem = (int)flags.excalibur; break;
					case "earth": keyItem = (int)flags.earthCrystal; break;
					case "fire": keyItem = (int)flags.fireCrystal; break;
					case "water": keyItem = (int)flags.waterCrystal; break;
					case "air": keyItem = (int)flags.airCrystal; break;
					case "lufienish": keyItem = (int)flags.learnLufuin; break;
					case "canoe": keyItem = (int)flags.canoe; break;
				}

				switch (values[1])
				{
					case "sara": location = (int)locations.sarah; break;
					case "king": location = (int)locations.coroniaKing; break;
					case "bikke": location = (int)locations.pirate; break;
					case "marsh": location = (int)locations.marsh; break;
					case "astos": location = (int)locations.astos; break;
					case "matoya": location = (int)locations.matoya; break;
					case "elf": location = (int)locations.elfPrince; break;
					case "locked_cornelia": location = (int)locations.coroniaTreasury; break;
					case "nerrick": location = (int)locations.dwarf; break;
					case "vampire": location = (int)locations.vampire; break;
					case "sarda": location = (int)locations.sage; break;
					case "ice": location = (int)locations.iceCave; break;
					case "citadel_of_trials": location = (int)locations.ordeals; break;
					case "fairy": location = (int)locations.gaia; break;
					case "mermaids": location = (int)locations.shrine5F; break;
					case "lefien": location = (int)locations.lefein; break;
					case "waterfall": location = (int)locations.waterfall; break;
					case "sky2": location = (int)locations.adamantite; break;
					case "smyth": location = (int)locations.excal; break;
					case "lich": location = (int)locations.lich; break;
					case "kary": location = (int)locations.marilith; break;
					case "kraken": location = (int)locations.kraken; break;
					case "tiamat": location = (int)locations.tiamat; break;
					case "dr_unne": location = (int)locations.unne; break;
					case "lukahn": location = (int)locations.lukahn; break; // Added lukahn
				}

				if (keyItem != -1 && location != -1)
				{
					ld.Add(new locationData(keyItem, location));
					complete.Add(location);
				}
			}

			List<int> allLocs = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
			List<int> bad = allLocs.Except(complete).ToList();

			foreach (int reallyBad in bad)
				ld.Add(new locationData(0, reallyBad));

			// NOW to go through the dreaded act of updating all of the JSON files..........
			foreach (locationData loc in ld)
			{
				string file = "";
				string file2 = "";
				switch (loc.ff1Event)
				{
					case (int)locations.sarah:
						file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20011", "Map_20011_2", "sc_e_0004_1.json"));
						file2 = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20011", "Map_20011_2", "sc_e_0004_2.json"));
						break;
					case (int)locations.coroniaKing: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20011", "Map_20011_2", "sc_e_0003_3.json")); break;
					case (int)locations.pirate: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20040", "Map_20040", "sc_e_0009_2.json")); break;
					case (int)locations.marsh: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30021", "Map_30021_3", "sc_e_0010_1.json")); break;
					case (int)locations.astos: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20081", "Map_20081_1", "sc_e_0011_2.json")); break;
					case (int)locations.matoya: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20031", "Map_20031_1", "sc_e_0012.json")); break;
					case (int)locations.elfPrince: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20071", "Map_20071_1", "sc_e_0013.json")); break;
					case (int)locations.coroniaTreasury: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20011", "Map_20011_1", "sc_e_0014.json")); break;
					case (int)locations.dwarf: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20051", "Map_20051_1", "sc_e_0015.json")); break;
					case (int)locations.excal: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20051", "Map_20051_1", "sc_e_0052.json")); break;
					case (int)locations.vampire: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30031", "Map_30031_3", "sc_e_0017.json")); break;
					case (int)locations.sage: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20101", "Map_20101_1", "sc_e_0019.json")); break;
					case (int)locations.lich: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30031", "Map_30031_5", "sc_e_0021_2.json")); break;
					case (int)locations.lukahn: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20110", "Map_20110", "sc_e_0022.json")); break;
					case (int)locations.marilith: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30051", "Map_30051_6", "sc_e_0023_2.json")); break;
					case (int)locations.iceCave: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30061", "Map_30061_4", "sc_e_0024_2.json")); break;
					case (int)locations.airship: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_10010", "Map_10010", "sc_e_0025_4.json")); break;
					case (int)locations.gaia: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20150", "Map_20150", "sc_e_0029.json")); break;
					case (int)locations.waterfall: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30091", "Map_30091_1", "sc_e_0026.json")); break;
					case (int)locations.shrine5F: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30081", "Map_30081_8", "sc_e_0033.json")); break;
					case (int)locations.kraken: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30081", "Map_30081_1", "sc_e_0036_2.json")); break;
					case (int)locations.unne: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20090", "Map_20090", "sc_e_0034.json")); break;
					case (int)locations.lefein: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_20160", "Map_20160", "sc_e_0035.json")); break;
					case (int)locations.ordeals: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30071", "Map_30071_3", "sc_e_0047.json")); break;
					case (int)locations.adamantite: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30111", "Map_30111_2", "sc_e_0051.json")); break;
					case (int)locations.tiamat: file = Inventory.Updater.MemoriaToMagiciteFile(directory, Path.Combine("Map", "Map_30111", "Map_30111_5", "sc_e_0037_2.json")); break;
				}

				for (int i = 1; i <= 2; i++)
				{
					string fileToUse = (i == 1 ? file : file2 != "" ? file2 : null);
					if (fileToUse != null)
						JsonRewrite(fileToUse, loc);
				}
			}

				// SetVehicle - 3/145/160 for airship.  4/145/162 for ship.
				
				operation.Complete($"Successfully processed {ld.Count} key item locations");
			}
			catch (Exception ex)
			{
				operation.Fail("Key items randomization failed", ex);
				logger.Critical("Key items randomization failed", "KeyItems", ex);
				throw new RandomizationException("Key items randomization failed", ex);
			}
		}

		/// <summary>
		/// Validates input parameters for key items randomization
		/// </summary>
		private void ValidateInputParameters(Random r1, string directory, OperationTracker operation)
		{
			if (r1 == null)
			{
				throw new ArgumentNullException(nameof(r1), "Random number generator cannot be null");
			}

			if (string.IsNullOrWhiteSpace(directory))
			{
				throw new ArgumentException("Directory path cannot be null or empty", nameof(directory));
			}

			var directoryValidation = ValidationUtility.ValidateDirectoryExists(directory, "Game Directory");
			if (!directoryValidation.IsValid)
			{
				operation.Fail($"Invalid directory: {directory}");
				throw new DirectoryNotFoundException($"Game directory not found: {directory}");
			}

			logger.Info($"Validated input parameters - Directory: {directory}", "KeyItems");
		}

		/// <summary>
		/// Validates that required clingo files exist
		/// </summary>
		private void ValidateClingoFiles(string keyItemDataFile, string keyItemSolvingFile)
		{
			var clingoDir = Path.Combine("clingo");
			var clingoExe = Path.Combine(clingoDir, "clingo");
			var dataFile = Path.Combine(clingoDir, keyItemDataFile);
			var solvingFile = Path.Combine(clingoDir, keyItemSolvingFile);

			var validationResults = new List<ValidationResult>
			{
				ValidationUtility.ValidateDirectoryExists(clingoDir, "Clingo Directory"),
				ValidationUtility.ValidateFileExists(clingoExe, "Clingo Executable"),
				ValidationUtility.ValidateFileExists(dataFile, "Key Item Data File"),
				ValidationUtility.ValidateFileExists(solvingFile, "Key Item Solving File")
			};

			logger.LogValidations(validationResults, "Clingo Files");

			var failures = validationResults.Where(r => !r.IsValid).ToList();
			if (failures.Any())
			{
				var errorMessage = $"Clingo file validation failed:\n{string.Join("\n", failures.Select(f => f.ErrorMessage))}";
				logger.Error(errorMessage, "KeyItems");
				throw new FileNotFoundException(errorMessage);
			}

			logger.Info($"Validated clingo files - Data: {keyItemDataFile}, Solving: {keyItemSolvingFile}", "KeyItems");
		}

		private void JsonRewrite(string fileName, locationData loc)
		{
			using var operation = logger.StartOperation($"JSON Rewrite: {Path.GetFileName(fileName)}");
			
			try
			{
				// Validate JSON file exists and is accessible
				var fileValidation = ValidationUtility.ValidateFileExists(fileName, "Event JSON File");
				if (!fileValidation.IsValid)
				{
					operation.Fail($"JSON file validation failed: {fileName}");
					throw new FileNotFoundException($"Event JSON file not found: {fileName}");
				}

				// Validate file permissions
				var permissionValidation = ValidationUtility.ValidateFilePermissions(fileName, true, "Event JSON File");
				if (!permissionValidation.IsValid)
				{
					operation.Fail($"Insufficient permissions for JSON file: {fileName}");
					throw new UnauthorizedAccessException($"Cannot write to JSON file: {fileName}");
				}

				logger.Debug($"Processing location {loc.ff1Event} with key item {loc.keyItem}", "JsonRewrite");

				string json = File.ReadAllText(fileName);
				
				// Validate JSON structure
				var jsonValidation = ValidationUtility.ValidateJsonStructure(fileName, new[] { "Mnemonics" }, "Event JSON");
				if (!jsonValidation.IsValid)
				{
					operation.Fail($"Invalid JSON structure: {fileName}");
					throw new InvalidDataException($"Invalid JSON structure in {fileName}: {jsonValidation.ErrorMessage}");
				}

				EventJSON jEvents = JsonConvert.DeserializeObject<EventJSON>(json);
				
				if (jEvents?.Mnemonics == null)
				{
					operation.Fail($"JSON file has no Mnemonics array: {fileName}");
					throw new InvalidDataException($"JSON file missing Mnemonics array: {fileName}");
				}

				var mnemonicsList = jEvents.Mnemonics.ToList();

			// Flag to track if the SysCall has been updated
			bool syscallUpdated = false;

			for (int i = 0; i < mnemonicsList.Count; i++)
			{
				var singleScript = mnemonicsList[i];

				if (singleScript.mnemonic == "MsgFunfare")
				{
					if (loc.keyItem == (int)flags.canoe)
					{
						singleScript.operands.sValues[0] = "MSG_GET_CANOE_02";
					}
					else
					{
						singleScript.operands.sValues[0] = "MSG_KEY_" + (loc.keyItem > 0 ? loc.keyItem.ToString() : "A1");
					}
				}

				if (singleScript.mnemonic == "GetItem" && singleScript.operands.iValues[1] >= 0)
				{
					int keyItem = GetKeyItemValue(loc.keyItem);
					singleScript.operands.iValues[0] = keyItem;
					singleScript.operands.iValues[1] = keyItem == 2 ? 0 : 1;
				}

				if (singleScript.mnemonic == "SetFlag" && singleScript.operands.iValues[0] < 100 && singleScript.operands.sValues[0] == "ScenarioFlag1")
				{
					singleScript.operands.iValues[0] = loc.keyItem > 0 ? loc.keyItem : 0;
				}

				// Process SysCall mnemonics
				if (singleScript.mnemonic == "SysCall")
				{
					// Only process SysCalls that are placeholders or the specific special calls
					if (singleScript.operands.sValues[0] == "キー入力待ち" ||
							singleScript.operands.sValues[0] == "カヌーの入手" ||
							IsCrystalSysCall(singleScript.operands.sValues[0]))
					{
						if (loc.keyItem == (int)flags.canoe)
						{
							singleScript.operands.sValues[0] = "カヌーの入手"; // Canoe SysCall
							syscallUpdated = true;
							logger.Debug($"Updated SysCall to Canoe for location {loc.ff1Event}", "SysCall Update");
						}
						else if (IsCrystal(loc.keyItem))
						{
							var crystalSysCall = GetCrystalSysCall(loc.keyItem);
							singleScript.operands.sValues[0] = crystalSysCall; // Crystal SysCall
							syscallUpdated = true;
							logger.Debug($"Updated SysCall to {crystalSysCall} for location {loc.ff1Event}", "SysCall Update");
						}
						else
						{
							// Replace with placeholder for non-special items
							singleScript.operands.sValues[0] = "キー入力待ち";
							syscallUpdated = true;
							logger.Debug($"Updated SysCall to placeholder for location {loc.ff1Event} (key item {loc.keyItem})", "SysCall Update");
						}
					}
				}
			}

			// If no SysCall was found and the key item is special, add the SysCall
			if (!syscallUpdated && (loc.keyItem == (int)flags.canoe || IsCrystal(loc.keyItem)))
			{
				AddSysCall(mnemonicsList, loc.keyItem == (int)flags.canoe ? "カヌーの入手" : GetCrystalSysCall(loc.keyItem));
			}

				// Validate crystal SysCall logic
				ValidateCrystalSysCallLogic(loc, syscallUpdated);

				// Serialize and save the updated JSON
				jEvents.Mnemonics = mnemonicsList.ToArray();
				string updatedJson = JsonConvert.SerializeObject(jEvents, Formatting.Indented);
				
				// Create backup before writing
				CreateBackupFile(fileName);
				
				File.WriteAllText(fileName, updatedJson);
				
				// Validate the written file
				var postWriteValidation = ValidationUtility.ValidateJsonStructure(fileName, new[] { "Mnemonics" }, "Updated Event JSON");
				if (!postWriteValidation.IsValid)
				{
					operation.Fail($"Post-write validation failed: {fileName}");
					RestoreBackupFile(fileName);
					throw new InvalidDataException($"JSON file corrupted after write: {fileName}");
				}

				logger.Debug($"Successfully updated JSON file: {Path.GetFileName(fileName)}", "JsonRewrite");
				operation.Complete($"JSON rewrite completed for location {loc.ff1Event}");
			}
			catch (Exception ex)
			{
				operation.Fail($"JSON rewrite failed for {Path.GetFileName(fileName)}", ex);
				logger.Error($"Failed to rewrite JSON file: {fileName}", "JsonRewrite", ex);
				
				// Attempt to restore backup if it exists
				RestoreBackupFile(fileName);
				throw new FileOperationException(fileName, "JSON rewrite failed", ex);
			}
		}

		/// <summary>
		/// Validates crystal SysCall logic to ensure proper crystal tracking
		/// </summary>
		private void ValidateCrystalSysCallLogic(locationData loc, bool syscallUpdated)
		{
			if (IsCrystal(loc.keyItem))
			{
				var expectedSysCall = GetCrystalSysCall(loc.keyItem);
				if (string.IsNullOrEmpty(expectedSysCall))
				{
					logger.Warning($"Unknown crystal type for key item {loc.keyItem} at location {loc.ff1Event}", "Crystal Validation");
					throw new InvalidOperationException($"Unknown crystal type for key item {loc.keyItem}");
				}

				if (!syscallUpdated)
				{
					logger.Warning($"Crystal SysCall not updated for key item {loc.keyItem} at location {loc.ff1Event}", "Crystal Validation");
				}
				else
				{
					logger.Info($"Crystal SysCall validated: {expectedSysCall} for location {loc.ff1Event}", "Crystal Validation");
				}
			}
		}

		/// <summary>
		/// Creates a backup of the JSON file before modification
		/// </summary>
		private void CreateBackupFile(string fileName)
		{
			try
			{
				var backupFileName = fileName + ".backup";
				if (File.Exists(fileName))
				{
					File.Copy(fileName, backupFileName, true);
					logger.Debug($"Created backup: {Path.GetFileName(backupFileName)}", "Backup");
				}
			}
			catch (Exception ex)
			{
				logger.Warning($"Failed to create backup for {Path.GetFileName(fileName)}: {ex.Message}", "Backup");
			}
		}

		/// <summary>
		/// Restores a backup file if it exists
		/// </summary>
		private void RestoreBackupFile(string fileName)
		{
			try
			{
				var backupFileName = fileName + ".backup";
				if (File.Exists(backupFileName))
				{
					File.Copy(backupFileName, fileName, true);
					File.Delete(backupFileName);
					logger.Info($"Restored backup for {Path.GetFileName(fileName)}", "Backup");
				}
			}
			catch (Exception ex)
			{
				logger.Error($"Failed to restore backup for {Path.GetFileName(fileName)}: {ex.Message}", "Backup");
			}
		}


		private void ReplaceSysCall(List<EventJSON.Mnemonic> mnemonicsList, string original, string replacement)
		{
			foreach (var mnemonic in mnemonicsList)
			{
				if (mnemonic.mnemonic == "SysCall" && mnemonic.operands.sValues[0] == original)
				{
					mnemonic.operands.sValues[0] = replacement;
				}
			}
		}

		private void ReplaceCrystalSysCalls(List<EventJSON.Mnemonic> mnemonicsList)
		{
			foreach (var mnemonic in mnemonicsList)
			{
				if (mnemonic.mnemonic == "SysCall" && IsCrystalSysCall(mnemonic.operands.sValues[0]))
				{
					mnemonic.operands.sValues[0] = "キー入力待ち";
				}
			}
		}

		private bool IsCrystal(int keyItem)
		{
			return keyItem == (int)flags.earthCrystal || keyItem == (int)flags.fireCrystal ||
						 keyItem == (int)flags.waterCrystal || keyItem == (int)flags.airCrystal;
		}

		private string GetCrystalSysCall(int keyItem)
		{
			return keyItem switch
			{
				(int)flags.earthCrystal => "黄色クリスタル点灯",
				(int)flags.fireCrystal => "赤色クリスタル点灯",
				(int)flags.waterCrystal => "青色クリスタル点灯",
				(int)flags.airCrystal => "緑色クリスタル点灯",
				_ => null,
			};
		}

		private void AddSysCall(List<EventJSON.Mnemonic> mnemonicsList, string sysCall)
		{
			bool foundMsgFunfare = false;

			for (int i = 0; i < mnemonicsList.Count; i++)
			{
				if (mnemonicsList[i].mnemonic == "MsgFunfare")
				{
					foundMsgFunfare = true;
				}

				if (foundMsgFunfare && mnemonicsList[i].mnemonic == "Exit")
				{
					var newSysCall = new EventJSON.Mnemonic
					{
						mnemonic = "SysCall",
						operands = new EventJSON.Operands
						{
							iValues = new int?[] { 0, 0, 0, 0, 0, 0, 0, 0 },
							rValues = new float?[] { 0, 0, 0, 0, 0, 0, 0, 0 },
							sValues = new string[] { sysCall, "", "", "", "", "", "", "" }
						},
						type = 1,
						comment = ""
					};
					mnemonicsList.Insert(i, newSysCall);
					break;
				}
			}
		}

		private int GetKeyItemValue(int keyItem)
		{
			return keyItem switch
			{
				(int)flags.floater => 55,
				(int)flags.chime => 56,
				(int)flags.cube => 58,
				(int)flags.oxyale => 60,
				(int)flags.crown => 46,
				(int)flags.crystalEye => 47,
				(int)flags.joltTonic => 48,
				(int)flags.mysticKey => 49,
				(int)flags.nitroPowder => 50,
				(int)flags.starRuby => 53,
				(int)flags.rod => 54,
				(int)flags.slab => 52,
				(int)flags.adamantite => 51,
				(int)flags.lute => 45,
				(int)flags.ratTail => 57,
				(int)flags.excalibur => 92,
				(int)flags.canoe => 61, // Handle the canoe
																// Crystals don't have a GetItem value
				_ => 2,
			};
		}

		private bool IsCrystalSysCall(string sValue)
		{
			return sValue == "黄色クリスタル点灯" || sValue == "赤色クリスタル点灯" ||
						 sValue == "青色クリスタル点灯" || sValue == "緑色クリスタル点灯";
		}

		/// <summary>
		/// Validates that all crystal locations have proper SysCalls after randomization
		/// </summary>
		public static void ValidateCrystalSysCalls(string directory)
		{
			var logger = new RandomizerLogger();
			using var operation = logger.StartOperation("Crystal SysCall Validation");

			try
			{
				var crystalLocations = new Dictionary<string, string>
				{
					{ "Earth Crystal (Lich)", Path.Combine("Map", "Map_30031", "Map_30031_5", "sc_e_0021_2.json") },
					{ "Fire Crystal (Marilith)", Path.Combine("Map", "Map_30051", "Map_30051_6", "sc_e_0023_2.json") },
					{ "Water Crystal (Kraken)", Path.Combine("Map", "Map_30081", "Map_30081_1", "sc_e_0036_2.json") },
					{ "Air Crystal (Tiamat)", Path.Combine("Map", "Map_30111", "Map_30111_5", "sc_e_0037_2.json") }
				};

				var expectedSysCalls = new Dictionary<string, string>
				{
					{ "Earth Crystal (Lich)", "黄色クリスタル点灯" },
					{ "Fire Crystal (Marilith)", "赤色クリスタル点灯" },
					{ "Water Crystal (Kraken)", "青色クリスタル点灯" },
					{ "Air Crystal (Tiamat)", "緑色クリスタル点灯" }
				};

				foreach (var location in crystalLocations)
				{
					var filePath = Inventory.Updater.MemoriaToMagiciteFile(directory, location.Value);
					
					if (!File.Exists(filePath))
					{
						logger.Warning($"Crystal location file not found: {filePath}", "Crystal Validation");
						continue;
					}

					var json = File.ReadAllText(filePath);
					var jEvents = JsonConvert.DeserializeObject<EventJSON>(json);
					
					bool foundCorrectSysCall = false;
					bool foundPlaceholder = false;

					foreach (var mnemonic in jEvents.Mnemonics)
					{
						if (mnemonic.mnemonic == "SysCall")
						{
							var sysCallValue = mnemonic.operands.sValues[0];
							
							if (sysCallValue == expectedSysCalls[location.Key])
							{
								foundCorrectSysCall = true;
								logger.Info($"✓ {location.Key}: Found correct SysCall '{sysCallValue}'", "Crystal Validation");
							}
							else if (sysCallValue == "キー入力待ち")
							{
								foundPlaceholder = true;
								logger.Warning($"⚠ {location.Key}: Found placeholder SysCall - may need crystal injection", "Crystal Validation");
							}
						}
					}

					if (!foundCorrectSysCall && !foundPlaceholder)
					{
						logger.Info($"ℹ {location.Key}: No crystal SysCall found (may have different item)", "Crystal Validation");
					}
				}

				operation.Complete("Crystal SysCall validation completed");
			}
			catch (Exception ex)
			{
				operation.Fail("Crystal SysCall validation failed", ex);
				logger.Error("Failed to validate crystal SysCalls", "Crystal Validation", ex);
			}
		}


	}
}
