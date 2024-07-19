using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FF1_PRR.Randomize
{
  public class NPCAssets
  {
    private class MapObject
    {
      public int id { get; set; }
      public string asset_name { get; set; }
      public int shadow_type { get; set; }
      public string shadow_name { get; set; }
      public bool shuffle { get; set; }
    }

    public NPCAssets(Random r1, string modsDir, string gameDir, bool oopsAllGarland, bool shuffleNPCs)
    {
      string modsFilePath = Path.Combine(modsDir, "mapobject.csv");
      string gameFilePath = Path.Combine(gameDir, "mapobject.csv");

      List<MapObject> mapObjects;

      // Read from the mods directory
      using (var reader = new StreamReader(modsFilePath))
      using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
      {
        mapObjects = csv.GetRecords<MapObject>().ToList();
      }

      // Filter the list to only include those that can be shuffled
      List<MapObject> shuffleObjects = mapObjects.Where(mo => mo.shuffle).ToList();

      if (oopsAllGarland)
      {
        // Set all shufflable assets to "mo_ff1_n038_c00"
        foreach (var obj in shuffleObjects)
        {
          obj.asset_name = "mo_ff1_n038_c00";
        }
      }
      else if (shuffleNPCs)
      {
        List<string> assetNames = shuffleObjects.Select(mo => mo.asset_name).ToList();
        assetNames.Shuffle(r1);

        for (int i = 0; i < shuffleObjects.Count; i++)
        {
          shuffleObjects[i].asset_name = assetNames[i];
        }

        // Combine shuffled list back to original list
        foreach (var mapObject in mapObjects)
        {
          if (mapObject.shuffle)
          {
            mapObject.asset_name = shuffleObjects.First(mo => mo.id == mapObject.id).asset_name;
          }
        }
      }

      // Write the modified list directly to the game directory
      using (var writer = new StreamWriter(gameFilePath))
      using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
      {
        // Write header manually
        csv.WriteField("id");
        csv.WriteField("asset_name");
        csv.WriteField("shadow_type");
        csv.WriteField("shadow_name");
        csv.NextRecord();

        // Write records without the shuffle field
        foreach (var mapObject in mapObjects)
        {
          csv.WriteField(mapObject.id);
          csv.WriteField(mapObject.asset_name);
          csv.WriteField(mapObject.shadow_type);
          csv.WriteField(mapObject.shadow_name);
          csv.NextRecord();
        }
      }
    }
  }

  // Extension method for shuffling lists
  public static class ListExtensions
  {
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
      int n = list.Count;
      while (n > 1)
      {
        n--;
        int k = random.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }
}
