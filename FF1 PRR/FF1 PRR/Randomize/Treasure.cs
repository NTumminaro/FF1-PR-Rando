using CsvHelper;
using Newtonsoft.Json;
using FF1_PRR.Inventory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FF1_PRR.Common;

namespace FF1_PRR.Randomize
{
	public class Treasure
	{
		private class ChestInfo
		{
			public int flag_id { get; set; }
			public string map { get; set; }
			public string submap { get; set; }
			public int entity_id { get; set; }
			public int content_id { get; set; }
			public int content_num { get; set; }
			public string script_id { get; set; }
		}

		private class PackageJson
		{
			public string name { get; set; }
			public string asset_group_name { get; set; }
			public List<Texture> texture { get; set; }
			public List<Map> map { get; set; }

			public class Texture
			{
				public string name { get; set; }
				public string asset { get; set; }
			}

			public class Map
			{
				public string name { get; set; }
				public string tilemap { get; set; }
				public string collision { get; set; }
				public string entity_default { get; set; }
				public List<Entity> entity { get; set; }
				public List<Script> script { get; set; }
			}

			public class Entity
			{
				public string name { get; set; }
				public string asset { get; set; }
			}

			public class Script
			{
				public string name { get; set; }
				public string asset { get; set; }
			}
		}

		private class ImportData
		{
			public List<string> keys { get; set; }
			public List<string> values { get; set; }

			public ImportData()
			{
				keys = new List<string>();
				values = new List<string>();
			}
		}

		List<ChestInfo> treasureList = new List<ChestInfo>();

		public Treasure(Random r1, int randoLevel, string datapath, bool traditional, bool fiendsRibbons, bool jackInTheBox)
		{
			using (var reader = new StreamReader(Path.Combine("data", "chestInfo.csv")))
			using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
			{
				treasureList = csv.GetRecords<ChestInfo>().ToList();
			}

			if (randoLevel == 1 || true) // Shuffle
			{
				List<(int, int)> contentsList = new List<(int, int)>();
				foreach (ChestInfo chest in treasureList)
				{
					contentsList.Add((chest.content_id, chest.content_num));
				}
				contentsList.Shuffle(r1);
				foreach (ChestInfo chest in treasureList)
				{
					(chest.content_id, chest.content_num) = contentsList[0];
					contentsList.RemoveAt(0);
				}
			}

			if (randoLevel == 2) // Standard
			{
				// Add Standard randomization logic here
			}

			if (randoLevel == 3) // Pro
			{
				// Add Pro randomization logic here
			}

			if (randoLevel == 4) // Wild
			{
				// Add Wild randomization logic here
			}

			// Assign script_id and set foldername for JackInTheBox
			if (jackInTheBox)
			{
				var zeroScriptChests = treasureList.Where(chest => chest.script_id == "0").ToList();
				if (zeroScriptChests.Any())
				{
					var randomChest = zeroScriptChests[r1.Next(zeroScriptChests.Count)];
					randomChest.script_id = "542";
				}
			}

			// Now write the chests back
			foreach (var chestsByFile in treasureList.GroupBy(x => x.submap))
			{
				string filename = Inventory.Updater.MemoriaToMagiciteFile(datapath, Path.Combine("Maps", chestsByFile.First().map, chestsByFile.First().submap, "entity_default.json"));
				string json = File.ReadAllText(filename);
				EvRoot entity_default = JsonConvert.DeserializeObject<EvRoot>(json);
				foreach (var chest in chestsByFile)
				{
					foreach (EvLayer layer in entity_default.layers)
					{
						foreach (EvObject obj in layer.objects)
						{
							foreach (EvProperty property in obj.properties)
							{
								if (property.name == "flag_id")
								{
									if (Int32.Parse(property.value) == chest.flag_id)
									{
										if (fiendsRibbons && Armor.ribbon == chest.content_id)
										{
											obj.properties.Find(x => x.name == "content_id").value = Items.potion.ToString();
										}
										else
										{
											obj.properties.Find(x => x.name == "content_id").value = chest.content_id.ToString();
										}
										obj.properties.Find(x => x.name == "content_num").value = chest.content_num.ToString();
										obj.properties.Find(x => x.name == "message_key").value = (chest.content_id == 1) ? "MSG_OTHER_12" : "MSG_OTHER_11";
										obj.properties.Find(x => x.name == "script_id").value = chest.script_id.ToString();
										if (chest.script_id == "542")
										{
											string mapdirectory = Inventory.Updater.MemoriaToMagiciteFile(datapath, Path.Combine("Maps", chestsByFile.First().map, chestsByFile.First().submap));
											string mapRootPath = Path.Combine(datapath, "Magicite", "FF1PRR", chestsByFile.First().map);
											AddScriptsToSubmap(mapdirectory, mapRootPath);
											AddScriptsToPackageJson(datapath, chestsByFile.First().map, chestsByFile.First().submap);
										}
										goto NextChest;
									}
								}
							}
						}
					}
				NextChest:
					continue;
				}
				JsonSerializer serializer = new JsonSerializer();

				using (StreamWriter sw = new StreamWriter(filename))
				using (JsonWriter writer = new JsonTextWriter(sw))
				{
					serializer.Serialize(writer, entity_default);
				}
			}
		}

