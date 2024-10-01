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
			List<locationData> ld = new List<locationData>();
			List<int> complete = new List<int>();

			Process p = new Process();
			string keyItemDataFile = flagDockAnywhere ? "KeyItemDataShipDockAnywhere.lp" : "KeyItemDataShip.lp";
			string keyItemSolvingFile = flagShuffleCanoe ? "KeyItemSolvingShipShuffleCanoe.lp" : "KeyItemSolvingShip.lp";

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
		}

		private void JsonRewrite(string fileName, locationData loc)
		{
			string json = File.ReadAllText(fileName);
			EventJSON jEvents = JsonConvert.DeserializeObject<EventJSON>(json);
			var mnemonicsList = jEvents.Mnemonics.ToList();

			// Replace the SysCall for the canoe and crystals with the placeholder
			if (loc.keyItem != (int)flags.canoe)
			{
				ReplaceSysCall(mnemonicsList, "カヌーの入手", "キー入力待ち");
			}
			// Always replace crystal SysCalls with the placeholder
			ReplaceCrystalSysCalls(mnemonicsList);

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
					if (loc.keyItem == (int)flags.canoe)
					{
						singleScript.operands.sValues[0] = "カヌーの入手"; // Canoe SysCall
						syscallUpdated = true;
					}
					else if (IsCrystal(loc.keyItem))
					{
						singleScript.operands.sValues[0] = GetCrystalSysCall(loc.keyItem); // Crystal SysCall
						syscallUpdated = true;
					}
					else
					{
						// Replace with placeholder
						singleScript.operands.sValues[0] = "キー入力待ち";
						syscallUpdated = true;
					}
				}
			}

			// If no SysCall was found and the key item is special, add the SysCall
			if (!syscallUpdated && (loc.keyItem == (int)flags.canoe || IsCrystal(loc.keyItem)))
			{
				AddSysCall(mnemonicsList, loc.keyItem == (int)flags.canoe ? "カヌーの入手" : GetCrystalSysCall(loc.keyItem));
			}

			// Serialize and save the updated JSON
			jEvents.Mnemonics = mnemonicsList.ToArray();
			string updatedJson = JsonConvert.SerializeObject(jEvents, Formatting.Indented);
			File.WriteAllText(fileName, updatedJson);
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


	}
}
