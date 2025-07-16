using CsvHelper;
using Newtonsoft.Json;
using FF1_PRR.Inventory;
using FF1_PRR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
  public class NPCs
  {
    private readonly string chaosLogFilePath;
    private class NPCInfo
    {
      public int id { get; set; }
      public string map { get; set; }
      public string submap { get; set; }
      public string name { get; set; }
      public string asset_id { get; set; }
      public string message_key { get; set; }
      public string script_id { get; set; }
      public bool exclude { get; set; }
      public string file_name { get; set; }

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

    List<NPCInfo> npcList = new List<NPCInfo>();

    private List<string> customAssetIds = new List<string>
        {
            "1", "2", "11", "13", "15", "16", "17", "18", "21",
            "24", "28", "30", "36", "37", "38", "40", "46", "52",
            "53", "54", "56", "57", "58", "59", "60", "61", "62",
            "63", "64", "66", "67", "68", "69", "76", "77", "78",
            "81", "83", "85", "86", "87", "88", "95", "104", "187",
            "201", "202", "203", "204",
        };

    public NPCs(Random r1, string datapath, bool hiddenChaos, bool shuffleAssetIds)
    {
      chaosLogFilePath = Path.Combine(datapath, "chaos_location.txt");
      using (var reader = new StreamReader(Path.Combine("data", "npc_data.csv")))
      using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
      {
        npcList = csv.GetRecords<NPCInfo>().ToList();
      }

      List<NPCInfo> regularNPCs = npcList.Where(npc => !npc.exclude).ToList();
      List<string> assetIdList = new List<string>(customAssetIds);


      // Assign script_id and set foldername for JackInTheBox
      if (hiddenChaos)
      {
        var randomNPC = regularNPCs[r1.Next(regularNPCs.Count)];
        randomNPC.script_id = "542";

        string mapdirectory = Inventory.Updater.MemoriaToMagiciteFile(datapath, Path.Combine("Maps", randomNPC.map, randomNPC.submap));
        string mapRootPath = Path.Combine(datapath, "Magicite", "FF1PRR", randomNPC.map);
        AddScriptsToSubmap(mapdirectory, mapRootPath);
        AddScriptsToPackageJson(datapath, randomNPC.map, randomNPC.submap);
      }

      if (shuffleAssetIds)
      {
        assetIdList.Shuffle(r1);

        var evenlyDistributedAssetIds = new List<string>();
        while (evenlyDistributedAssetIds.Count < npcList.Count)
        {
          evenlyDistributedAssetIds.AddRange(assetIdList);
        }
        evenlyDistributedAssetIds = evenlyDistributedAssetIds.Take(npcList.Count).ToList();
        evenlyDistributedAssetIds.Shuffle(r1);

        for (int i = 0; i < npcList.Count; i++)
        {
          npcList[i].asset_id = evenlyDistributedAssetIds[i];
        }
      }


      // Now write the NPCs back
      foreach (var npc in npcList)
      {
        string filename = string.IsNullOrEmpty(npc.file_name) ? "entity_default.json" : npc.file_name;
        string filePath = Inventory.Updater.MemoriaToMagiciteFile(datapath, Path.Combine("Maps", npc.map, npc.submap, filename));

        string json = File.ReadAllText(filePath);
        EvRoot entity_default = JsonConvert.DeserializeObject<EvRoot>(json);

        bool npcFound = false;
        foreach (EvLayer layer in entity_default.layers)
        {
          foreach (EvObject obj in layer.objects)
          {
            if (obj.id == npc.id)
            {
              obj.properties.Find(x => x.name == "asset_id").value = npc.asset_id.ToString();

              // Update script_id and action_id only if script_id is 542
              if (npc.script_id == "542")
              {
                obj.properties.Find(x => x.name == "script_id").value = npc.script_id.ToString();

                var actionProperty = obj.properties.Find(x => x.name == "action_id");
                if (actionProperty != null)
                {
                  actionProperty.value = "2";
                }
              }

              npcFound = true;
              break;
            }
          }
          if (npcFound) break;
        }
        if (!npcFound)
        {
          // NPC not found - this could indicate a data mismatch
          // Consider logging this to a debug file if needed for troubleshooting
        }

        JsonSerializer serializer = new JsonSerializer();

        using (StreamWriter sw = new StreamWriter(filePath))
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



    private void LogChaosLocation(string message)
    {
      using (StreamWriter sw = new StreamWriter(chaosLogFilePath, true))
      {
        sw.WriteLine($"{DateTime.Now}: {message}");
      }
    }
  }
}




