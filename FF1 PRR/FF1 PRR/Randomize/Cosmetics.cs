using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
  public class Cosmetics
  {
    private class BattleBackground
    {
      public int id { get; set; }
      public string asset_name { get; set; }
      public int ability_random_group_id { get; set; }
    }

    public Cosmetics(Random r1, string modsDir, string gameDir, bool shuffleBackgrounds)
    {
      string modsFilePath = Path.Combine(modsDir, "battle_background_asset.csv");
      string gameFilePath = Path.Combine(gameDir, "battle_background_asset.csv");

      List<BattleBackground> battleBackgrounds;

      // Read from the mods directory
      using (var reader = new StreamReader(modsFilePath))
      using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
      {
        battleBackgrounds = csv.GetRecords<BattleBackground>().ToList();
      }

      if (shuffleBackgrounds)
      {
        List<string> assetNames = battleBackgrounds.Select(bb => bb.asset_name).ToList();
        assetNames.Shuffle(r1);

        for (int i = 0; i < battleBackgrounds.Count; i++)
        {
          battleBackgrounds[i].asset_name = assetNames[i];
        }
      }

      // Write the modified list directly to the game directory
      using (var writer = new StreamWriter(gameFilePath))
      using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
      {
        // Write header manually
        csv.WriteField("id");
        csv.WriteField("asset_name");
        csv.WriteField("ability_random_group_id");
        csv.NextRecord();

        // Write records
        foreach (var background in battleBackgrounds)
        {
          csv.WriteField(background.id);
          csv.WriteField(background.asset_name);
          csv.WriteField(background.ability_random_group_id);
          csv.NextRecord();
        }
      }
    }
  }
}