		private void AddScriptsToSubmap(string submapPath, string mapRootPath)
		{
			// Copy custom scripts to the submap directory
			File.Copy(Path.Combine("data", "mods", "scripts", "sc_t_0099.json"), Path.Combine(submapPath, "sc_t_0099.json"), true);
			File.Copy(Path.Combine("data", "mods", "scripts", "sc_t_0099_after.json"), Path.Combine(submapPath, "sc_t_0099_after.json"), true);

			// Update Export.json with the new scripts
			UpdateExportJson(mapRootPath, submapPath, "sc_t_0099");
			UpdateExportJson(mapRootPath, submapPath, "sc_t_0099_after");
		}

		private void UpdateExportJson(string mapRootPath, string submapPath, string scriptName)
		{
			string exportJsonPath = Path.Combine(mapRootPath, "keys", "Export.json");

			ImportData importJson = new ImportData();
			if (File.Exists(exportJsonPath))
			{
				using (StreamReader sr = new StreamReader(exportJsonPath))
				using (JsonTextReader reader = new JsonTextReader(sr))
				{
					JsonSerializer deserializer = new JsonSerializer();
					importJson = deserializer.Deserialize<ImportData>(reader);
				}
			}

			var keysSet = new HashSet<string>(importJson.keys);
			var valuesSet = new HashSet<string>(importJson.values);

			string submap = Path.GetFileName(submapPath);
			string key = $"{submap}/{scriptName}";
			string value = Path.Combine("Assets", "GameAssets", "Serial", "Res", "Map", Path.GetFileName(mapRootPath), Path.GetFileName(submapPath), scriptName).Replace("\\", "/");
			if (!keysSet.Contains(key))
			{
				keysSet.Add(key);
				valuesSet.Add(value);
			}

			importJson.keys = keysSet.ToList();
			importJson.values = valuesSet.ToList();

			JsonSerializer serializer = new JsonSerializer();
			using (StreamWriter sw = new StreamWriter(exportJsonPath))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				serializer.Serialize(writer, importJson);
			}
		}

		private void AddScriptsToPackageJson(string datapath, string map, string submap)
		{
			string packageJsonPath = Path.Combine(datapath, "Magicite", "FF1PRR", map, "Assets", "GameAssets", "Serial", "Res", "Map", map, "package.json");

			PackageJson packageJson;
			using (StreamReader sr = new StreamReader(packageJsonPath))
			using (JsonTextReader reader = new JsonTextReader(sr))
			{
				JsonSerializer deserializer = new JsonSerializer();
				packageJson = deserializer.Deserialize<PackageJson>(reader);
			}

			var mapEntry = packageJson.map.FirstOrDefault(m => m.name == submap);
			if (mapEntry != null)
			{
				string script1Name = "sc_t_0099";
				string script2Name = "sc_t_0099_after";
				string script1Asset = $"{submap}/sc_t_0099";
				string script2Asset = $"{submap}/sc_t_0099_after";

				if (!mapEntry.script.Any(s => s.name == script1Name))
				{
					mapEntry.script.Add(new PackageJson.Script { name = script1Name, asset = script1Asset });
				}

				if (!mapEntry.script.Any(s => s.name == script2Name))
				{
					mapEntry.script.Add(new PackageJson.Script { name = script2Name, asset = script2Asset });
				}
			}

			using (StreamWriter sw = new StreamWriter(packageJsonPath))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(writer, packageJson);
			}
		}
	}
}
