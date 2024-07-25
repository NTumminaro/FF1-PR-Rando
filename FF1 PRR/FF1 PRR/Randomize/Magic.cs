using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static FF1_PRR.Common.Common;
using FF1_PRR.Inventory;

namespace FF1_PRR.Inventory
{
  public class Magic
  {
    enum whiteMagic
    {
      wCure = 213,
      wProtect = 214,
      wDia = 215,
      wBlink = 216,
      wInvis = 221,
      wSilence = 222,
      wBlindna = 223,
      wNulShock = 224,
      wCura = 229,
      wNulBlaze = 230,
      wDiara = 231,
      wHeal = 232,
      wPoisona = 237,
      wNulFrost = 238,
      wFear = 239,
      wVox = 240,
      wCuraga = 245,
      wDiaga = 246,
      wLife = 247,
      wHealara = 248,
      wStona = 253,
      wProtera = 254,
      wExit = 255,
      wInvisira = 256,
      wCuraja = 261,
      wNulDeath = 262,
      wDiaja = 263,
      wHealaga = 264,
      wFullLife = 269,
      wNulAll = 270,
      wHoly = 271,
      wDispel = 272
    }

    enum blackMagic
    {
      bFire = 217,
      bFocus = 218,
      bSleep = 219,
      bThunder = 220,
      bBlizzard = 225,
      bTemper = 226,
      bDark = 227,
      bSlow = 228,
      bFira = 233,
      bThundara = 234,
      bHold = 235,
      bFocara = 236,
      bSleepra = 241,
      bConfuse = 242,
      bHaste = 243,
      bBlizzara = 244,
      bFiraga = 249,
      bTeleport = 250,
      bScourge = 251,
      bSlowra = 252,
      bThundaga = 257,
      bQuake = 258,
      bDeath = 259,
      bStun = 260,
      bBlizzaga = 265,
      bSaber = 266,
      bBreak = 267,
      bBlind = 268,
      bFlare = 273,
      bWarp = 274,
      bStop = 275,
      bKill = 276
    }

    public static List<int> bAll = Enum.GetValues(typeof(blackMagic)).Cast<int>().ToList();
    public static List<int> wAll = Enum.GetValues(typeof(whiteMagic)).Cast<int>().ToList();
    public static List<int> all = bAll.Concat(wAll).ToList();

    public static int WHITE_MAGIC = 1;
    public static int BLACK_MAGIC = 2;
    public static int DUPE_CURE_4 = 100;

    private List<ability> records;
    private string file;
    private string productpath;

