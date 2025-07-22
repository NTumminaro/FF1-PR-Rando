using Newtonsoft.Json.Linq;
using FF1_PRR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
  public class ShuffleBosses
  {
    private class BossData
    {
      public string FilePath { get; set; }
      public int EncountBossId { get; set; }
    }

    public ShuffleBosses(Random r1, string gameDir, bool shuffleBosses, bool includeAllBosses, bool garlandAtShrine = false, bool vampireIsWarmech = false)
    {
      // Define the paths to boss files and their initial EncountBossIds
      List<BossData> bossFiles = new List<BossData>
            {
                new BossData { FilePath = Path.Combine(gameDir, "Map_30031", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30031", "Map_30031_5", "sc_e_0021.json"), EncountBossId = 345 }, // Lich
                new BossData { FilePath = Path.Combine(gameDir, "Map_30051", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30051", "Map_30051_6", "sc_e_0023.json"), EncountBossId = 344 }, // Marilith
                new BossData { FilePath = Path.Combine(gameDir, "Map_30081", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30081", "Map_30081_1", "sc_e_0036.json"), EncountBossId = 343 }, // Kraken
                new BossData { FilePath = Path.Combine(gameDir, "Map_30111", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30111", "Map_30111_5", "sc_e_0037.json"), EncountBossId = 342 }  // Tiamat
            };

      if (includeAllBosses)
      {
        var additionalBosses = new List<BossData>
                {
                    new BossData { FilePath = Path.Combine(gameDir, "Map_20081", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_20081", "Map_20081_1", "sc_e_0011.json"), EncountBossId = 348 }, // Astos
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30011", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30011", "Map_30011_1", "sc_e_0003.json"), EncountBossId = 350 }, // Garland
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30031", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30031", "Map_30031_3", "sc_e_0016.json"), EncountBossId = 347 }, // Vampire
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30121", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30121", "Map_30121_4", "sc_e_0040.json"), EncountBossId = 338 }, // Lich 2
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30121", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30121", "Map_30121_5", "sc_e_0041.json"), EncountBossId = 339 }, // Marilith 2
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30121", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30121", "Map_30121_6", "sc_e_0042.json"), EncountBossId = 340 }, // Kraken 2
                    new BossData { FilePath = Path.Combine(gameDir, "Map_30121", "Assets", "GameAssets", "Serial", "Res", "Map", "Map_30121", "Map_30121_7", "sc_e_0043.json"), EncountBossId = 341 }  // Tiamat 2
                };
        bossFiles.AddRange(additionalBosses);
      }

      if (shuffleBosses)
      {
        // Handle Vampire is Warmech option first (before shuffling)
        if (vampireIsWarmech)
        {
          ModifyVampireToWarmech(gameDir);
        }

        // Shuffle the boss EncountBossIds
        List<int> bossIds = bossFiles.Select(b => b.EncountBossId).ToList();
        
        // Handle Garland at Shrine option - exclude Garland from shuffle if enabled
        if (garlandAtShrine && includeAllBosses)
        {
          var garlandBoss = bossFiles.FirstOrDefault(b => b.EncountBossId == 350);
          if (garlandBoss != null)
          {
            // Remove Garland's ID from the shuffle pool
            bossIds.Remove(350);
            // Shuffle remaining IDs
            bossIds.Shuffle(r1);
            
            // Assign shuffled IDs to non-Garland bosses
            int shuffleIndex = 0;
            for (int i = 0; i < bossFiles.Count; i++)
            {
              if (bossFiles[i].EncountBossId != 350)
              {
                bossFiles[i].EncountBossId = bossIds[shuffleIndex];
                shuffleIndex++;
              }
              // Garland keeps its original ID (350)
            }
          }
        }
        else
        {
          // Normal shuffle - all bosses included
          bossIds.Shuffle(r1);
          for (int i = 0; i < bossFiles.Count; i++)
          {
            bossFiles[i].EncountBossId = bossIds[i];
          }
        }
      }

      // Modify each boss file with the new EncountBossId
      foreach (var boss in bossFiles)
      {
        ModifyBossFile(boss.FilePath, boss.EncountBossId);
      }
    }

    private void ModifyBossFile(string filePath, int newEncountBossId)
    {
      string jsonContent = File.ReadAllText(filePath);
      JObject json = JObject.Parse(jsonContent);

      JToken encountBossToken = json.SelectToken("$.Mnemonics[?(@.mnemonic == 'EncountBoss')]");
      if (encountBossToken != null)
      {
        encountBossToken["operands"]["iValues"][0] = newEncountBossId;
      }

      File.WriteAllText(filePath, json.ToString());
    }

    private void ModifyVampireToWarmech(string gameDir)
    {
      // Modify the monster_party.csv to replace Vampire (ID 347) with Warmech stats
      string monsterPartyPath = Path.Combine("data", "assets", "monster_party.csv");
      
      if (File.Exists(monsterPartyPath))
      {
        var lines = File.ReadAllLines(monsterPartyPath).ToList();
        
        for (int i = 0; i < lines.Count; i++)
        {
          if (lines[i].StartsWith("347,"))
          {
            // Replace Vampire entry (347) with Warmech stats but keep the encounter ID
            lines[i] = "347,29,15,1,0,1,0,0,0,0,0,2,0,0,0,0,25,10,1,0,25,10,1,0,25,10,1,0,25,10,1,0,25,10,1,0,25,10,1,0,25,10,1,119,25,10,1,0,25,10,1";
            break;
          }
        }
        
        File.WriteAllLines(monsterPartyPath, lines);
      }
    }
  }

}