    public Magic(Random r1, int randoLevel, string fileName, string product, bool keepPermissions)
    {
      file = fileName;
      productpath = product;
      using (StreamReader reader = new StreamReader(fileName))
      using (CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
      {
        records = csv.GetRecords<ability>().ToList();
      }
      shuffleMagic(r1, randoLevel, keepPermissions);
    }

    public List<ability> getRecords()
    {
      return records;
    }

    public void writeToFile()
    {
      using (StreamWriter writer = new StreamWriter(file))
      using (CsvWriter csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
      {
        csv.WriteRecords(records);
      }
    }

    public class ability
    {
      public int id { get; set; }
      public int sort_id { get; set; }
      public int ability_lv { get; set; }
      public int ability_group_id { get; set; }
      public int type_id { get; set; }
      public int attribute_id { get; set; }
      public int attribute_group_id { get; set; }
      public int system_id { get; set; }
      public int use_value { get; set; }
      public int standard_value { get; set; }
      public int adding_hit_rate { get; set; }
      public int valid_hit_rate { get; set; }
      public int weak_hit_rate { get; set; }
      public int attack_count { get; set; }
      public int accuracy_rate { get; set; }
      public int Impact_status { get; set; }
      public int use_job_group_id { get; set; }
      public int condition_group_id { get; set; }
      public int renge_id { get; set; }
      public int menu_renge_id { get; set; }
      public int battle_renge_id { get; set; }
      public int content_flag_group_id { get; set; }
      public int invalid_reflection { get; set; }
      public int invalid_boss { get; set; }
      public int resistance_attribute { get; set; }
      public int battle_effect_asset_id { get; set; }
      public int menu_se_asset_id { get; set; }
      public int reaction_type { get; set; }
      public int menu_function_group_id { get; set; }
      public int battle_function_group_id { get; set; }
      public int buy { get; set; }
      public int sell { get; set; }
      public int sales_not_possible { get; set; }
      public int ability_wait { get; set; }
      public string process_prog { get; set; }
      public int data_a { get; set; }
      public int data_b { get; set; }
      public int data_c { get; set; }
    }

    //RandoLevel 1: Simple
    // Description: In the Simple mode, all spells are shuffled while keeping their original levels and prices. This ensures that the progression of spell acquisition remains consistent with the original game design. The spell levels and prices are maintained in the correct order, ensuring a balanced gameplay experience.
    // RandoLevel 2: Standard
    // Description: The Standard mode shuffles all spells similar to the Simple mode, maintaining their original levels and prices. Additionally, the order of spells in shops is shuffled, with the first shop's order preserved. This introduces variability in spell availability across different shops, enhancing the exploration aspect of the game.
    // RandoLevel 3: Pro
    // Description: The Pro mode shuffles spells and their levels and prices independently. Spells of the same level are distributed across different shops, ensuring there are always four spells per level available. Prices are matched to the original cost of spells, regardless of their shuffled levels. This mode increases the challenge by requiring players to find spells across various shops.
    // RandoLevel 4: Chaos
    // Description: The Chaos mode provides the highest level of randomness. All spells are shuffled, including their type (white or black magic) and job group IDs. This means spells can change their magic type and become available to different job groups. After shuffling, the spells are split into white and black categories for their respective shops. This mode offers an unpredictable and challenging experience, where spell types and job compatibility are completely randomized.

    public void shuffleMagic(Random r1, int randoLevel, bool keepPermissions)
    {
      List<ability> whiteSpells = records.Where(spell => spell.type_id == WHITE_MAGIC && spell.id != DUPE_CURE_4).ToList();
      List<ability> blackSpells = records.Where(spell => spell.type_id == BLACK_MAGIC).ToList();

      switch (randoLevel)
      {
        case 1:
          // Simple
          ShuffleSpells(r1, whiteSpells, blackSpells, keepPermissions);
          break;

        case 2:
          // Standard
          ShuffleSpells(r1, whiteSpells, blackSpells, keepPermissions);
          ShuffleShopsExceptFirst(r1, ref whiteSpells, ref blackSpells);
          break;

        case 3:
          // Pro
          ShuffleSpells(r1, whiteSpells, blackSpells, keepPermissions);
          ShuffleLevelsAndPrices(whiteSpells, r1);
          ShuffleLevelsAndPrices(blackSpells, r1);
          EnsureFourSpellsPerLevel(whiteSpells, r1);
          EnsureFourSpellsPerLevel(blackSpells, r1);
          break;

        case 4:
          // Chaos
          List<ability> allSpells = whiteSpells.Concat(blackSpells).ToList();
          allSpells.Shuffle(r1);
          RandomizeTypeAndJobGroup(allSpells, r1, keepPermissions);
          SplitAndAssignSpells(allSpells, ref whiteSpells, ref blackSpells);
          break;
      }

      UpdateAbilityCsv(whiteSpells);
      UpdateAbilityCsv(blackSpells);
      UpdateProductCsv(r1, randoLevel);
      writeToFile();
    }

    private void ShuffleSpells(Random r1, List<ability> whiteSpells, List<ability> blackSpells, bool keepPermissions)
    {
      // Extract the original levels and prices before shuffling
      List<int> originalWhiteLevels = whiteSpells.Select(spell => spell.ability_lv).ToList();
      List<int> originalWhitePrices = whiteSpells.Select(spell => spell.buy).ToList();
      List<int> originalWhiteJobGroupIds = whiteSpells.Select(spell => spell.use_job_group_id).ToList();

      List<int> originalBlackLevels = blackSpells.Select(spell => spell.ability_lv).ToList();
      List<int> originalBlackPrices = blackSpells.Select(spell => spell.buy).ToList();
      List<int> originalBlackJobGroupIds = blackSpells.Select(spell => spell.use_job_group_id).ToList();

      // Shuffle the spells
      whiteSpells.Shuffle(r1);
      blackSpells.Shuffle(r1);

      // Apply original levels, prices, and job group IDs to the shuffled list
      for (int i = 0; i < whiteSpells.Count; i++)
      {
        whiteSpells[i].ability_lv = originalWhiteLevels[i];
        whiteSpells[i].buy = originalWhitePrices[i];
        if (!keepPermissions)
        {
          whiteSpells[i].use_job_group_id = originalWhiteJobGroupIds[i];
        }
      }

      for (int i = 0; i < blackSpells.Count; i++)
      {
        blackSpells[i].ability_lv = originalBlackLevels[i];
        blackSpells[i].buy = originalBlackPrices[i];
        if (!keepPermissions)
        {
          blackSpells[i].use_job_group_id = originalBlackJobGroupIds[i];
        }
      }
    }

    private void ShuffleShopsExceptFirst(Random r1, ref List<ability> whiteSpells, ref List<ability> blackSpells)
    {
      List<int> whiteShopIds = Product.whiteMagicStores.Skip(1).ToList();
      List<int> blackShopIds = Product.blackMagicStores.Skip(1).ToList();

      whiteShopIds.Shuffle(r1);
      blackShopIds.Shuffle(r1);

      whiteShopIds.Insert(0, Product.whiteMagicStores.First());
      blackShopIds.Insert(0, Product.blackMagicStores.First());

      AssignShopsToSpells(ref whiteSpells, whiteShopIds);
      AssignShopsToSpells(ref blackSpells, blackShopIds);
    }

    private void AssignShopsToSpells(ref List<ability> spells, List<int> shopIds)
    {
      for (int i = 0; i < spells.Count; i++)
      {
        spells[i].sort_id = shopIds[i % shopIds.Count];
      }
    }

    private void ShuffleLevelsAndPrices(List<ability> spells, Random r1)
    {
      List<int> levels = spells.Select(spell => spell.ability_lv).ToList();
      List<int> prices = spells.Select(spell => spell.buy).ToList();

      levels.Shuffle(r1);
      prices.Shuffle(r1);

      for (int i = 0; i < spells.Count; i++)
      {
        spells[i].ability_lv = levels[i];
        spells[i].buy = prices[i];
      }
    }

    private void EnsureFourSpellsPerLevel(List<ability> spells, Random r1)
    {
      var groupedByLevel = spells.GroupBy(spell => spell.ability_lv).ToList();
      foreach (var group in groupedByLevel)
      {
        if (group.Count() != 4)
        {
          var difference = 4 - group.Count();
          for (int i = 0; i < Math.Abs(difference); i++)
          {
            if (difference > 0)
            {
              var randomSpell = spells[r1.Next(spells.Count)];
              randomSpell.ability_lv = group.Key;
            }
            else
            {
              var randomSpell = group.ElementAt(r1.Next(group.Count()));
              randomSpell.ability_lv = spells[r1.Next(spells.Count)].ability_lv;
            }
          }
        }
      }
    }

    private void RandomizeTypeAndJobGroup(List<ability> spells, Random r1, bool keepPermissions)
    {
      List<int> typeIds = new List<int> { WHITE_MAGIC, BLACK_MAGIC };
      List<int> jobGroupIds = spells.Select(spell => spell.use_job_group_id).Distinct().ToList();

      typeIds.Shuffle(r1);
      jobGroupIds.Shuffle(r1);

      for (int i = 0; i < spells.Count; i++)
      {
        spells[i].type_id = typeIds[r1.Next(typeIds.Count)];
        if (!keepPermissions)
        {
          spells[i].use_job_group_id = jobGroupIds[r1.Next(jobGroupIds.Count)];
        }
      }
    }

    private void SplitAndAssignSpells(List<ability> allSpells, ref List<ability> whiteSpells, ref List<ability> blackSpells)
    {
      whiteSpells = allSpells.Where(spell => spell.type_id == WHITE_MAGIC).ToList();
      blackSpells = allSpells.Where(spell => spell.type_id == BLACK_MAGIC).ToList();
    }

    private void UpdateAbilityCsv(List<ability> spells)
    {
      foreach (var spell in spells)
      {
        var originalSpell = records.FirstOrDefault(r => r.id == spell.id);
        if (originalSpell != null)
        {
          originalSpell.ability_lv = spell.ability_lv;
          originalSpell.buy = spell.buy;
          originalSpell.use_job_group_id = spell.use_job_group_id;
          originalSpell.type_id = spell.type_id;
        }
      }
    }

    private void UpdateProductCsv(Random r1, int randoLevel)
    {
      List<ShopItem> shopDB = Product.readShopDB(productpath);
      shopDB = shopDB.FindAll(x => !Product.allMagicStores.Contains(x.group_id));

      shopDB = determineSpells(r1, randoLevel, shopDB);
      shopDB.Sort((x, y) => x.id.CompareTo(y.id));
      Product.writeShopDB(productpath, shopDB);
    }

    private List<ShopItem> determineSpells(Random r1, int randoLevel, List<ShopItem> shopDB)
    {
      List<ShopItem> magicShopDB = new List<ShopItem>();
      int[,] magicMemory = new int[2, 8];
      int productID = Enumerable.Range(1, 1000).Except(shopDB.Select(x => x.id)).First();
      List<int> shopLookup = new List<int>{
                0,0,0,0,
                1,1,1,1,
                2,2,2,2,
                3,3,3,3,
                4,4,4,4,
                5,5,5,5,
                6,6,8,8,
                7,7,9,9
            };
      List<int> wmShops = Product.whiteMagicStores;
      List<int> bmShops = Product.blackMagicStores;

      if (randoLevel == 2 || randoLevel == 4)
      {
        int wmHead = wmShops[0];
        int bmHead = bmShops[0];
        wmShops = wmShops.Skip(1).ToList();
        bmShops = bmShops.Skip(1).ToList();
        wmShops.Shuffle(r1);
        bmShops.Shuffle(r1);
        wmShops.Insert(0, wmHead);
        bmShops.Insert(0, bmHead);
      }

      if (randoLevel == 4)
      {
        shopLookup.Shuffle(r1);
        wmShops.Shuffle(r1);
        bmShops.Shuffle(r1);
      }

      foreach (ability spell in records)
      {
        if (spell.ability_group_id == 1 && spell.id != DUPE_CURE_4)
        {
          List<int> shopType = (spell.type_id == WHITE_MAGIC) ? wmShops : bmShops;
          ShopItem newItem = new ShopItem
          {
            id = productID,
            content_id = spell.id + 208,
            group_id = shopType[shopLookup[(spell.ability_lv - 1) * 4 + magicMemory[spell.type_id - 1, spell.ability_lv - 1]]]
          };
          magicMemory[spell.type_id - 1, spell.ability_lv - 1]++;
          shopDB.Add(newItem);
          productID = Enumerable.Range(1, 1000).Except(shopDB.Select(x => x.id)).First();
        }
      }

      return shopDB;
    }
  }
}
